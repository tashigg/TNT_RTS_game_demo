using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NetworkObjCtrl : SaiMonoBehaviour
{
    public NetworkObject networkObject;
    public NetworkTransform networkTransform;

    protected override void Start()
    {
        base.Start();
        this.StartNetworkObject();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNetworkObject();
        this.LoadNetworkTransform();
    }

    protected virtual void LoadNetworkObject()
    {
        if (this.networkObject != null) return;
        this.networkObject = transform.GetComponent<NetworkObject>();
        Debug.LogWarning(transform.name + ": LoadNetworkObject", gameObject);
    }

    protected virtual void LoadNetworkTransform()
    {
        if (this.networkTransform != null) return;
        this.networkTransform = transform.GetComponent<NetworkTransform>();
        Debug.LogWarning(transform.name + ": LoadNetworkTransform", gameObject);
    }

    protected virtual void StartNetworkObject()
    {
        if (!this.networkObject.IsOwnedByServer) return;
        Debug.Log(transform.name + ": StartNetworkObject", gameObject);
        //this.networkObject = transform.parent.gameObject.AddComponent<NetworkObject>();
        //this.networkObject.enabled = true;
        //this.networkTransform.enabled = true;
        this.networkObject.Spawn();
    }
}
