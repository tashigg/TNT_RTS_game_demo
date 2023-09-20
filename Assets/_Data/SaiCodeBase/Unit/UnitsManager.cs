using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitsManager : SaiSingleton<UnitsManager>
{
    [Header("Unit Manager")]
    public BuildingCtrl currentBuilding;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadDefaultBuilding();
    }

    public virtual void SetCurrentBuilding(BuildingCtrl buildingCtrl)
    {
        this.currentBuilding = buildingCtrl;
    }

    protected virtual void LoadDefaultBuilding()
    {
        if(NetworkPlayers.Instance.me == null)
        {
            Invoke(nameof(this.LoadDefaultBuilding), 0.5f);
            return;
        }

        this.SetCurrentBuilding(NetworkPlayers.Instance.me);
    }

    public virtual void CreateUnit(UnitCode unit)
    {
        ulong networkObjectId = this.currentBuilding.networkObject.NetworkObjectId;
        NetworkPlayers.Instance.me.mainBaseEvents.CreateUnitServerRpc(networkObjectId, unit);
    }

    public virtual void CreateUnitFromServer(ulong netObjectId, UnitCode unitCode)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[netObjectId];
        BuildingCtrl buildingCtrl = netObj.GetComponent<BuildingCtrl>();
        Vector3 spawnPos = buildingCtrl.spawnPoint.position;
        Transform newObj = UnitSpawner.Instance.Spawn(unitCode.ToString(), spawnPos);
        NetworkObject newNetObj = newObj.GetComponent<NetworkObject>();
        newObj.gameObject.SetActive(true);
        newNetObj.Spawn();
    }
}
