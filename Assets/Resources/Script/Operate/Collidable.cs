using UnityEngine;
using System.Collections.Generic;

public class Collidable : Operable
{
    public Unit owner;

    public static List<Collidable> collisionableList = new List<Collidable>();

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

    protected virtual void Hit(Unit target)
    {
        TriggerCollision.UnitEventReceive(owner, target);
    }

    protected virtual void FixedUpdate()
    {
        CollisionCheckFrame();
    }

    protected virtual void CollisionCheckFrame()
    {
        if (owner.unitActive == false)
            return;

        List<Collidable> colTargetList = CollisionCheck();
        if (colTargetList == null)
            return;

        for (int i = 0; i < colTargetList.Count; ++i)
        {
            Hit(colTargetList[i].owner);
        }
    }

    public virtual Collidable FirstCollisionCheck(Unit.Relation targetRelation)
    {
        List<Collidable> colTargetList = CollisionCheck(targetRelation);

        if (colTargetList == null)
            return null;

        return colTargetList[0];
    }

    public virtual List<Collidable> CollisionCheck()
    {
        return CollisionCheck(Unit.Relation.ENEMY);
    }

    public virtual List<Collidable> CollisionCheck(Unit.Relation targetRelation)
    {
        List<Collidable> ret = new List<Collidable>();

        for (int i = 0; i < Collidable.collisionableList.Count; ++i)
        {
            Collidable target = Collidable.collisionableList[i];

            if (target == null)
                continue;

            if (target.gameObject.activeInHierarchy == false)
                continue;

            if (target.owner.unitActive == false)
                continue;

            Unit.Relation relation = Unit.GetRelation(owner.force, target.owner.force);
            if (targetRelation != relation)
                continue;

            if (CollisionCheck(target) == true)
                ret.Add(target);
        }

        return null;
    }

    public virtual bool CollisionCheck(Collidable target)
    {
        return VEasyCalculator.IntersectCheck(owner, target.owner);
    }
}
