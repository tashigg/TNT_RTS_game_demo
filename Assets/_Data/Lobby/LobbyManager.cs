using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using RTSEngine.Faction;

public class LobbyManager : SaiSingleton<LobbyManager>
{
    [Header("Controll")]
    public TeamManager teamManager;

    [Header("Lobby")]
    public int playerCount = 0;
    public int maxPlayers = 7;
    public string uniqueId;
    public string profileName;
    public string playerServiceId;
    public string lobbyId = "";
    public string lobbyCode = "";
    public string lobbyCodeToJoin = "";
    public float nextHeartbeat;
    public float nextLobbyRefresh;
    public string lobbyRandomNumber = "";
    public string isLobbyGameStart = "";
    public string teamString = "";
    public float lobbyRefreshDelay = 1.6f;
    public bool isLobbyHost = false;
    public bool isJoinedLobby = false;
    public bool isInLobby = false;
    public bool isGameStarting = false;
    public bool isGameStarted = false;
    public Lobby lobby;
    public List<LobbyPlayer> players;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
        this.RandomUniqueId();
    }

    void FixedUpdate()
    {
        this.LobbyUpdating();
        this.ClientStartGame();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTeamManager();
    }

    protected virtual void LoadTeamManager()
    {
        if (this.teamManager != null) return;
        this.teamManager = GetComponentInChildren<TeamManager>();
        Debug.LogWarning(transform.name + ": LoadTeamManager", gameObject);
    }

    protected virtual void ClientStartGame()
    {
        if (this.isGameStarted) return;
        if (this.isLobbyGameStart != "True") return;
        this.GameStart();
    }

    protected async void LobbyUpdating()
    {
        if (!this.isInLobby) return;

        if (Time.realtimeSinceStartup >= nextHeartbeat && isLobbyHost)
        {
            nextHeartbeat = Time.realtimeSinceStartup + 15;
            await LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
        }

        if (Time.realtimeSinceStartup >= this.nextLobbyRefresh)
        {
            this.nextLobbyRefresh = Time.realtimeSinceStartup + this.lobbyRefreshDelay;
            this.UpdateLobbyData();
            this.LoadLobbyData();
        }
    }

    protected virtual async void LoadLobbyData()
    {
        this.lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
        if (this.lobby == null) return;
        this.playerCount = this.lobby.Players.Count;
        this.LoadLobbyPlayer(this.lobby.Players);

        if (this.lobby.Data == null) return;

        if (this.lobby.Data.TryGetValue("randomNumber", out DataObject randomNumber))
        {
            this.lobbyRandomNumber = randomNumber.Value;
        }

        if (this.lobby.Data.TryGetValue("isGameStart", out DataObject isGameStart))
        {
            this.isLobbyGameStart = isGameStart.Value;
        }

        if (this.lobby.Data.TryGetValue("team", out DataObject team))
        {
            this.teamString = team.Value;
        }
    }

    protected virtual void LoadLobbyPlayer(List<Player> players)
    {
        Debug.Log("Load Lobby Player");
        string id;
        string name;
        string position;
        LobbyPlayer lobbyPlayer;
        PlayerDataObject playerData;
        foreach (Player player in players)
        {
            id = player.Id;

            playerData = player.Data[LobbyPlayerData.name.ToString()];
            name = playerData.Value;

            playerData = player.Data[LobbyPlayerData.position.ToString()];
            position = playerData.Value;

            lobbyPlayer = this.players.Find(player => player.id == id);
            if (lobbyPlayer == null)
            {
                lobbyPlayer = new LobbyPlayer
                {
                    id = id,
                    name = name,
                    position = LobbyPositionsParser.FromString(position),
                    playerOptions = player.Data,
                };

                this.players.Add(lobbyPlayer);
            }
        }
    }


    protected virtual async void UpdateLobbyData()
    {
        if (!this.isLobbyHost) return;
        Debug.Log("Update Lobby Data");

        UpdateLobbyOptions options = new UpdateLobbyOptions
        {
            Name = this.LobbyName(),
            MaxPlayers = this.maxPlayers,
            IsPrivate = false,
            Data = this.GetLobbyOptionsData(),
        };

        this.lobby = await LobbyService.Instance.UpdateLobbyAsync(this.lobbyId, options);
    }

    protected virtual Dictionary<string, DataObject> GetLobbyOptionsData()
    {
        Dictionary<string, DataObject> lobbyOptionsData = new Dictionary<string, DataObject>()
            {
                {
                    //For testing only
                    "randomNumber", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: Random.Range(1, 999).ToString())
                },
                {
                    "isGameStart", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: this.isGameStarting.ToString())
                },
                {
                    "team", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: this.GetTeamString())
                },
            };

        return lobbyOptionsData;
    }

    protected virtual string GetTeamString()
    {
        string teamString = "";
        string playerString;
        int index = 0;
        foreach (LobbyPlayer lobbyPlayer in this.players)
        {
            playerString = $"{lobbyPlayer.name},capital_{index}";
            teamString += playerString + ";";
            index++;
        }
        return teamString;
    }

    public virtual async void CreateLobby()
    {
        await this.ServiceInit(this.uniqueId);

        var lobbyOptions = new CreateLobbyOptions
        {
            IsPrivate = false,
        };

        string lobbyName = this.LobbyName();
        Debug.Log($"Create {lobbyName}: {this.maxPlayers}");

        PlayerDataObject playerData;
        UpdatePlayerOptions playerOptions = new UpdatePlayerOptions();
        playerOptions.Data = new Dictionary<string, PlayerDataObject>();

        playerData = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, this.profileName);
        playerOptions.Data.Add(LobbyPlayerData.name.ToString(), playerData);

        playerData = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, LobbyPositions.host.ToString());
        playerOptions.Data.Add(LobbyPlayerData.position.ToString(), playerData);

        var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, this.maxPlayers, lobbyOptions);
        await LobbyService.Instance.UpdatePlayerAsync(lobby.Id, this.playerServiceId, playerOptions);

        this.lobbyId = lobby.Id;
        this.lobbyCode = lobby.LobbyCode;
        this.isLobbyHost = true;
        this.isInLobby = true;
    }

    public virtual async void JoinLobby()
    {
        await this.ServiceInit(this.uniqueId);

        PlayerDataObject playerData;
        UpdatePlayerOptions playerOptions = new UpdatePlayerOptions();
        playerOptions.Data = new Dictionary<string, PlayerDataObject>();

        playerData = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, this.profileName);
        playerOptions.Data.Add(LobbyPlayerData.name.ToString(), playerData);

        playerData = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, LobbyPositions.member.ToString());
        playerOptions.Data.Add(LobbyPlayerData.position.ToString(), playerData);

        var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(this.lobbyCodeToJoin);
        await LobbyService.Instance.UpdatePlayerAsync(lobby.Id, this.playerServiceId, playerOptions);

        this.lobbyId = lobby.Id;
        this.lobbyCode = lobby.LobbyCode;
        this.isJoinedLobby = true;
        this.isInLobby = true;
    }

    public virtual async Task ServiceInit(string uniqueId)
    {
        //this.profileName = "profile_" + Random.Range(1111111, 10001000).ToString();
        this.profileName = "profile_" + uniqueId;
        //Debug.Log("ServiceInit: " + this.profileName);

        var options = new InitializationOptions();
        options.SetProfile(this.profileName);
        await UnityServices.InitializeAsync(options);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        this.playerServiceId = AuthenticationService.Instance.PlayerId;
        this.profileName = AuthenticationService.Instance.Profile;
    }

    public virtual async void Leave()
    {
        await LobbyService.Instance.RemovePlayerAsync(this.lobbyId, this.playerServiceId);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }

    protected virtual string LobbyName()
    {
        return this.profileName + "_lobby";
    }

    public virtual void GameStart()
    {
        this.isGameStarting = true;
        string sceneName = "1_Game";
        SceneManager.LoadScene(sceneName);
        this.isGameStarted = true;
    }

    protected virtual void RandomUniqueId()
    {
        this.uniqueId = Random.Range(1000000, 9999999).ToString();
    }
}
