using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TxtTotalUnits : BaseText
{

    private void FixedUpdate()
    {
        this.LoadTotalUnits();
    }

    protected virtual void LoadTotalUnits()
    {
        this.textMeshPro.text = UnitCounter.Instance.UnitCount.ToString("N0");
    }
}
