using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : SaiSingleton<UnitSelections>
{
    [Header("Unit Selections")]
    public UnitSelectable firstUnit;
    public List<UnitSelectable> units;

    public virtual void Clear()
    {
        this.units.Clear();
    }

    public virtual void Select(UnitSelectable unit)
    {
        this.Clear();
        this.units.Add(unit);
        this.firstUnit = unit;
    }
}
