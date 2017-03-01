using UnityEngine;
using System.Collections.Generic;

public class Collisionable : Operable
{
    public Unit owner;

    public static List<Collisionable> collisionableList = new List<Collisionable>();

    protected virtual void Awake()
    {
        owner = GetComponent<Unit>();

        collisionableList.Add(this);
    }

    protected virtual void OnDestroy()
    {
        collisionableList.Remove(this);
    }

    public List<Unit.Relation> checkForce = new List<Unit.Relation>();

    protected virtual void Hit(Unit behitter)
    {
        TriggerForCollisions.UnitEventReceive(owner, behitter);
    }

    protected virtual void BeHit(Unit hitter)
    {
        TriggerForCollisions.UnitEventReceive(hitter, owner);
    }

    protected virtual void FixedUpdate()
    {
        CollisionCheckFrame();
    }

    protected virtual void CollisionCheckFrame()
    {
        if (owner.unitActive == false)
            return;

        List<Collisionable> colTargetList = CollisionCheck();
        if (colTargetList == null)
            return;

        for (int i = 0; i < colTargetList.Count; ++i)
        {
            Hit(colTargetList[i].owner);
            colTargetList[i].BeHit(owner);
        }
    }

    public virtual Collisionable FirstCollisionCheck(Unit.Relation targetRelation)
    {
        List<Collisionable> colTargetLit = CollisionCheck(targetRelation);

        if (colTargetLit == null)
            return null;

        return colTargetLit[0];
    }

    public virtual List<Collisionable> CollisionCheck()
    {
        return CollisionCheck(Unit.Relation.ENEMY);
    }

    public virtual List<Collisionable> CollisionCheck(Unit.Relation targetRelation)
    {
        List<Collisionable> ret = new List<Collisionable>();

        for (int i = 0; i < Collisionable.collisionableList.Count; ++i)
        {
            Collisionable target = Collisionable.collisionableList[i];

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

    public virtual bool CollisionCheck(Collisionable target)
    {
        return VEasyCalculator.IntersectCheck(owner, target.owner);
    }
}
