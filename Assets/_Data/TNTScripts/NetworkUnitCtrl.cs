using RTSEngine.Entities;
using RTSEngine.EntityComponent;
using Unity.Netcode;
using UnityEngine;

public class NetworkUnitCtrl : NetworkObjCtrl
{
    [Header("Network Unit")]
    public Unit unit;
    public UnitMovement unitMovement;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUnit();
        this.LoadUnitMovement();
    }

    protected virtual void LoadUnit()
    {
        if (this.unit != null) return;
        this.unit = transform.GetComponent<Unit>();
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

        InitUnitParameters unitInitParams = new()
        {
            factionID = this.factionID,
            free = false,
            setInitialHealth = false,
            rallypoint = null,
            giveInitResources = true
        };
        //this.unit.Init(TNTGameManager.Instance.gameManager, unitInitParams);
    }

    protected override void StartClientObject()
    {
        this.unitMovement.enabled = false;
    }
}
