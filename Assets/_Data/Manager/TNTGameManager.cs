using RTSEngine.Entities;
using RTSEngine.Faction;
using RTSEngine.Game;
using System.Collections;
using System.Collections.Generic;
using Tashi.NetworkTransport;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TNTGameManager : SaiSingleton<TNTGameManager>
{
    public TashiNetworkTransport NetworkTransport => NetworkManager.Singleton.NetworkConfig.NetworkTransport as TashiNetworkTransport;
    public NetworkManager networkManager;
    public GameManager gameManager;
    public List<FactionPlayer> factionPlayers;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        this.AssignPlayers2Factions();
        this.GameStart();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNetworkManager();
        this.LoadGameManager();
    }

    protected virtual void LoadNetworkManager()
    {
        if (this.networkManager != null) return;
        this.networkManager = GameObject.Find("TNTNetworkManager").GetComponent<NetworkManager>();
        Debug.LogWarning(transform.name + ": LoadNetworkManager", gameObject);
    }

    protected virtual void LoadGameManager()
    {
        if (this.gameManager != null) return;
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.LogWarning(transform.name + ": LoadGameManager", gameObject);
    }

    protected virtual void GameStart()
    {
        if (!LobbyManager.Instance.isInLobby
            || LobbyManager.Instance.isLobbyHost)
            this.networkManager.StartHost();
        else this.networkManager.StartClient();
    }

    protected virtual async void ReceiveIncomingDetail()
    {
        if (NetworkTransport.SessionHasStarted) return;
        Debug.LogWarning("Receive Incoming Detail");

        string lobbyId = LobbyManager.Instance.lobbyId;
        var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
        var incomingSessionDetails = IncomingSessionDetails.FromUnityLobby(lobby);

        if (incomingSessionDetails.AddressBook.Count == lobby.Players.Count)
        {
            Debug.LogWarning("Update Session Details");
            NetworkTransport.UpdateSessionDetails(incomingSessionDetails);
        }
    }

    protected virtual void AssignPlayers2Factions()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            Invoke(nameof(this.AssignPlayers2Factions), 1f);
            return;
        }
        Debug.Log("AssignPlayers2Factions");

        FactionPlayer factionPlayer;
        List<LobbyPlayer> lobbyPlayers = LobbyManager.Instance.players;
        foreach(LobbyPlayer lobbyPlayer in lobbyPlayers)
        {
            FactionSlot factionSlot = this.GetFreeSlot();
            factionPlayer = new FactionPlayer
            {
                lobbyPlayer = lobbyPlayer,
                factionSlot = factionSlot,
            };
            this.factionPlayers.Add(factionPlayer);
        }
    }

    protected virtual FactionSlot GetFreeSlot()
    {
        FactionPlayer exist;
        foreach (FactionSlot factionSlot in this.gameManager.FactionSlots)
        {
            string name = factionSlot.Data.name;
            exist = this.factionPlayers.Find(factionPlayer => factionPlayer.factionSlot.Data.name == name);
            if (exist.lobbyPlayer != null) continue;
            return factionSlot;
        }

        return null;
    }
}
