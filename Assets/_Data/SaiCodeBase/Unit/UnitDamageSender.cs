using UnityEngine;

public class UnitDamageSender: DamageSender
{
    [Header("Unit")]
    public BoxCollider boxCollider;
    public UnitCtrl unitCtrl;
    [SerializeField] protected UnitCtrl targetUnitCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUnitCtrl();
        this.LoadBoxCollider();
    }

    protected virtual void LoadUnitCtrl()
    {
        if (this.unitCtrl != null) return;
        this.unitCtrl = GetComponentInParent<UnitCtrl>();
        Debug.LogWarning(transform.name + ": LoadUnitCtrl", gameObject);
    }

    protected virtual void LoadBoxCollider()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = GetComponent<BoxCollider>();
        this.boxCollider.center = new Vector3(0, 0.5f, 1);
        this.boxCollider.isTrigger = true;
        Debug.LogWarning(transform.name + ": LoadBoxCollider", gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning($"On Trigger Enter {transform.parent.name} {other.transform.parent.name}");
        UnitCtrl unitCtrl = other.transform.parent.GetComponent<UnitCtrl>();
        if (unitCtrl == null) return;
        if (unitCtrl.ClientID == this.unitCtrl.ClientID) return;
        Debug.LogWarning($"Target ========================");

        this.targetUnitCtrl = unitCtrl;
    }

}
