using UnityEngine;
using System.Collections.Generic;


public class Collidable : Operable
{
    public new Collider2D collider;

    // canCollideSeveral: true면 한 프레임에 1번만 충돌 가능
    // collideInFrame: 각 Collidable의 충돌처리 여부 상태를 매 프레임 시작 전에 초기화
    public bool canCollideSeveral = false;
    public bool collideInFrame = false;

    public delegate void OnHitDelegate(Actor from, Actor to);
    public OnHitDelegate onHitDelegate = new OnHitDelegate(OnHitCallback);
    public static void OnHitCallback(Actor from, Actor to) { }

    protected override void Awake()
    {
        base.Awake();

        // TODO 에디터에서 getcomponent해서 연결되게 수정
        if (collider == null) collider = GetComponent<Collider2D>();
        if (collider == null) Debug.LogWarning(this.name + " has not collider.");
    }

    protected virtual void Hit(Collidable target)
    {
        onHitDelegate(owner, target.owner);
        target.onHitDelegate(target.owner, owner);

        collideInFrame = true;
        target.collideInFrame = true;
    }

    protected virtual void FixedUpdate()
    {
        if (state == false) return;
        CollisionCheckFrame();
    }

    protected virtual void CollisionCheckFrame()
    {
        if (canCollideSeveral == false &&
            collideInFrame == true)
            return;

        List<Collidable> colTargetList = CollisionCheck();
        if (colTargetList == null)
            return;

        for (int i = 0; i < colTargetList.Count; ++i)
        {
            Hit(colTargetList[i]);
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

            if (target.state == false)
                continue;

            if (target.canCollideSeveral == false &&
                target.collideInFrame == true)
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
