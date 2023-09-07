using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : LobbyManagerAbstract
{
    [Header("Team Manager")]
    public List<TeamStruct> teams;

    private void FixedUpdate()
    {
        this.UpdateTeamStruct();
    }

    protected virtual void UpdateTeamStruct()
    {
        string teamString = this.lobbyManager.teamString;
        if (teamString == "") return;

        string teamId, teamName;
        TeamStruct teamStruct, teamExist;
        
        string[] arrayPlayerData;
        string[] arrayPlayersData = teamString.Split(";");
        foreach (string playerString in arrayPlayersData)
        {
            if (playerString == "") continue;
            arrayPlayerData = playerString.Split(",");

            teamName = arrayPlayerData[0];
            teamId = arrayPlayerData[1];

            teamExist = this.teams.Find((team) => team.name == teamName);
            if (teamExist.teamId != null) continue;

            teamStruct = new TeamStruct
            {
                name = arrayPlayerData[0],
                teamId = teamId,
                teamIndex = int.Parse(Regex.Replace(teamId, @"[^\d]", "")),
            };

            this.teams.Add(teamStruct);
        }
    }

    public virtual TeamStruct GetTeam(string name)
    {
        return this.teams.Find((team) => team.name == name);
    }
}
