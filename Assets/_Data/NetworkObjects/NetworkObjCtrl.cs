using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjCtrl : SaiMonoBehaviour
{
    public NetworkObject networkObject;

    protected override void Start()
    {
        base.Start();
        this.StartNetworkObject();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        //this.LoadNetworkObject();
    }

    protected virtual void LoadNetworkObject()
    {
        if (this.networkObject != null) return;
        this.networkObject = GetComponent<NetworkObject>();
        this.networkObject.enabled = false;
        Debug.LogWarning(transform.name + ": LoadNetworkObject", gameObject);
    }

    protected virtual void StartNetworkObject()
    {
        this.networkObject = transform.parent.gameObject.AddComponent<NetworkObject>();
    }
}
