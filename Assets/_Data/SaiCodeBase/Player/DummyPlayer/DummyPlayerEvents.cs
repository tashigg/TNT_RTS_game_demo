using RTSEngine.Entities;
using RTSEngine.UI;
using Unity.Netcode;
using UnityEngine;

public class DummyPlayerEvents : NetworkBehaviour
{
    [Header("Main Base Events")]
    public DummyPlayerCtrl playerCtrl;

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
        this.playerCtrl = GetComponent<DummyPlayerCtrl>();
        Debug.LogWarning(transform.name + ": LoadPlayerCtrl", gameObject);
    }

    public override void OnNetworkSpawn()
    {
        //Debug.LogWarning($"OnNetworkSpawn: " + this.OwnerClientId);
        base.OnNetworkSpawn();
        NetworkPlayers.Instance.Add(this.playerCtrl);
        if (this.IsOwner) NetworkPlayers.Instance.SetMe(this.playerCtrl);
    }

    [ServerRpc]
    public void CreateUnitServerRpc(ulong networkObjectId, UnitCode unitCode)
    {
        Debug.LogWarning($"CreateUnit: {networkObjectId} {unitCode}", gameObject);
        UnitsManager.Instance.CreateUnitFromServer(networkObjectId, unitCode);
    }

    [ServerRpc]
    public void SpawnFirstBaseServerRpc()
    {
        ulong ownerId = this.playerCtrl.networkObject.OwnerClientId;
        BuildingManager.Instance.SpawnFirstBaseServerRpc(ownerId);
    }

    [ServerRpc]
    public void MoveRallyPointServerRpc(ulong buildingNetId, Vector3 position)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[buildingNetId];
        BuildingCtrl buildingCtrl = netObj.GetComponent<BuildingCtrl>();
        buildingCtrl.rallyPoint.transform.position = position;
    }

    [ServerRpc]
    public void MoveUnitServerRpc(ulong unitId, Vector3 position)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[unitId];
        UnitCtrl unitCtrl = netObj.GetComponent<UnitCtrl>();
        unitCtrl.unitMovementAgent.SetMovePosition(position);
    }
}
