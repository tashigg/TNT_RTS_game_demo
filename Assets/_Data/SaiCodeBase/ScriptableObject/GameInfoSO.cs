using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "SO/GameInfo", order = 1)]
public class GameInfoSO : ScriptableObject
{
    public List<TeamData> teams;
}