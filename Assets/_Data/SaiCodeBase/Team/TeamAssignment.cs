using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamAssignment : SaiMonoBehaviour
{
    [Header("Color Render")]
    [SerializeField] protected int teamId = 0;
    [SerializeField] protected GameInfoSO gameInfo;
    [SerializeField] protected List<MeshRenderer> colorRenderers;

    protected override void Start()
    {
        base.Start();
        //this.ColorApply(this.teamId);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameData();
    }

    protected override void Reset()
    {
        base.Reset();
        this.LoadMeshRenderer();
    }

    protected virtual void LoadGameData()
    {
        if (this.gameInfo != null) return;
        this.gameInfo = Resources.Load("GameInfo") as GameInfoSO;
        Debug.LogWarning(transform.name + ": LoadGameData", gameObject);
    }

    protected virtual void LoadMeshRenderer()
    {
        if (this.colorRenderers.Count > 0) return;
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.name.Contains("roof "))
                {
                    this.colorRenderers.Add(renderer);
                    continue;
                }
            }
        }
        Debug.LogWarning(transform.name + ": LoadMeshRenderer", gameObject);
    }

    public virtual void ColorApply(int teamId)
    {
        Debug.Log($"ColorApply {teamId}");
        this.teamId = teamId;
        string materialName;
        int materialCount;
        Material[] newMaterials;
        Material material;
        TeamData team = this.gameInfo.teams[teamId];
        foreach (MeshRenderer renderer in this.colorRenderers)
        {
            newMaterials = renderer.materials;
            materialCount = newMaterials.Length;
            for (int i = 0; i < materialCount; i++)
            {
                material = newMaterials[i];
                materialName = material.name.Replace(" (Instance)", "");
                if (materialName == "roof")
                {
                    newMaterials[i].color = team.color;
                }
            }
            renderer.materials = newMaterials;
        }
    }
}
