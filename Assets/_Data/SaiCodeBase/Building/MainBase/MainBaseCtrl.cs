using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainBaseCtrl : BuildingCtrl
{
    [Header("Main Base")]
    public MainBaseEvents mainBaseEvents;
    [SerializeField] protected int clientId = -1;
    public int ClientID => clientId;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadClientId();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMainBaseEvents();
    }

    protected virtual void LoadMainBaseEvents()
    {
        if (this.mainBaseEvents != null) return;
        this.mainBaseEvents = GetComponent<MainBaseEvents>();
        Debug.LogWarning(transform.name + ": LoadMainBaseEvents", gameObject);
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
