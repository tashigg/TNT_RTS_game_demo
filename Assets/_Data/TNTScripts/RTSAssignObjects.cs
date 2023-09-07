using RTSEngine.Faction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSAssignObjects : TNTGameManagerAbstract
{
    [Header("RTS Assign Objects")]
    [SerializeField] protected string myName;
    [SerializeField] protected int myTeamIndex = -1;

    protected override void Awake()
    {
        base.Awake();
        this.LoadMyData();
        this.AssignMy2Capital();
    }

    protected virtual void LoadMyData()
    {
        this.myName = LobbyManager.Instance.profileName;
        TeamStruct teamStruct = LobbyManager.Instance.teamManager.GetTeam(this.myName);
        this.myTeamIndex = teamStruct.teamIndex;
    }

    protected virtual void AssignMy2Capital()
    {
        FactionSlot factionSlot = this.tntGameManager.gameManager.FactionSlots[this.myTeamIndex] as FactionSlot;
        factionSlot.enabled = true;
        factionSlot.data.isLocalPlayer = true;
    }
}
