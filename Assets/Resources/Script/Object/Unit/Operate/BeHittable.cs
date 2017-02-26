using UnityEngine;
using System.Collections.Generic;

public class BeHittable : Operable
{
    public Unit owner;

    public static List<BeHittable> behittableList = new List<BeHittable>();

    protected virtual void Awake()
    {
        owner = GetComponent<Unit>();

        behittableList.Add(this);
    }

    protected virtual void OnDestroy()
    {
        behittableList.Remove(this);
    }

    public virtual void BeHit(Unit Hitter)
    {
        TriggerForCollisions.UnitEventReceive(Hitter, owner);
    }

}