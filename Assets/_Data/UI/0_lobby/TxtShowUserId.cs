using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TxtShowUserId : BaseText
{
    private void FixedUpdate()
    {
        this.Showing();
    }

    protected virtual void Showing()
    {
        this.textMeshPro.text = LobbyManager.Instance.playerServiceId??"no-id";
    }
}
