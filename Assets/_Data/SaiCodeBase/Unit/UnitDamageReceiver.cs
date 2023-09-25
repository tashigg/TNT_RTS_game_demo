using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitDamageReceiver : DamageReceiver
{
    [Header("Unit")]
    public UnitCtrl unitCtrl;

    protected override void Start()
    {
        base.Start();
        //this.Test();
    }

    protected virtual void Test()
    {
        this.Deduct(1);
        Invoke(nameof(this.Test), 1);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUnitCtrl();
    }

    protected virtual void LoadUnitCtrl()
    {
        if (this.unitCtrl != null) return;
        this.unitCtrl = GetComponentInParent<UnitCtrl>();
        Debug.LogWarning(transform.name + ": LoadUnitCtrl", gameObject);
    }


    protected override void OnDead()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        Destroy(this.unitCtrl.gameObject);
        //TODO: you can implement pooling object despawn here
    }
}
