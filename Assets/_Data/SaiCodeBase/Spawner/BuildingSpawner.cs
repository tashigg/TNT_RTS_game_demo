using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : Spawner
{
    private static BuildingSpawner instance;
    public static BuildingSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (BuildingSpawner.instance != null) Debug.LogError("Only 1 BuildingSpawner allow to exist");
        BuildingSpawner.instance = this;
    }
}
