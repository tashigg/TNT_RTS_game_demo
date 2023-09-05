using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RTSNetworkBehaviour : NetworkBehaviour
{
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [ClientRpc]
    public void PongClientRpc(string playerId, string team)
    {
        Debug.LogWarning($"playerId {playerId}, team {team}");
    }

    [ServerRpc]
    public void PingServerRpc(string playerId)
    {
        Debug.LogWarning($"Player Join {playerId}");
    }
}
