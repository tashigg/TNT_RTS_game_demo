using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RallyPointCtrl : SaiMonoBehaviour
{
    [Header("Rally Point Movement")]
    public BuildingCtrl buildingCtrl;

    private void Update()
    {
        this.ChoosePlace2Move();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBuildingCtrl();
    }

    protected virtual void LoadBuildingCtrl()
    {
        if (this.buildingCtrl != null) return;
        this.buildingCtrl = transform.GetComponentInParent<BuildingCtrl>();
        Debug.LogWarning(transform.name + ": LoadBuildingCtrl", gameObject);
    }

    protected virtual void ChoosePlace2Move()
    {
        if (GodModeCtrl.instance == null) return;
        if (GodModeCtrl.instance.godInput.isMouseRotating) return;
        if (!Input.GetKeyUp(KeyCode.Mouse1)) return;
        if (!this.buildingCtrl.unitSelectable.IsSelected()) return;

        Ray ray = GodModeCtrl.instance._camera.ScreenPointToRay(Input.mousePosition);

        int mask = (1 << MyLayerManager.instance.layerGroundTerrain);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, mask))
        {
            Debug.LogWarning($"ChoosePlace2Move: {hit.point}");

            if (NetworkManager.Singleton.IsServer)
            {
                transform.position = hit.point;
                return;
            }

            ulong buildingNetId = this.buildingCtrl.networkObject.NetworkObjectId;
            NetworkPlayers.Instance.me.playerEvents.MoveRallyPointServerRpc(buildingNetId, hit.point);
        }
    }
}
