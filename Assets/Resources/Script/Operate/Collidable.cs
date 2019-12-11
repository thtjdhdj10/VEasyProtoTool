﻿using UnityEngine;
using System.Collections.Generic;


public class Collidable : Operable
{
    public new Collider2D collider;

    public delegate void OnHitDelegate(Unit from, Unit to);
    public OnHitDelegate onHitDelegate = new OnHitDelegate(OnHitCallback);
    public static void OnHitCallback(Unit from, Unit to) { }

    public delegate void OnCollidableAddedDelegate(Collidable col);
    public static OnCollidableAddedDelegate onCollidableAddedDelegate
        = new OnCollidableAddedDelegate(OnCollidableAdded);
    public static void OnCollidableAdded(Collidable col) { }

    protected override void Awake()
    {
        base.Awake();
        onCollidableAddedDelegate(this);

        // TODO 에디터에서 getcomponent해서 연결되게 수정
        if (collider == null) collider = GetComponent<Collider2D>();
        if (collider == null) Debug.LogWarning(this.name + " has not collider.");
    }

    protected virtual void Hit(Unit target)
    {
        onHitDelegate(owner, target);
    }

    protected virtual void FixedUpdate()
    {
        if (state == false) return;
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

            if (target.state == false)
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
