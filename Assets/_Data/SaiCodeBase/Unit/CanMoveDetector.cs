using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMoveDetector : SaiMonoBehaviour
{
    [Header("Move detector")]
    public UnitMovementAgent unitMovement;
    public List<Collider> colliders;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUnitMovementAgent();
    }

    protected virtual void LoadUnitMovementAgent()
    {
        if (this.unitMovement != null) return;
        this.unitMovement = GetComponentInParent<UnitMovementAgent>();
        Debug.LogWarning(transform.name + ": LoadUnitMovementAgent", gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        this.colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        this.colliders.Remove(other);
    }
}
