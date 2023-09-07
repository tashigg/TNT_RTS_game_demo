using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TNTGameManagerAbstract : SaiMonoBehaviour
{
    [Header("TNT Game Manager Abstract")]
    public TNTGameManager tntGameManager;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTNTGameManager();
    }

    protected virtual void LoadTNTGameManager()
    {
        if (this.tntGameManager != null) return;
        this.tntGameManager = transform.parent.GetComponent<TNTGameManager>();
        Debug.LogWarning(transform.name + ": LoadTNTGameManager", gameObject);
    }
}
