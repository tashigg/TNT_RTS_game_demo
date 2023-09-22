using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovementAgent : UnitAbstract
{
    [Header("Unit Movement")]
    public NavMeshAgent navMeshAgent;
    public bool isWalking = false;
    [SerializeField] protected Transform target;
    [SerializeField] protected float walkLimit = 0.7f;
    [SerializeField] protected float targetDistance = 0f;

    void FixedUpdate()
    {
        this.Moving();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNavMeshAgent();
    }

    protected virtual void LoadNavMeshAgent()
    {
        if (this.navMeshAgent !=  null) return;
        this.navMeshAgent = GetComponentInParent<NavMeshAgent>();
        this.navMeshAgent.speed = 7;
        this.navMeshAgent.stoppingDistance = 2;
        this.navMeshAgent.acceleration = 25;
        this.navMeshAgent.angularSpeed = 7777;
        Debug.LogWarning(transform.name + ": LoadNavMeshAgent", gameObject);
    }

    public virtual Transform GetTarget()
    {
        return this.target;
    }

    public virtual void SetTarget(Transform trans)
    {
        this.target = trans;

        if (this.target == null)
        {
            this.navMeshAgent.enabled = false;
        }
        else
        {
            this.navMeshAgent.enabled = true;
            this.IsClose2Target();
        }
    }

    protected virtual void Moving()
    {
        if (this.target == null || this.IsClose2Target())
        {
            this.isWalking = false;
            return;
        }

        this.isWalking = true;
        this.navMeshAgent.SetDestination(this.target.position);
    }

    public virtual bool IsClose2Target()
    {
        if (this.target == null) return false;

        Vector3 targetPos = this.target.position;
        targetPos.y = transform.position.y;

        this.targetDistance = Vector3.Distance(transform.position, targetPos);
        return this.targetDistance < this.walkLimit;
    }

    public virtual float TargetDistance()
    {
        return this.targetDistance;
    }
}
