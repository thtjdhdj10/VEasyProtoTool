using UnityEngine;
using System.Collections.Generic;

public class Targetable : Operable
{
    public Unit owner;

    public float importance;

    public static List<Targetable> targetableList = new List<Targetable>();

    protected virtual void Awake()
    {
        owner = GetComponent<Unit>();

        targetableList.Add(this);
    }

    protected virtual void OnDestroy()
    {
        targetableList.Remove(this);
    }


}
