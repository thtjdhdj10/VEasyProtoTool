using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Collidable : Operable
{
    public new Collider2D collider;

    // canCollideSeveral: true면 한 프레임에 1번만 충돌 가능
    // collideInFrame: 각 Collidable의 충돌처리 여부 상태를 매 프레임 시작 전에 초기화
    public bool canCollisionSeveralInFrame = false;
    public bool isCollisionInFrame = false;

    public delegate void OnHitDelegate(Actor from, Actor to);
    public OnHitDelegate onHitDelegate = new OnHitDelegate(OnHitMethod);
    public static void OnHitMethod(Actor from, Actor to) { }

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

        isCollisionInFrame = true;
        target.isCollisionInFrame = true;
    }

    protected virtual void FixedUpdate()
    {
        if (state == false) return;
        CollisionCheckFrame();
    }

    protected virtual void CollisionCheckFrame()
    {
        if (canCollisionSeveralInFrame == false &&
            isCollisionInFrame == true)
            return;

        GetCollisionTarget(Unit.Relation.ENEMY)?.ForEach(t => Hit(t));
    }

    public virtual List<Collidable> GetCollisionTarget(Unit.Relation targetRelation)
    {
        List<Collidable> ret = _allOperableListDic[typeof(Collidable)].
            ConvertAll(t => t as Collidable);

        return (from target in ret
               where target != null
               where target.gameObject.activeInHierarchy == true 
               where target.state == true
               where target.canCollisionSeveralInFrame == true ||
                    target.isCollisionInFrame == false
               where targetRelation == Unit.GetRelation(owner.force, target.owner.force)
               where IsCollision(target)
               select target).ToList();
    }

    public virtual bool IsCollision(Collidable target)
    {
        return collider.IsTouching(target.collider);
    }
}
