using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayers : SaiMonoBehaviour
{
    public List<TNTPlayer> tntPlayers;

    protected override void Start()
    {
        base.Start();
        this.ListenToNetworkManager();
    }

    protected virtual void ListenToNetworkManager()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }
    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.LogWarning("OnClientConnectedCallback: "+clientId);
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        Debug.LogWarning("OnClientDisconnectCallback: " + clientId);
    }
}
