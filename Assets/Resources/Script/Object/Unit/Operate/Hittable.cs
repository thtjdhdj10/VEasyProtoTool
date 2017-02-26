using UnityEngine;
using System.Collections.Generic;

public class Hittable : Operable
{
    public Unit owner;

    public static List<Hittable> hittableList = new List<Hittable>();

    protected virtual void Awake()
    {
        owner = GetComponent<Unit>();

        hittableList.Add(this);
    }

    protected virtual void OnDestroy()
    {
        hittableList.Remove(this);
    }

    public List<Unit.Relation> checkForce = new List<Unit.Relation>();

    protected virtual void Hit(Unit behitter)
    {
        TriggerForCollisions.UnitEventReceive(owner, behitter);
    }

    protected virtual void FixedUpdate()
    {
        CollisionCheckFrame();
    }

    protected virtual void CollisionCheckFrame()
    {
        if (owner.unitActive == false)
            return;

        List<BeHittable> colTargetList = CollisionCheck();
        if (colTargetList == null)
            return;

        for (int i = 0; i < colTargetList.Count; ++i)
        {
            Hit(colTargetList[i].owner);
            colTargetList[i].BeHit(owner);
        }
    }

    public virtual BeHittable FirstCollisionCheck(Unit.Relation targetRelation)
    {
        List<BeHittable> colTargetLit = CollisionCheck(targetRelation);

        if (colTargetLit == null)
            return null;

        return colTargetLit[0];
    }

    public virtual List<BeHittable> CollisionCheck()
    {
        return CollisionCheck(Unit.Relation.ENEMY);
    }

    public virtual List<BeHittable> CollisionCheck(Unit.Relation targetRelation)
    {
        List<BeHittable> ret = new List<BeHittable>();

        for (int i = 0; i < BeHittable.behittableList.Count; ++i)
        {
            BeHittable target = BeHittable.behittableList[i];

            if (target == null)
                continue;

            if (target.gameObject.activeInHierarchy == false)
                continue;

            if (target.owner.unitActive == false)
                continue;

            Unit.Relation relation = Unit.GetRelation(owner.force, target.owner.force);
            if (targetRelation != relation)
            {
                continue;
            }

            if (CollisionCheck(target) == true)
            {
                ret.Add(target);
            }
        }

        return null;
    }

    public virtual bool CollisionCheck(BeHittable target)
    {
        return VEasyCalculator.IntersectCheck(owner, target.owner);
    }
}
