using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Movable : Operable
{
    public float speed = 1f;

    protected Vector2 _targetPos;

    protected virtual void FixedUpdate()
    {
        if (state == false) return;

        if(owner.TryGetOperable(out Targetable ownerTarget))
        {
            if (ownerTarget.target != null)
                _targetPos = ownerTarget.target.transform.position;
        }

        // TODO bounce랑 충돌 동시에 되면 동시에 수행이 안되고 하나가 먼저됨
        // 그거땜에 다른 하나가 동작안할때가있음
        // hit 충돌처리할때 다른 하나가 이미 destroy 됐을수도 있음

        MoveFrame();

        BounceProcessing();
    }

    protected abstract void MoveFrame();

    public bool _enableBounce;
    public float _bounceCooldown;
    private float _remainBounceCooldown;
    public EBounceBy _bounceBy;
    public EBounceTo _bounceTo;

    public enum EBounceBy
    {
        NONE = 0,
        ALL,
        BOUNDARY_TOUCH,
        BOUNDARY_OUT,
        UNIT,
        ENEMY,
        ALLY,
    }
    
    public enum EBounceTo
    {
        NONE = 0,
        TARGET,
        REVERSE, // dir +180
        REFLECT, // 거울반사
        BLOCK, // 길막
        DESTROY,
    }

    protected virtual void BounceProcessing()
    {
        if (_enableBounce == false) return;

        if (_remainBounceCooldown > 0f)
        {
            _remainBounceCooldown -= Time.fixedDeltaTime;
            return;
        }
        else _remainBounceCooldown = _bounceCooldown;

        bool isBounced = false;
        float targetDir = 0f;

        if (_bounceBy == EBounceBy.ALL ||
            _bounceBy == EBounceBy.BOUNDARY_TOUCH)
        {
            Collidable col = owner.GetOperable<Collidable>();
            Const.EDirection bounceByDir = VEasyCalculator.CheckTerritory2D(col.collider);

            isBounced = true;
            switch (bounceByDir)
            {
                case Const.EDirection.NONE:
                    isBounced = false;
                    break;
                case Const.EDirection.UP:
                    targetDir = 90f;
                    break;
                case Const.EDirection.DOWN:
                    targetDir = 270f;
                    break;
                case Const.EDirection.LEFT:
                    targetDir = 180f;
                    break;
                case Const.EDirection.RIGHT:
                    targetDir = 0f;
                    break;
            }
        }
        else if (_bounceBy == EBounceBy.ALL ||
            _bounceBy == EBounceBy.BOUNDARY_OUT)
        {
            Collidable col = owner.GetOperable<Collidable>();
            Const.EDirection bounceByDir = VEasyCalculator.CheckOutside2D(col.collider);

            isBounced = true;
            switch (bounceByDir)
            {
                case Const.EDirection.NONE:
                    isBounced = false;
                    break;
                case Const.EDirection.UP:
                    targetDir = 90f;
                    break;
                case Const.EDirection.DOWN:
                    targetDir = 270f;
                    break;
                case Const.EDirection.LEFT:
                    targetDir = 180f;
                    break;
                case Const.EDirection.RIGHT:
                    targetDir = 0f;
                    break;
            }
        }
        else if (_bounceBy == EBounceBy.ALL ||
            _bounceBy == EBounceBy.UNIT ||
            _bounceBy == EBounceBy.ALLY ||
            _bounceBy == EBounceBy.ENEMY)
        {
            if(owner.TryGetOperable(out Collidable col))
            {
                Unit.ERelation targetRelation = Unit.ERelation.NONE;
                if (_bounceBy == EBounceBy.UNIT)
                    targetRelation = Unit.ERelation.NEUTRAL;
                else if (_bounceBy == EBounceBy.ALLY)
                    targetRelation = Unit.ERelation.ALLY;
                else if (_bounceBy == EBounceBy.ENEMY)
                    targetRelation = Unit.ERelation.ENEMY;

                Collidable colTarget = col.GetCollisionTarget(targetRelation)?.First();

                if (colTarget != null)
                {
                    isBounced = true;
                    targetDir = VEasyCalculator.GetDirection(owner.transform.position, _targetPos);
                }
            }
        }

        if(isBounced)
        {
            switch (_bounceTo)
            {
                case EBounceTo.TARGET:
                    owner.moveDirection = targetDir;
                    break;
                case EBounceTo.REVERSE:
                    owner.moveDirection += 180f;
                    break;
                case EBounceTo.REFLECT:
                    owner.moveDirection = VEasyCalculator.GetReflectedDirection(owner.moveDirection, targetDir);
                    break;
                case EBounceTo.BLOCK:
                    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(
                        owner.moveDirection, 1f);
                    Vector2 targetVector = VEasyCalculator.GetRotatedPosition(
                        targetDir, 1f);

                    float inner = VEasyCalculator.Inner(moveVector, targetVector);

                    Vector2 escapeVector = VEasyCalculator.GetRotatedPosition(
                        targetDir + 180f, inner * speed * Time.fixedDeltaTime);

                    owner.transform.position = (Vector2)owner.transform.position + escapeVector;
                    // TODO 최적화 및 벽에서 달달달 안하게
                    break;
                case EBounceTo.DESTROY:
                    Destroy(owner.gameObject);
                    break;
            }
        }
    }
}


//

//public void InitDodge(float spd, int _dodgeFrame, int dodgeDir)
//{
//    moveType = MoveType.DODGE;

//    speed = spd;
//    dodgeFrame = _dodgeFrame;
//    dodgeDirection = dodgeDir;
//}

//

// 엔터더건전 쥐갈공명 참고
// TODO: 미구현
//protected virtual void DodgeMove()
//{
//    List<Unit> bulletList = new List<Unit>();

//    // PLAYER 의 모든 총알을 대상으로
//    for (int i = 0; i < Unit.unitList.Count; ++i)
//    {
//        Unit unit = Unit.unitList[i];

//        if (unit.force == Unit.Force.PLAYER &&
//            unit.Hittable != null &&
//            unit.isActiveAndEnabled == true)
//        {
//            bulletList.Add(unit);
//        }
//    }

//    //        Vector2 originalPosition = owner.transform.position;

//    // dodgeDirection 개의 방향으로 dodgeFrame * speed 만큼 이동했을 때
//    List<Vector2> movablePos = new List<Vector2>();
//    for (int i = 0; i < dodgeDirection + 1; ++i)
//    {
//        Vector2 checkPosition = new Vector2();

//        bool collision = false;

//        float checkDistance = 0f;

//        for (int j = 0; j < bulletList.Count; ++j)
//        {
//            if (i < dodgeDirection)
//            {
//                direction = 360f / (float)dodgeDirection * (float)i;
//                checkDistance = speed * Time.fixedDeltaTime * (float)dodgeFrame;

//                collision = VEasyCalculator.FutureIntersectCheck(owner, bulletList[j], dodgeFrame);
//            }
//            else
//            {
//                direction = 0f;
//                checkDistance = 0f;

//                collision = VEasyCalculator.IntersectCheck(owner, bulletList[j]);
//            }
//        }

//        if (collision == false)
//        {
//            checkPosition = owner.transform.position;
//            checkPosition += VEasyCalculator.GetRotatedPosition(direction, checkDistance);

//            movablePos.Add(checkPosition);
//        }
//    }

//    Debug.Log(movablePos.Count);

//    if (movablePos.Count > 0)
//    {
//        owner.transform.position = movablePos[movablePos.Count - 1];
//    }

//    // 그 중에서 가장 가까운 총알 궤적까지의 거리가 가장 큰 위치를 선택
//    if (movablePos.Count > 0)
//    {

//    }
//}