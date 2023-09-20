using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayers : SaiSingleton<NetworkPlayers>
{
    [Header("TNT Network Players")]
    public List<MainBaseCtrl> playerCtrls;
    public MainBaseCtrl me;

    public virtual void Add(MainBaseCtrl mainBaseCtrl)
    {
        this.playerCtrls.Add(mainBaseCtrl);
    }

    public virtual void SetMe(MainBaseCtrl mainBaseCtrl)
    {
        this.me = mainBaseCtrl;
    }

    public virtual MainBaseCtrl FindByClientId(int clientId)
    {
        foreach(MainBaseCtrl playerCtrl in this.playerCtrls)
        {
            int playerClientId = (int) playerCtrl.networkObject.OwnerClientId;
            if (clientId == playerClientId) return playerCtrl;
        }

        return null;
    }
}
