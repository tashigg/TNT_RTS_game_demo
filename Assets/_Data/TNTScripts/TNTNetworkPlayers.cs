using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TNTNetworkPlayers : SaiSingleton<TNTNetworkPlayers>
{
    [Header("TNT Network Players")]
    public List<RTSPlayerCtrl> playerCtrls;
    public RTSPlayerCtrl me;

    public virtual RTSPlayerCtrl FindByFactionId(int factionid)
    {
        foreach(RTSPlayerCtrl playerCtrl in this.playerCtrls)
        {
            int clientId = (int) playerCtrl.networkObject.OwnerClientId;
            if (clientId == factionid) return playerCtrl;
        }

        return null;
    }
}
