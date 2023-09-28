using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitsManager : SaiSingleton<UnitsManager>
{
    [Header("Unit Manager")]
    public BuildingCtrl currentBuilding;
    [SerializeField] protected List<UnitCtrl> myUnits;
    public List<UnitCtrl> MyUnits => myUnits;

    public virtual void SetCurrentBuilding(BuildingCtrl buildingCtrl)
    {
        this.currentBuilding = buildingCtrl;
    }

    public virtual List<UnitCtrl> GetMyUnits()
    {
        this.ClearMissing();
        return this.myUnits;
    }

    protected virtual void ClearMissing()
    {
        for (int i = this.myUnits.Count - 1; i >= 0; i--)
        {
            if (this.myUnits[i] == null)
            {
                this.myUnits.RemoveAt(i);
            }
        }
    }

    public virtual void CreateUnit(UnitCode unit)
    {
        if (this.currentBuilding == null) return;
        ulong networkObjectId = this.currentBuilding.networkObject.NetworkObjectId;
        NetworkPlayers.Instance.me.playerEvents.CreateUnitServerRpc(networkObjectId, unit);
    }

    public virtual void CreateUnitFromServer(ulong netObjectId, UnitCode unitCode)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[netObjectId];
        BuildingCtrl buildingCtrl = netObj.GetComponent<BuildingCtrl>();
        Vector3 spawnPos = buildingCtrl.spawnPoint.position;

        Debug.LogWarning($"CreateUnitFromServer {netObj.name} => {unitCode} {spawnPos}");

        Transform newObj = UnitSpawner.Instance.Spawn(unitCode.ToString(), spawnPos);
        newObj.gameObject.SetActive(true);
        UnitMoveableCtrl unitMoveableCtrl = newObj.GetComponent<UnitMoveableCtrl>();
        unitMoveableCtrl.unitMovement.MoveTo(buildingCtrl.rallyPoint.transform.position);

        ulong ownerId = netObj.OwnerClientId;
        NetworkObject newNetObj = newObj.GetComponent<NetworkObject>();
        newNetObj.SpawnWithOwnership(ownerId);
    }

    public virtual void AddMyUnit(UnitCtrl unitCtrl)
    {
        ulong clientId = unitCtrl.networkObject.OwnerClientId;
        ulong myClientId = NetworkManager.Singleton.LocalClientId;
        if (clientId != myClientId) return;
        this.myUnits.Add(unitCtrl);
    }
}
