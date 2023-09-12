using RTSEngine.Faction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSAssignObjects : TNTGameManagerAbstract
{
    [Header("RTS Assign Objects")]
    [SerializeField] protected string myName;
    [SerializeField] protected int myTeamIndex = -1;
    [SerializeField] protected bool assigned = false;

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
        //if (this.assigned) return;
        //if (this.tntGameManager == null) return;
        //if (this.tntGameManager.gameManager == null) return;
        //if (this.tntGameManager.gameManager.FactionSlots == null) return;

        FactionSlot factionSlot = this.tntGameManager.gameManager.FactionSlots[this.myTeamIndex] as FactionSlot;
        factionSlot.enabled = true;
        factionSlot.data.isLocalPlayer = true;
        this.assigned = true;
    }
}
