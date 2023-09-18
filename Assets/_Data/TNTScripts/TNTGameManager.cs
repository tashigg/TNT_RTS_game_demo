using RTSEngine.Entities;
using RTSEngine.Faction;
using RTSEngine.Game;
using RTSEngine.UnitExtension;
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
    public GameManager gameManager;
    public UnitManager unitManager;
    public List<FactionPlayer> factionPlayers;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        this.EnableCapitals();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameManager();
    }

    protected virtual void LoadGameManager()
    {
        if (this.gameManager != null) return;
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.unitManager = this.gameManager.GetComponentInChildren<UnitManager>();
        Debug.LogWarning(transform.name + ": LoadGameManager", gameObject);
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

    protected virtual void EnableCapitals()
    {
        for (int i = 0; i < LobbyManager.Instance.playerCount; i++)
        {
            FactionSlot factionSlot = this.gameManager.FactionSlots[i] as FactionSlot;
            factionSlot.enabled = true;
        }
    }
}
