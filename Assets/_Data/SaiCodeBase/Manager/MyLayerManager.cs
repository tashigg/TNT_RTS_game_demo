using UnityEngine;

public class MyLayerManager : SaiMonoBehaviour
{
    public static MyLayerManager instance;

    [Header("Layers")]
    public int layerWorker;
    public int layerGround;
    public int layerGroundTerrain;
    public int layerBuilding;
    public int layerTree;
    public int layerUnitSelectable;

    protected override void Awake()
    {
        if (MyLayerManager.instance != null) Debug.LogError("Only 1 MyLayerManager allow");
        MyLayerManager.instance = this;

        this.LoadComponents();
        //Physics.IgnoreLayerCollision(this.layerBullet, this.layerGround, true);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.GetPlayers();
    }

    protected virtual void GetPlayers()
    {
        this.layerWorker = LayerMask.NameToLayer("Worker");
        this.layerGround = LayerMask.NameToLayer("Ground");
        this.layerGroundTerrain = LayerMask.NameToLayer("GroundTerrain");
        this.layerBuilding = LayerMask.NameToLayer("Building");
        this.layerTree = LayerMask.NameToLayer("Tree");
        this.layerUnitSelectable = LayerMask.NameToLayer("UnitSelectable");

        if (this.layerWorker < 0) Debug.LogError("Layer Worker is mising");
        if (this.layerGround < 0) Debug.LogError("Layer Ground is mising");
        if (this.layerGroundTerrain < 0) Debug.LogError("Layer GroundTerrain is mising");
        if (this.layerBuilding < 0) Debug.LogError("Layer Building is mising");
        if (this.layerTree < 0) Debug.LogError("Layer Tree is mising");
        if (this.layerUnitSelectable < 0) Debug.LogError("Layer UnitSelectable is mising");

        //Debug.Log(transform.name + ": GetPlayers", gameObject);
    }
}
