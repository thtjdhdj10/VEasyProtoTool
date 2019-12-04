using UnityEngine;
using System.Collections.Generic;

public class Collidable : Operable
{
    // colType에 따라 rec 나 circle 하나만 입력하게 수정
    public enum ColliderType // Collidable 의 속성으로 이동
    {
        CIRCLE,
        RECT,
    }

    public ColliderType colType;

    public float radius;

    public Vector2 rect;

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

        List<Operable> collidableList = allOperableListDic[typeof(Collidable)];

        for (int i = 0; i < collidableList.Count; ++i)
        {
            Collidable target = collidableList[i] as Collidable;

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
        return VEasyCalculator.IntersectCheck(this, target);
    }
}
