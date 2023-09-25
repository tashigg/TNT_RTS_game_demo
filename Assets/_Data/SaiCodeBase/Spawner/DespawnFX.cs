using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnFX : DespawnByTime
{
    public override void DespawnObject()
    {
        FXSpawner.Instance.Despawn(transform.parent);
    }
}
