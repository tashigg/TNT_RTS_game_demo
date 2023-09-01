using System;
using UnityEngine;

public enum LobbyPlayerData
{
    unknow = 0,
    name = 1,
    position = 2,
    team = 3,
}

public class LobbyPlayerDataParser
{
    public static LobbyPlayerData FromString(string name)
    {
        try
        {
            return (LobbyPlayerData)System.Enum.Parse(typeof(LobbyPlayerData), name);
        }
        catch (ArgumentException e)
        {
            Debug.LogError(e.ToString());
            return LobbyPlayerData.unknow;
        }
    }
}