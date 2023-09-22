using UnityEngine;

public abstract class BuildingAbstract : SaiMonoBehaviour
{
    [Header("Building Abstract")]
    public BuildingCtrl buildingCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBuildingCtrl();
    }

    protected virtual void LoadBuildingCtrl()
    {
        if (this.buildingCtrl != null) return;
        this.buildingCtrl = GetComponentInParent<BuildingCtrl>();
        Debug.LogWarning(transform.name + ": LoadBuildingCtrl", gameObject);
    }
}
