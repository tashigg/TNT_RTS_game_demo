using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectable : UnitAbstract
{
    //[Header("Unit Selectable")]

    public virtual bool IsSelected()
    {
        return UnitSelections.Instance.Contains(this);
    }

}
