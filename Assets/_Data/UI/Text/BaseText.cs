using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseText : SaiMonoBehaviour
{
    [Header("Base Text")]
    public TextMeshProUGUI textMeshPro;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTextLobbyCode();
    }

    protected virtual void LoadTextLobbyCode()
    {
        if (this.textMeshPro != null) return;
        this.textMeshPro = GetComponent<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + ": LoadTextLobbyCode", gameObject);
    }
}
