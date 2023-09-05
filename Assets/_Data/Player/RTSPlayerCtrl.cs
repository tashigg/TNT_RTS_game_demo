using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayerCtrl : SaiMonoBehaviour
{
    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
