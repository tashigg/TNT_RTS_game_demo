using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCreateLightInfantry : BaseButton
{
    protected override void OnClick()
    {
        Debug.Log("Btn Create Light Infantry");
        UnitsManager.Instance.CreateUnit(UnitCode.LightInfantry);
    }
}
