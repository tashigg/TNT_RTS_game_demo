using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBaseAbstract : SaiMonoBehaviour
{
    [Header("Main Base")]
    public MainBaseCtrl mainBaseCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMainBaseCtrl();
    }

    protected virtual void LoadMainBaseCtrl()
    {
        if (this.mainBaseCtrl != null) return;
        this.mainBaseCtrl = transform.parent.GetComponent<MainBaseCtrl>();
        Debug.LogWarning(transform.name + ": LoadMainBaseCtrl", gameObject);
    }
}
