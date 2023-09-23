using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : Spawner
{
    private static UnitSpawner instance;
    public static UnitSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (UnitSpawner.instance != null) Debug.LogError("Only 1 BulletSpawner allow to exist");
        UnitSpawner.instance = this;
    }
}
