using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionRenderer : BuildingAbstract
{
    [Header("Selection Renderer")]
    public MeshRenderer meshRenderer;

    private void FixedUpdate()
    {
        this.ShowingSelection();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMeshRenderer();
    }

    protected virtual void LoadMeshRenderer()
    {
        if (this.meshRenderer != null) return;
        this.meshRenderer = GetComponent<MeshRenderer>();
        Debug.LogWarning(transform.name + ": LoadMeshRenderer", gameObject);
    }

    protected virtual void ShowingSelection()
    {
        this.meshRenderer.enabled = this.buildingCtrl.unitSelectable.IsSelected();
    }
}
