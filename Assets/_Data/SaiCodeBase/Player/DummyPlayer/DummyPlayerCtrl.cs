using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DummyPlayerCtrl : SaiMonoBehaviour
{
    [Header("Dummy Player")]
    public NetworkObject networkObject;
    public DummyPlayerEvents playerEvents;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNetworkObject();
        this.LoadDummyPlayerCtrl();
    }

    protected virtual void LoadDummyPlayerCtrl()
    {
        if (this.playerEvents != null) return;
        this.playerEvents = GetComponent<DummyPlayerEvents>();
        Debug.LogWarning(transform.name + ": LoadDummyPlayerCtrl", gameObject);
    }

    protected virtual void LoadNetworkObject()
    {
        if (this.networkObject != null) return;
        this.networkObject = GetComponent<NetworkObject>();
        Debug.LogWarning(transform.name + ": LoadNetworkObject", gameObject);
    }
}
