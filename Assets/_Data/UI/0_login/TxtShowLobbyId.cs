using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TxtShowLobbyId : BaseText
{

    private void FixedUpdate()
    {
        this.ShowingLobbyId();
    }

    protected virtual void ShowingLobbyId()
    {
        this.textMeshPro.text = LobbyManager.Instance.lobbyId??"no-id";
    }
}
