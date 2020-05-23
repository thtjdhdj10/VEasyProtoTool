using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Collidable : Operable
{
    public new Collider2D collider;

    public List<Actor.ERelation> colRelationList = new List<Actor.ERelation>();

    // canCollideSeveral: true면 한 프레임에 1번만 충돌 가능
    // isCollisionInFrame: 각 Collidable의 충돌처리 여부 상태를 매 프레임 시작 전에 초기화
    public bool canCollisionSeveralInFrame = false;
    [System.NonSerialized]
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

        // TODO: can collision several in frame: true 면 first 조건 없이 충돌 확인
        Collidable col = GetFirstCollision();
        if (col != null) Hit(col);
    }

    // TODO: collidable을 bullet, unit 등 카테고리를 나눠서 충돌 처리 최적화
    // relation 확인 코드 수정 필요
    // foreach 대신 for 사용
    public virtual Collidable GetFirstCollision()
    {
        foreach (var operable in _allOperableListDic[typeof(Collidable)])
        {
            Collidable col = operable as Collidable;

            if (col != null &&
                col.gameObject.activeInHierarchy &&
                col.state == true &&
                (col.canCollisionSeveralInFrame || !col.isCollisionInFrame) &&
                colRelationList.Contains(Actor.GetRelation(owner.force, col.owner.force)) &&
                IsCollision(col))
            {
                return col;
            }
        }

        return null;
    }

    public virtual bool IsCollision(Collidable target)
    {
        return collider.IsTouching(target.collider);
    }
}
