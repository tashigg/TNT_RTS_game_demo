using RTSEngine.Entities;
using RTSEngine.EntityComponent;
using RTSEngine.Health;
using RTSEngine.Selection;
using Unity.Netcode;
using UnityEngine;

public class NetworkUnitCtrl : NetworkObjCtrl
{
    [Header("Network Unit")]
    public Unit unit;
    public UnitHealth unitHealth;
    public UnitMovement unitMovement;
    public EntitySelectionCollider entitySelectionCollider;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUnit();
        this.LoadUnitMovement();
        this.LoadSelectionCollider();
    }

    protected virtual void LoadUnit()
    {
        if (this.unit != null) return;
        this.unit = transform.GetComponent<Unit>();
        this.unitHealth = GetComponentInChildren<UnitHealth>();
        this.unit.Health = this.unitHealth;
        Debug.LogWarning(transform.name + ": LoadUnit", gameObject);
    }

    protected virtual void LoadUnitMovement()
    {
        if (this.unitMovement != null) return;
        this.unitMovement = GetComponentInChildren<UnitMovement>();
        Debug.LogWarning(transform.name + ": LoadUnitMovement", gameObject);
    }

    protected override void StartServerObject()
    {
        this.networkObject.Spawn();
   }

    protected override void StartClientObject()
    {
        base.StartClientObject();
        this.unitMovement.enabled = false;
    }

    protected virtual void LoadSelectionCollider()
    {
        if (this.entitySelectionCollider != null) return;
        this.entitySelectionCollider = GetComponentInChildren<EntitySelectionCollider>();
        this.entitySelectionCollider.Entity = this.unit;
        Debug.LogWarning(transform.name + ": LoadSelectionCollider", gameObject);
    }
}
