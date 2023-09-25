using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovementAgent : UnitAbstract
{
    [Header("Unit Movement")]
    public NavMeshAgent navMeshAgent;
    public bool canMove = true;
    [SerializeField] protected bool isWalking = false;
    [SerializeField] protected Transform movePoint;
    [SerializeField] protected float walkLimit = 0.7f;
    [SerializeField] protected float targetDistance = 0f;

    private void Update()
    {
        this.ChoosePlace2Move();
    }

    void LateUpdate()
    {
        this.Moving();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMovePoint();
        this.LoadNavMeshAgent();
    }

    protected virtual void LoadMovePoint()
    {
        if (this.movePoint != null) return;
        this.movePoint = transform.Find("MovePoint");
        Debug.LogWarning(transform.name + ": LoadMovePoint", gameObject);
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

    public virtual void MoveTo(Vector3 position)
    {
        this.movePoint.position = position;
        this.movePoint.parent = null;
    }

    protected virtual void Moving()
    {
        if (!this.CanMove() || this.IsClose2Target())
        {
            this.isWalking = false;
            return;
        }

        this.isWalking = true;
        this.navMeshAgent.SetDestination(this.movePoint.position);
    }

    public virtual bool CanMove()
    {
        return this.canMove && this.navMeshAgent.enabled;
    }

    public virtual bool IsClose2Target()
    {
        Vector3 targetPos = this.movePoint.position;
        targetPos.y = transform.position.y;

        this.targetDistance = Vector3.Distance(transform.position, targetPos);
        return this.targetDistance < this.walkLimit;
    }

    public virtual float TargetDistance()
    {
        return this.targetDistance;
    }

    public virtual void SetMovePosition(Vector3 pos)
    {
        this.movePoint.position = pos;
    }

    protected virtual void ChoosePlace2Move()
    {
        if (GodModeCtrl.instance == null) return;
        if (GodModeCtrl.instance.godInput.isMouseRotating) return;
        if (!Input.GetKeyUp(KeyCode.Mouse1)) return;
        if (!this.unitCtrl.unitSelectable.IsSelected()) return;

        Ray ray = GodModeCtrl.instance._camera.ScreenPointToRay(Input.mousePosition);

        int mask = (1 << MyLayerManager.instance.layerGroundTerrain);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, mask))
        {
            Debug.LogWarning($"ChoosePlace2Move: {hit.point}");

            if (NetworkManager.Singleton.IsServer)
            {
                this.movePoint.position = hit.point;
                return;
            }

            ulong unitId = this.unitCtrl.networkObject.NetworkObjectId;
            NetworkPlayers.Instance.me.playerEvents.MoveUnitServerRpc(unitId, hit.point);
        }
    }
}
