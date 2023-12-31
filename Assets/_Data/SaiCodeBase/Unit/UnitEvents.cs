using Unity.Netcode;
using UnityEngine;

public class UnitEvents : NetworkBehaviour
{
    public UnitCtrl unitCtrl;

    private void Reset()
    {
        this.LoadComponents();
    }

    protected virtual void LoadComponents()
    {
        if (this.unitCtrl != null) return;
        this.unitCtrl = GetComponent<UnitCtrl>();
        Debug.LogWarning(transform.name + ": LoadComponents", gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        UnitsManager.Instance.AddMyUnit(this.unitCtrl);
        UnitCounter.Instance.Add();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkSpawn();
        UnitCounter.Instance.Remove();
    }
}
