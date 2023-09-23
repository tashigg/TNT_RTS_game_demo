using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayers : SaiSingleton<NetworkPlayers>
{
    [Header("TNT Network Players")]
    public List<DummyPlayerCtrl> playerCtrls;
    public DummyPlayerCtrl me;

    public virtual void Add(DummyPlayerCtrl playerCtrl)
    {
        this.playerCtrls.Add(playerCtrl);
    }

    public virtual void SetMe(DummyPlayerCtrl playerCtrl)
    {
        this.me = playerCtrl;
    }

    public virtual DummyPlayerCtrl FindByClientId(int clientId)
    {
        foreach(DummyPlayerCtrl playerCtrl in this.playerCtrls)
        {
            int playerClientId = (int) playerCtrl.networkObject.OwnerClientId;
            if (clientId == playerClientId) return playerCtrl;
        }

        return null;
    }
}
