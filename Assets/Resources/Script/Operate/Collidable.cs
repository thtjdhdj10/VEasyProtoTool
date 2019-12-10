using UnityEngine;
using System.Collections.Generic;


public class Collidable : Operable
{
    public new Collider2D collider;

    protected override void Awake()
    {
        base.Awake();

        // TODO 에디터에서 getcomponent해서 연결되게 수정
        if (collider == null) collider = GetComponent<Collider2D>();
        if (collider == null) Debug.LogWarning(this.name + " has not collider.");
    }

    protected virtual void Hit(Unit target)
    {
        TriggerCollision.UnitEventReceive(owner, target);
        TriggerCollision.UnitEventReceive(target, owner);
    }

    protected virtual void FixedUpdate()
    {
        if(state.State) CollisionCheckFrame();
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

        if (colTargetList == null) return null;

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

        return ret;
    }

    public virtual bool CollisionCheck(Collidable target)
    {
        return collider.IsTouching(target.collider);
    }
}
