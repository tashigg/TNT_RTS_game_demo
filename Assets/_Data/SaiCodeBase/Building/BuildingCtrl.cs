using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingCtrl : UnitCtrl
{
    [Header("Building")]
    public Transform spawnPoint;
    public RallyPointCtrl rallyPoint;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawnPoint();
        this.LoadRallyPointCtrl();
    }

    protected virtual void LoadSpawnPoint()
    {
        if (this.spawnPoint != null) return;
        this.spawnPoint = transform.Find("SpawnPoint");
        Debug.LogWarning(transform.name + ": LoadSpawnPoint", gameObject);
    }

    protected virtual void LoadRallyPointCtrl()
    {
        if (this.rallyPoint != null) return;
        this.rallyPoint = GetComponentInChildren<RallyPointCtrl>();
        Debug.LogWarning(transform.name + ": LoadRallyPointCtrl", gameObject);
    }
}
