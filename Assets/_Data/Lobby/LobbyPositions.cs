
using System;
using UnityEngine;

public enum LobbyPositions
{
    noPosition = 0,
    host = 1,
    member = 2,
}

public class LobbyPositionsParser
{
    public static LobbyPositions FromString(string name)
    {
        try
        {
            return (LobbyPositions)System.Enum.Parse(typeof(LobbyPositions), name);
        }
        catch (ArgumentException e)
        {
            Debug.LogError(e.ToString());
            return LobbyPositions.noPosition;
        }
    }
}