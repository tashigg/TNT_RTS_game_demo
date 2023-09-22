using UnityEngine;

public class UnitCtrl : SaiMonoBehaviour
{
    [Header("Unit Ctrl")]
    public UnitSelectable unitSelectable;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUnitSelectable();
    }

    protected virtual void LoadUnitSelectable()
    {
        if (this.unitSelectable != null) return;
        this.unitSelectable = GetComponentInChildren<UnitSelectable>();
        Debug.LogWarning(transform.name + ": LoadUnitSelectable", gameObject);
    }
}
