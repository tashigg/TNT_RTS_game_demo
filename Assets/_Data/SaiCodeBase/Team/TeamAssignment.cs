using System.Collections.Generic;
using UnityEngine;

public class TeamAssignment : SaiMonoBehaviour
{
    [Header("Color Render")]
    [SerializeField] protected int teamId = 0;
    [SerializeField] protected GameInfoSO gameData;
    [SerializeField] protected List<MeshRenderer> colorRenderers;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameData();
    }

    protected virtual void LoadGameData()
    {
        if (this.gameData != null) return;
        this.gameData = Resources.Load("GameInfo") as GameInfoSO;
        Debug.LogWarning(transform.name + ": LoadGameData", gameObject);
    }

    public virtual void ColorApply(int teamId)
    {
        this.teamId = teamId;
        string materialName;
        int materialCount;
        Material[] newMaterials;
        Material material;
        TeamData team = this.gameData.teams[teamId];
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
