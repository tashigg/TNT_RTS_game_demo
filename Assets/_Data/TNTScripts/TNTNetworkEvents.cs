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

public class TNTNetworkEvents : SaiSingleton<TNTGameManager>
{
    public NetworkManager networkManager;

    protected override void Start()
    {
        base.Start();
        this.RegistryEvents();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNetworkManager();
    }

    protected virtual void LoadNetworkManager()
    {
        if (this.networkManager != null) return;
        this.networkManager = GetComponent<NetworkManager>();
        Debug.LogWarning(transform.name + ": LoadNetworkManager", gameObject);
    }

    protected virtual void RegistryEvents()
    {
        this.networkManager.OnClientConnectedCallback += this._onClientConnectedCallback;
        this.networkManager.OnClientDisconnectCallback += this._onClientDisconnectCallback;
    }

    protected virtual void _onClientConnectedCallback(ulong number)
    {
        Debug.Log("_onClient Connected Callback: " + number);
    }

    protected virtual void _onClientDisconnectCallback(ulong number)
    {
        Debug.Log("_onClient Disconnect Callback: " + number);
    }
}
