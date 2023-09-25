using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetGameManager : SaiSingleton<NetGameManager>
{
    [SerializeField] bool gameStarted = false;
    public bool GameStarted => gameStarted;

    protected override void Start()
    {
        base.Start();
        this.GameStart();
    }

    protected virtual void GameStart()
    {
        this.gameStarted = true;
        this.SpawnPlayers();
        this.SpawnFirstBase();
    }

    protected virtual void SpawnPlayers()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        int playerCount = LobbyManager.Instance.playerCount;
        //Debug.LogWarning("SpawnPlayers: " + playerCount);
        for (int i = 0; i < playerCount; i++)
        {
            Transform newObj = BuildingSpawner.Instance.Spawn(BuildingCode.DummyPlayer.ToString(), Vector3.zero);
            NetworkObject networkObject = newObj.GetComponent<NetworkObject>();
            networkObject.SpawnWithOwnership((ulong)i, false);
        }
    }

    protected virtual void SpawnFirstBase()
    {
        if (NetworkPlayers.Instance.me == null)
        {
            Invoke(nameof(this.SpawnFirstBase), 0.5f);
            return;
        }

        NetworkPlayers.Instance.me.playerEvents.SpawnFirstBaseServerRpc();
    }
}
