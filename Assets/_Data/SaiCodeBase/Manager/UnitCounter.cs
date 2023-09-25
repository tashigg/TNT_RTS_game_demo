using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCounter : SaiSingleton<UnitCounter>
{
    [SerializeField] protected int unitCount = 0;
    public int UnitCount => unitCount;

    public virtual void Add()
    {
        this.unitCount++;
    }

    public virtual void Remove()
    {
        this.unitCount--;
    }
}
