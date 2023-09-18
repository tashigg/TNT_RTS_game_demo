using RTSEngine.Entities;
using RTSEngine.UI;
using Unity.Netcode;
using UnityEngine;

public class PlayerEvents : NetworkBehaviour
{
    [Header("Player Events")]
    public RTSPlayerCtrl playerCtrl;
    public Unit villager;

    private void Awake()
    {
        this.LoadComponents();
    }

    private void Reset()
    {
        this.LoadComponents();
    }

    protected virtual void LoadComponents()
    {
        this.LoadPlayerCtrl();
    }

    protected virtual void LoadPlayerCtrl()
    {
        if (this.playerCtrl != null) return;
        this.playerCtrl = GetComponent<RTSPlayerCtrl>();
        Debug.LogWarning(transform.name + ": LoadPlayerCtrl", gameObject);
    }

    public override void OnNetworkSpawn()
    {
        Debug.LogWarning($"OnNetworkSpawn: " + this.OwnerClientId);
        base.OnNetworkSpawn();
        TNTNetworkPlayers.Instance.playerCtrls.Add(this.playerCtrl);
        if (this.IsOwner) TNTNetworkPlayers.Instance.me = this.playerCtrl;
    }

    [ServerRpc(RequireOwnership = false)]
    public void CreateUnitServerRpc(int factionId, string taskCode)
    {
        Debug.LogWarning($"CreateUnit: {factionId} {taskCode}", gameObject);

        RTSPlayerCtrl playerCtrl = TNTNetworkPlayers.Instance.FindByFactionId(factionId);
        Vector3 spawnPos = playerCtrl.unitCreator.SpawnPosition;

        InitUnitParameters unitParam = new InitUnitParameters
        {
            factionID = factionId,
            free = false,

            setInitialHealth = false,

            giveInitResources = true,

            rallypoint = playerCtrl.unitCreator.factionEntity.Rallypoint,
            creatorEntityComponent = playerCtrl.unitCreator,

            useGotoPosition = true,
            gotoPosition = spawnPos,

            isSquad = true,
            squadCount = 1,

            playerCommand = true
        };
        TNTGameManager.Instance.unitManager.CreateUnitLocal(this.villager, spawnPos, Quaternion.identity, unitParam);
    }
}
