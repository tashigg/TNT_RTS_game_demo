using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : SaiSingleton<UnitSelections>
{
    [Header("Unit Selections")]
    public UnitSelectable firstUnit;
    public List<UnitSelectable> units;

    private void Update()
    {
        this.ClearSelections();
        this.ChooseOneUnit();
    }

    public virtual void Clear()
    {
        this.units.Clear();
        this.firstUnit = null;
    }

    public virtual void Select(UnitSelectable unit)
    {
        if (!unit.unitCtrl.IsOwner()) return;

        this.Clear();
        this.units.Add(unit);
        this.firstUnit = unit;

        UnitsManager.Instance.SetCurrentBuilding(unit.unitCtrl as BuildingCtrl);
    }

    protected virtual void ChooseOneUnit()
    {
        if (!Input.GetKeyUp(KeyCode.Mouse0)) return;

        Ray ray = GodModeCtrl.instance._camera.ScreenPointToRay(Input.mousePosition);

        int mask = (1 << MyLayerManager.instance.layerUnitSelectable);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, mask))
        {
            UnitSelectable unitSelectable = hit.collider.gameObject.GetComponent<UnitSelectable>();
            this.Select(unitSelectable);
            Debug.LogWarning(hit.collider.gameObject.transform.parent.name);
            return;
        }

        //this.Clear();
    }

    protected virtual void ClearSelections()
    {
        if (!Input.GetKeyUp(KeyCode.Escape)) return;
        this.Clear();
    }
}
