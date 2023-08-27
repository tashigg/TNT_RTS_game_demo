using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InpPlayerCount : BaseInputField
{

    protected override void Start()
    {
        base.Start();
        this.inputField.interactable = false;
    }

    private void FixedUpdate()
    {
        this.CheckLobbyCreated();
    }

    protected virtual void CheckLobbyCreated()
    {
        this.inputField.text = LobbyManager.Instance.playerCount.ToString();
    }


    protected override void onChanged(string value)
    {
        //Do nothing
    }
}
