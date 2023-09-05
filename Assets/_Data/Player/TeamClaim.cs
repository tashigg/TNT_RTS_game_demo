using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamClaim : RTSPlayerAbstract
{
    [Header("Team Claim")]
    public string myName;
    public string myTeam;

    protected override void Start()
    {
        base.Start();
        this.TeamClaiming();
        this.CapitalClaiming();
    }

    protected virtual void TeamClaiming()
    {
        string teamString = LobbyManager.Instance.teamString;
        if(teamString == "")
        {
            Invoke(nameof(this.TeamClaiming), 1);
            return;
        }

        this.myName = LobbyManager.Instance.profileName;

        string[] arrayPlayerData;
        string playerName, playerTeam;
        string[] arrayPlayersData = teamString.Split(";");

        foreach (string playerString in arrayPlayersData)
        {
            if (playerString == "") continue;
            arrayPlayerData = playerString.Split(",");
            playerName = arrayPlayerData[0];
            playerTeam = arrayPlayerData[1];

            if (playerName == this.myName)
            {
                this.myTeam = playerTeam;
                return;
            }
        }
    }

    protected virtual void CapitalClaiming()
    {
        if(this.myTeam == "")
        {
            Invoke(nameof(this.CapitalClaiming), 1);
            return;
        }
    }
}
