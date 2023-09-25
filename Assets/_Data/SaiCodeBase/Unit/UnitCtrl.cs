using Unity.Netcode;
using UnityEngine;

public class UnitCtrl : SaiMonoBehaviour
{
    [Header("Unit Ctrl")]
    public NetworkObject networkObject;
    public UnitSelectable unitSelectable;
    public TeamAssignment teamAssignment;
    public UnitMovementAgent unitMovementAgent;
    public UnitDamageReceiver unitDamageReceiver;

    [SerializeField] protected int clientId = -1;
    public int ClientID => clientId;

    protected override void Start()
    {
        base.Start();
        this.LoadClientId();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNetworkManager();
        this.LoadUnitSelectable();
        this.LoadTeamAssignment();
        this.LoadUnitMovementAgent();
        this.LoadUnitDamageReceiver();
    }

    protected virtual void LoadUnitDamageReceiver()
    {
        if (this.unitDamageReceiver != null) return;
        this.unitDamageReceiver = GetComponentInChildren<UnitDamageReceiver>();
        Debug.LogWarning(transform.name + ": LoadUnitDamageReceiver", gameObject);
    }

    protected virtual void LoadUnitMovementAgent()
    {
        if (this.unitMovementAgent != null) return;
        this.unitMovementAgent = GetComponentInChildren<UnitMovementAgent>();
        Debug.LogWarning(transform.name + ": LoadUnitMovementAgent", gameObject);
    }

    protected virtual void LoadNetworkManager()
    {
        if (this.networkObject != null) return;
        this.networkObject = GetComponent<NetworkObject>();
        Debug.LogWarning(transform.name + ": LoadNetworkManager", gameObject);
    }

    protected virtual void LoadUnitSelectable()
    {
        if (this.unitSelectable != null) return;
        this.unitSelectable = GetComponentInChildren<UnitSelectable>();
        Debug.LogWarning(transform.name + ": LoadUnitSelectable", gameObject);
    }

    public virtual bool IsOwner()
    {
        return this.networkObject.IsOwner;
    }

    protected virtual void LoadClientId()
    {
        if (this.networkObject == null) return;
        if (!this.networkObject.IsSpawned)
        {
            Invoke(nameof(this.LoadClientId), 0.2f);
            return;
        }

        this.clientId = (int)this.networkObject.OwnerClientId;
        this.AssignTeam();
    }

    protected virtual void AssignTeam()
    {
        this.teamAssignment.ColorApply(this.clientId);
    }

    protected virtual void LoadTeamAssignment()
    {
        if (this.teamAssignment != null) return;
        this.teamAssignment = GetComponent<TeamAssignment>();
        Debug.LogWarning(transform.name + ": LoadTeamAssignment", gameObject);
    }
}
