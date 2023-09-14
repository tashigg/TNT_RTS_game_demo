using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TNTNetworkPlayers : SaiSingleton<TNTNetworkPlayers>
{
    [Header("TNT Network Players")]
    public List<RTSPlayerCtrl> playerCtrls;
    public RTSPlayerCtrl me;
}
