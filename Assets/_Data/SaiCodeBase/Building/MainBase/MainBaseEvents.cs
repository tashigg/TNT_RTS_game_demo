using RTSEngine.Entities;
using RTSEngine.UI;
using Unity.Netcode;
using UnityEngine;

public class MainBaseEvents : NetworkBehaviour
{
    [Header("Main Base Events")]
    public MainBaseCtrl playerCtrl;

    private void Awake()
    {
        this.LoadComponents();
    }

    private void Reset()
    {
        this.LoadComponents();
    }

    protected virtual void LoadComponents()
    {
        this.LoadPlayerCtrl();
    }

    protected virtual void LoadPlayerCtrl()
    {
        if (this.playerCtrl != null) return;
        this.playerCtrl = GetComponent<MainBaseCtrl>();
        Debug.LogWarning(transform.name + ": LoadPlayerCtrl", gameObject);
    }

    public override void OnNetworkSpawn()
    {
        Debug.LogWarning($"OnNetworkSpawn: " + this.OwnerClientId);
        base.OnNetworkSpawn();
        NetworkPlayers.Instance.Add(this.playerCtrl);
        if (this.IsOwner) NetworkPlayers.Instance.SetMe(this.playerCtrl);
    }

    [ServerRpc(RequireOwnership = false)]
    public void CreateUnitServerRpc(ulong networkObjectId, UnitCode unitCode)
    {
        Debug.LogWarning($"CreateUnit: {networkObjectId} {unitCode}", gameObject);
        UnitsManager.Instance.CreateUnitFromServer(networkObjectId, unitCode);
    }
}
