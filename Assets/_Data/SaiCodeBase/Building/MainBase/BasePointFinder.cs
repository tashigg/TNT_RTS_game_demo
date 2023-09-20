using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePointFinder : MainBaseAbstract
{
    [SerializeField] protected Transform basePoint;

    protected override void OnEnable()
    {
        this.FindAndMoveToBasePoint();
    }

    protected virtual void FindAndMoveToBasePoint()
    {
        if (BasePoints.instance == null || this.mainBaseCtrl.ClientID == -1)
        {
            Invoke(nameof(this.FindAndMoveToBasePoint), 0.5f);
            return;
        }

        this.basePoint = BasePoints.instance.points[this.mainBaseCtrl.ClientID];
        transform.parent.position = this.basePoint.position;
        this.MoveGodModeCamera();
    }

    protected virtual void MoveGodModeCamera()
    {
        Vector3 pos = this.basePoint.transform.position;
        pos.z -= 7f;
        GodModeCtrl.instance.godMovement.MoveTo(pos.x, pos.z);
    }
}
