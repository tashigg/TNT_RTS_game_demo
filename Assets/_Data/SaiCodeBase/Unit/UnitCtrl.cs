using Unity.Netcode;
using UnityEngine;

public class UnitCtrl : SaiMonoBehaviour
{
    [Header("Unit Ctrl")]
    public NetworkObject networkObject;
    public UnitSelectable unitSelectable;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNetworkManager();
        this.LoadUnitSelectable();
    }

    protected virtual void LoadNetworkManager()
    {
        if (this.networkObject != null) return;
        this.networkObject = GetComponent<NetworkObject>();
        Debug.LogWarning(transform.name + ": LoadNetworkManager", gameObject);
    }

    protected virtual void LoadUnitSelectable()
    {
        if (this.unitSelectable != null) return;
        this.unitSelectable = GetComponentInChildren<UnitSelectable>();
        Debug.LogWarning(transform.name + ": LoadUnitSelectable", gameObject);
    }

    public virtual bool IsOwner()
    {
        return this.networkObject.IsOwner;
    }
}
