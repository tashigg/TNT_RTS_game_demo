using Unity.Netcode;
using UnityEngine;

public class TasksManager : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void PingServerRpc(int pingCount)
    {
        Debug.LogWarning($"PingServerRpc: {pingCount}");
    }
}
