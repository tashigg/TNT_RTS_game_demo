using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainBaseCtrl : BuildingCtrl
{
    [Header("Main Base")]
    [SerializeField] protected int clientId = -1;
    public int ClientID => clientId;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadClientId();
    }

    protected virtual void LoadClientId()
    {
        if (!this.networkObject.IsSpawned)
        {
            Invoke(nameof(this.LoadClientId), 0.2f);
            return;
        }

        this.clientId = (int) this.networkObject.OwnerClientId;
    }
}
