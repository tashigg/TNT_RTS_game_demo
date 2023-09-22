using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingCtrl : UnitCtrl
{
    [Header("Building")]
    public NetworkObject networkObject;
    public Transform spawnPoint;
    public Transform rallyPoint;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNetworkManager();
        this.LoadSpawnPoint();
        this.LoadRallyPoint();
    }

    protected virtual void LoadNetworkManager()
    {
        if (this.networkObject != null) return;
        this.networkObject = GetComponent<NetworkObject>();
        Debug.LogWarning(transform.name + ": LoadNetworkManager", gameObject);
    }

    protected virtual void LoadSpawnPoint()
    {
        if (this.spawnPoint != null) return;
        this.spawnPoint = transform.Find("SpawnPoint");
        Debug.LogWarning(transform.name + ": LoadSpawnPoint", gameObject);
    }

    protected virtual void LoadRallyPoint()
    {
        if (this.rallyPoint != null) return;
        this.rallyPoint = transform.Find("RallyPoint");
        Debug.LogWarning(transform.name + ": LoadRallyPoint", gameObject);
    }
}
