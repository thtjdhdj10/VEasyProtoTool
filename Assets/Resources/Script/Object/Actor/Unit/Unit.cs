using UnityEngine;
using System.Collections.Generic;

public class Unit : Actor
{
    public static List<Unit> unitList = new List<Unit>();

    [System.NonSerialized]
    public UnitStatus unitStatus;

    //

    protected override void Awake()
    {
        base.Awake();
        unitList.Add(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        unitList.Remove(this);
    }
}
