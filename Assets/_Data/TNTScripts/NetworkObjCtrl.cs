using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NetworkObjCtrl : SaiMonoBehaviour
{
    [Header("Network Obj")]
    public NetworkObject networkObject;
    public NetworkTransform networkTransform;
    public int factionID = 1;

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
        Debug.Log(transform.name + ": StartNetworkObject", gameObject);
        if (NetworkManager.Singleton.IsServer) this.StartServerObject();
        else StartClientObject();

    }

    protected virtual void StartServerObject()
    {
        this.networkObject.Spawn();
    }

    protected virtual void StartClientObject()
    {
        //Todo
    }
}
