using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingManager : SaiSingleton<BuildingManager>
{
    [Header("Unit Manager")]
    public BuildingCtrl currentBuilding;

    public virtual void SetCurrentBuilding(BuildingCtrl buildingCtrl)
    {
        this.currentBuilding = buildingCtrl;
    }

    public virtual Transform CreateBuildingFromServer(ulong ownerId, BuildingCode buildingCode, Vector3 spawnPos)
    {
        Transform newObj = BuildingSpawner.Instance.Spawn(buildingCode.ToString(), spawnPos);
        NetworkObject newNetObj = newObj.GetComponent<NetworkObject>();
        newNetObj.SpawnWithOwnership(ownerId);
        return newObj;
    }

    public void SpawnFirstBaseServerRpc(ulong ownerId)
    {
        Transform basePoint = BasePoints.instance.points[(int)ownerId];
        Transform newBuilding = BuildingSpawner.Instance.Spawn(BuildingCode.MainBase.ToString(), basePoint.position);
        NetworkObject newNetObj = newBuilding.GetComponent<NetworkObject>();
        newNetObj.SpawnWithOwnership(ownerId);
    }

    public virtual BuildingCtrl FindByNetId(ulong netId)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[netId];
        BuildingCtrl buildingCtrl = netObj.GetComponent<BuildingCtrl>();
        return buildingCtrl;
    }
}
