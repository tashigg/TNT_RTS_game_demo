using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayerAbstract : SaiMonoBehaviour
{
    [Header("RTS Player Abstract")]
    public RTSPlayerCtrl rtsPlayerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRTSPlayerCtrl();
    }

    protected virtual void LoadRTSPlayerCtrl()
    {
        if (this.rtsPlayerCtrl != null) return;
        this.rtsPlayerCtrl = transform.parent.GetComponent<RTSPlayerCtrl>();
        Debug.LogWarning(transform.name + ": LoadRTSPlayerCtrl", gameObject);
    }
}
