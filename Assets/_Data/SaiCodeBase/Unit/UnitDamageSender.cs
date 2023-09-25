using System.Collections.Generic;
using UnityEngine;

public class UnitDamageSender: DamageSender
{
    [Header("Unit")]
    public SphereCollider _collider;
    public UnitCtrl unitCtrl;
    [SerializeField] protected UnitCtrl targetUnitCtrl;
    [SerializeField] protected float attackRange = 4f;
    [SerializeField] protected float attackTimer = 0;
    [SerializeField] protected float attackDelay = 1f;
    [SerializeField] protected List<UnitCtrl> enemies;

    private void FixedUpdate()
    {
        this.TargetFinding();
        this.FollowTarget();
        this.Attacking();
    }

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
        if (this._collider != null) return;
        this._collider = GetComponent<SphereCollider>();
        this._collider.center = new Vector3(0, 0.5f, 0);
        this._collider.radius = this.attackRange;
        this._collider.isTrigger = true;
        Debug.LogWarning(transform.name + ": LoadBoxCollider", gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        UnitDamageReceiver unitDamageReceiver = other.transform.GetComponent<UnitDamageReceiver>();
        if (unitDamageReceiver == null) return;
        if (unitDamageReceiver.unitCtrl.ClientID == this.unitCtrl.ClientID) return;
        if (this.enemies.Contains(unitDamageReceiver.unitCtrl)) return;
        this.enemies.Add(unitDamageReceiver.unitCtrl);
    }

    private void OnTriggerExit(Collider other)
    {
        UnitDamageReceiver unitDamageReceiver = other.transform.GetComponent<UnitDamageReceiver>();
        if (unitDamageReceiver == null) return;
        if (unitDamageReceiver.unitCtrl.ClientID == this.unitCtrl.ClientID) return;
        this.enemies.Remove(unitDamageReceiver.unitCtrl);
    }

    protected virtual void Attacking()
    {
        this.attackTimer += Time.fixedDeltaTime;
        if (this.attackTimer < this.attackDelay) return;
        if (this.IsTargetDead()) return;
        this.attackTimer = 0;

        this.Send(this.targetUnitCtrl.unitDamageReceiver);

        Vector3 pos = this.targetUnitCtrl.transform.position;
        this.CreateImpactFX(pos);
    }

    protected virtual bool IsTargetDead()
    {
        if (this.targetUnitCtrl == null) return true;
        if (this.targetUnitCtrl == false) return true;
        return false;
    }

    protected virtual void FollowTarget()
    {
        if (this.IsTargetDead() || !this.IsTargetInRange())
        {
            this.targetUnitCtrl = null;
            return;
        }

        if (!this.unitCtrl.unitMovementAgent.IsClose2Target()) return;
        this.unitCtrl.unitMovementAgent.SetMovePosition(this.targetUnitCtrl.transform.position);
    }

    protected virtual bool IsTargetInRange()
    {
        return this.enemies.Contains(this.targetUnitCtrl);
    }

    protected virtual void TargetFinding()
    {
        if (!this.IsTargetDead()) return;
        if (!this.unitCtrl.unitMovementAgent.IsClose2Target()) return;
        float minDistance = Mathf.Infinity;
        float distance;
        UnitCtrl unitNearest = null;
        this.ClearMissingTarget();
        foreach (UnitCtrl enemy in this.enemies)
        {
            distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                unitNearest = enemy;
            }
        }

        this.targetUnitCtrl = unitNearest;
    }

    protected virtual void ClearMissingTarget()
    {
        for (int i = this.enemies.Count - 1; i >= 0; i--)
        {
            if (this.enemies[i] == null)
            {
                this.enemies.RemoveAt(i);
            }
        }
    }
}
