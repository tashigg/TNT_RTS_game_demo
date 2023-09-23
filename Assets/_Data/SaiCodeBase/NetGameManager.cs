using System.Collections;
using System.Collections.Generic;
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
        this.SpawnFirstBase();
    }

    protected virtual void SpawnFirstBase()
    {
        Debug.LogWarning($"try to SpawnFirstBase", gameObject);

        if (NetworkPlayers.Instance.me == null)
        {
            Invoke(nameof(this.SpawnFirstBase), 0.5f);
            return;
        }

        NetworkPlayers.Instance.me.playerEvents.SpawnFirstBaseServerRpc();
    }
}
