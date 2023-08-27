using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InpLobbyCode : BaseInputField
{
    private void FixedUpdate()
    {
        this.CheckLobbyCreated();
    }

    protected virtual void CheckLobbyCreated()
    {
        if (this.inputField.readOnly) return;
        if (!LobbyManager.Instance.isLobbyHost) return;
        this.inputField.text = LobbyManager.Instance.lobbyCode;
        this.inputField.readOnly = true;
    }



    protected override void onChanged(string value)
    {
        LobbyManager.Instance.lobbyCodeToJoin = value;
    }
}
