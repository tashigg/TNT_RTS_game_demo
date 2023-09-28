using System.Collections.Generic;
using UnityEngine;

public class BasePoints : SaiMonoBehaviour
{
    public static BasePoints instance;
    [Header("Base Points")]
    public List<Transform> points;

    protected override void Awake()
    {
        base.Awake();
        if (BasePoints.instance != null) Debug.LogError("Only 1 BasePoints is allow");
        else BasePoints.instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPoints();
    }

    protected virtual void LoadPoints()
    {
        if (this.points.Count > 0) return;
        foreach(Transform point in transform)
        {
            this.points.Add(point);
        }
        Debug.Log(transform.name + ": LoadPoints", gameObject);
    }
}
