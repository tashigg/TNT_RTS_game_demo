using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnLobbyStart : BaseButton
{

    protected override void Start()
    {
        base.Start();
        this.button.interactable = false;
    }

    private void FixedUpdate()
    {
        this.CheckIsLobbyCreated();
    }

    protected virtual void CheckIsLobbyCreated()
    {
        if (this.button.interactable == true) return;
        if (!LobbyManager.Instance.isLobbyHost) return;
        this.button.interactable = true;
    }

    protected override void OnClick()
    {
        Debug.Log("Start Lobby");
        LobbyManager.Instance.GameStart();
    }
}
