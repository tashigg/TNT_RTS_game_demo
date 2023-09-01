using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[Serializable]
public class LobbyPlayer 
{
    public string name;
    public string id;
    public LobbyPositions position;
}
