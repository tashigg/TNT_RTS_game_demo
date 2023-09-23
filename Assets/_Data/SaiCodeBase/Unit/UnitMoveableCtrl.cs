using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class UnitMoveableCtrl: UnitCtrl
{
    [Header("Unit Moveable Ctrl")]
    public NavMeshAgent navMeshAgent;
    public UnitMovementAgent unitMovement;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.DisableClientNavAgnet();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.UnitNavMeshAgent();
        this.UnitMovementAgent();
    }

    protected virtual void UnitNavMeshAgent()
    {
        if (this.navMeshAgent != null) return;
        this.navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.LogWarning(transform.name + ": UnitNavMeshAgent", gameObject);
    }

    protected virtual void UnitMovementAgent()
    {
        if (this.unitMovement != null) return;
        this.unitMovement = GetComponentInChildren<UnitMovementAgent>();
        Debug.LogWarning(transform.name + ": UnitMovementAgent", gameObject);
    }

    protected virtual void DisableClientNavAgnet()
    {
        if (NetworkManager.Singleton.IsServer) return;
        this.navMeshAgent.enabled = false;
    }
}
