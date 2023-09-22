using UnityEngine;

public class UnitMoveableCtrl: UnitCtrl
{
    [Header("Unit Moveable Ctrl")]
    public UnitMovementAgent unitMovement;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.UnitMovementAgent();
    }

    protected virtual void UnitMovementAgent()
    {
        if (this.unitMovement != null) return;
        this.unitMovement = GetComponentInChildren<UnitMovementAgent>();
        Debug.LogWarning(transform.name + ": UnitMovementAgent", gameObject);
    }
}
