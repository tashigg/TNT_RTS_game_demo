using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectable : BuildingAbstract
{
    //[Header("Unit Selectable")]

    public virtual bool IsSelected()
    {
        return this.buildingCtrl.unitSelectable == UnitSelections.Instance.firstUnit;
    }

    void OnMouseDown()
    {
        UnitSelections.Instance.Select(this);
    }
}
