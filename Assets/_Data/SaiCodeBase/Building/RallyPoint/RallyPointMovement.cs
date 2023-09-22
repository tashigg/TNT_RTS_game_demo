using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPointMovement : BuildingAbstract
{
    [Header("Rally Point Movement")]
    [SerializeField] protected bool isMoveable = false;


    private void Update()
    {
        this.ChoosePlace2Move();
    }

    protected virtual void ChoosePlace2Move()
    {
        if (GodModeCtrl.instance.godInput.isMouseRotating) return;
        if (!Input.GetKeyUp(KeyCode.Mouse1)) return;
        if (!this.buildingCtrl.unitSelectable.IsSelected()) return;

        Ray ray = GodModeCtrl.instance._camera.ScreenPointToRay(Input.mousePosition);

        int mask = (1 << MyLayerManager.instance.layerGroundTerrain);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, mask))
        {
            transform.position = hit.point;
        }
    }
}
