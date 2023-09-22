using UnityEngine;

public abstract class UnitAbstract : SaiMonoBehaviour
{
    [Header("Unit Abstract")]
    public UnitCtrl unitCtrl;

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
}
