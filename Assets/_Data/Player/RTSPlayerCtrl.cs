using RTSEngine.EntityComponent;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RTSPlayerCtrl : SaiMonoBehaviour
{
    [Header("Player Ctrl")]
    public NetworkObject networkObject;
    public PlayerEvents playerEvents;
    public GameObject capital;
    public UnitCreator unitCreator;

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        this.LoadMyCapital();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNetworkObject();
        this.LoadPlayerEvents();
    }

    protected virtual void LoadNetworkObject()
    {
        if (this.networkObject != null) return;
        this.networkObject = GetComponent<NetworkObject>();
        Debug.LogWarning(transform.name + ": LoadNetworkObject", gameObject);
    }

    protected virtual void LoadPlayerEvents()
    {
        if (this.playerEvents != null) return;
        this.playerEvents = GetComponent<PlayerEvents>();
        Debug.LogWarning(transform.name + ": LoadPlayerEvents", gameObject);
    }

    protected virtual void LoadMyCapital()
    {
        if (this.capital != null) return;
        string capitalName = $"capital_{this.networkObject.OwnerClientId}";
        this.capital = GameObject.Find(capitalName);
        Debug.LogWarning(transform.name + ": LoadMyCapital "+ capitalName, gameObject);

        if (this.capital == null) return;
        this.unitCreator = this.capital.GetComponentInChildren<UnitCreator>();
    }
}
