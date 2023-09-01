using UnityEngine;
using System;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;

[Serializable]
public class LobbyPlayer 
{
    public string name;
    public string id;
    public LobbyPositions position;
    public Dictionary<string, PlayerDataObject> playerOptions;
}
