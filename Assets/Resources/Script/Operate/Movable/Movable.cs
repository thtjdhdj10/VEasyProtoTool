using UnityEngine;
using System.Collections.Generic;

public class Movable : Operable
{
    public float speed = 1f;
    public bool isRotate = true;

    public float direction;
    public Unit target;

    protected virtual void FixedUpdate()
    {
        MoveFrame();

        BounceProcessing();

        if (isRotate) SetSpriteAngle();
    }

    protected virtual void MoveFrame()
    {

    }

    public bool enableBounce;
    public float bounceCooldown;
    private float remainBounceCooldown;
    public BounceBy bounceBy;
    public BounceTo bounceTo;

    public enum BounceBy
    {
        WORLD_BOUNDARY,
        UNIT,
        ENEMY,
        ALLY,
    }
    
    public enum BounceTo
    {
        TARGET,
        REVERSE, // dir +180
        REFLECT, // 거울반사
    }

    public virtual void SetSpriteAngle()
    {
        Vector3 rot = transform.eulerAngles;
        rot.z = direction + SpriteManager.spriteDefaultRotation;
        transform.eulerAngles = rot;
    }

    protected virtual void BounceProcessing()
    {
        if (enableBounce == false) return;

        bool isBounced = false;
        float targetDir = 0f;

        switch (bounceBy)
        {
            case BounceBy.WORLD_BOUNDARY:
                {
                    Collidable col = owner.GetOperable<Collidable>();
                    GameManager.Direction bounceByDir = VEasyCalculator.CheckTerritory2D(col.collider);
                    Debug.Log(bounceByDir);
                    isBounced = true;
                    switch (bounceByDir)
                    {
                        case GameManager.Direction.NONE:
                            isBounced = false;
                            break;
                        case GameManager.Direction.UP:
                            targetDir = 90f;
                            break;
                        case GameManager.Direction.DOWN:
                            targetDir = 270f;
                            break;
                        case GameManager.Direction.LEFT:
                            targetDir = 180f;
                            break;
                        case GameManager.Direction.RIGHT:
                            targetDir = 0f;
                            break;
                    }
                }
                break;
            case BounceBy.UNIT:
            case BounceBy.ALLY:
            case BounceBy.ENEMY:
                {
                    Collidable col = owner.GetOperable<Collidable>();
                    if (col != null)
                    {
                        Unit.Relation targetRelation = Unit.Relation.NONE;
                        if (bounceBy == BounceBy.UNIT)
                            targetRelation = Unit.Relation.ALLY_OR_ENEMY;
                        else if (bounceBy == BounceBy.ALLY)
                            targetRelation = Unit.Relation.ALLY;
                        else if (bounceBy == BounceBy.ENEMY)
                            targetRelation = Unit.Relation.ENEMY;

                        Collidable colTarget = col.FirstCollisionCheck(targetRelation);

                        if (colTarget != null)
                        {
                            isBounced = true;
                            targetDir = VEasyCalculator.GetDirection(owner, colTarget.owner);
                        }
                    }
                }
                break;
        }

        if(isBounced)
        {
            switch (bounceTo)
            {
                case BounceTo.TARGET:
                    direction = targetDir;
                    break;
                case BounceTo.REVERSE:
                    direction += 180f;
                    break;
                case BounceTo.REFLECT:
                    direction = VEasyCalculator.GetReflectedDirection(direction, targetDir);
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
//// TODO
// 생각해봤는데, InitLerpCurve 와 InitLerpCurvePerDistanceMove 는
// 필요로 하는 멤버가 다르므로, 다른 기능이라고 봐야 함.
// 따라서 클래스를 따로 하는게 의미상 명확.

//public float maxCurveDistance;

//public float minCurveDistance;

//    public float distanceFactor;

//public void Init(float spd, float dir, Unit tar, float curve, float maxDis, float minDis, float disFactor)
//{
//    target = tar;

//    speed = spd;
//    direction = dir;
//    curveFactor = curve;
//    maxCurveDistance = maxDis;
//    minCurveDistance = minDis;
//    distanceFactor = disFactor;
//}

//// 목적과의 방향차이와 거리차이에 비례해서 선회
//protected virtual void LerpCurvePerDistanceMove()
//{
//    float moveDistance = speed * Time.fixedDeltaTime;

//    float dirToPlayer = VEasyCalculator.GetDirection(owner.transform.position, target.transform.position);

//    float disToPlayer = Vector2.Distance(target.transform.position, owner.transform.position);

//    direction = VEasyCalculator.GetLerpDirection(
//        direction, dirToPlayer, curveFactor * Time.fixedDeltaTime,
//        maxCurveDistance, minCurveDistance, disToPlayer, distanceFactor);

//    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(direction, moveDistance);

//    Vector2 v2Pos = owner.transform.position;
//    owner.transform.position = v2Pos + moveVector;
//}


//public float maxCurveDistance;

//public float minCurveDistance;

//public float distanceFactor;

//public void Init(float spd, float dir, Unit tar, float curve, float maxDis, float minDis, float disFactor)
//{
//    target = tar;

//    speed = spd;
//    direction = dir;
//    curveFactor = curve;
//    maxCurveDistance = maxDis;
//    minCurveDistance = minDis;
//    distanceFactor = disFactor;
//}

//// 방향 무관. 거리 차이에 비례해 선회
//protected virtual void RegularCurvePerDistanceMove()
//{
//    float moveDistance = speed * Time.fixedDeltaTime;

//    float dirToPlayer = VEasyCalculator.GetDirection(owner.transform.position, target.transform.position);

//    float disToPlayer = Vector2.Distance(target.transform.position, owner.transform.position);

//    direction = VEasyCalculator.GetTurningDirection(
//        direction, dirToPlayer, curveFactor * Time.fixedDeltaTime,
//        maxCurveDistance, minCurveDistance, disToPlayer, distanceFactor * Time.fixedDeltaTime);

//    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(direction, moveDistance);

//    Vector2 v2Pos = owner.transform.position;
//    owner.transform.position = v2Pos + moveVector;
//}
