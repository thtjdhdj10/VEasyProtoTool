using UnityEngine;
using System.Collections.Generic;

public class Movable : Operable
{
    public Unit owner;

    public float speed = 1f;
    public bool isRotate = true;
    public BounceType bounceType;

    public float direction;
    public Unit target;

    public static List<Movable> movableList = new List<Movable>();

    protected virtual void Awake()
    {
        owner = GetComponent<Unit>();

        movableList.Add(this);
    }

    protected virtual void OnDestroy()
    {
        movableList.Remove(this);
    }

    protected virtual void FixedUpdate()
    {
        MoveFrame();

        if (bounceType != BounceType.NONE) BounceProcessing();

        if (isRotate) SetSpriteAngle();
    }

    protected virtual void MoveFrame()
    {

    }

    public enum BounceType
    {
        // BOUNCE_충돌 대상_충돌 후 향하는 방향
        NONE = 0,
        BOUNCE_WALL_TARGET,
        BOUNCE_WALL_REVERSE,
        BOUNCE_UNIT_TARGET,
        BOUNCE_UNIT_REVERSE,
        BOUNCE_ENEMY_TARGET,
        BOUNCE_ENEMY_REVERSE,
    }

    public virtual void SetSpriteAngle()
    {
        Vector3 rot = transform.eulerAngles;
        rot.z = direction + SpriteManager.spriteDefaultRotation;
        transform.eulerAngles = rot;
    }

    protected virtual void BounceProcessing()
    {
        switch (bounceType)
        {
            case BounceType.BOUNCE_WALL_TARGET:
                {
                    float dirToTarget = VEasyCalculator.GetDirection(owner, target);
                    direction = dirToTarget;
                }
                break;
            case BounceType.BOUNCE_WALL_REVERSE:
                {
                    VEasyCalculator.GetNormalizedDirection(ref direction);
                    if (VEasyCalculator.CheckTerritory(owner) == GameManager.Direction.DOWN)
                    {
                        if (direction >= 90f &&
                            direction < 270f)
                        {
                            direction = 180f - direction;
                        }
                    }
                    else if (VEasyCalculator.CheckTerritory(owner) == GameManager.Direction.LEFT)
                    {
                        if (direction < 180f)
                        {
                            direction = 360f - direction;
                        }
                    }
                    else if (VEasyCalculator.CheckTerritory(owner) == GameManager.Direction.RIGHT)
                    {
                        if (direction >= 180f)
                        {
                            direction = 360f - direction;
                        }
                    }
                    else if (VEasyCalculator.CheckTerritory(owner) == GameManager.Direction.UP)
                    {
                        if (direction < 90f ||
                            direction >= 270f)
                        {
                            direction = 180f - direction;
                        }
                    }
                    VEasyCalculator.GetNormalizedDirection(ref direction);
                }
                break;
            case BounceType.BOUNCE_UNIT_TARGET:
                {
                    Collidable col = (Collidable)owner.GetOperable(typeof(Collidable));
                    if (col != null)
                    {
                        Unit colUnit = col.FirstCollisionCheck(Unit.Relation.ALL).owner;

                        if (colUnit != null)
                        {
                            float dirToTarget = VEasyCalculator.GetDirection(owner, target);
                            direction = dirToTarget;
                        }
                    }
                }
                break;
            case BounceType.BOUNCE_UNIT_REVERSE:
                {
                    Collidable col = (Collidable)owner.GetOperable(typeof(Collidable));
                    if (col != null)
                    {
                        Unit colUnit = col.FirstCollisionCheck(Unit.Relation.ALL).owner;

                        if (colUnit != null)
                        {
                            direction += 180f;
                            VEasyCalculator.GetNormalizedDirection(ref direction);
                        }
                    }
                }
                break;
            case BounceType.BOUNCE_ENEMY_TARGET:
                {
                    Collidable col = (Collidable)owner.GetOperable(typeof(Collidable));
                    if (col != null)
                    {
                        Unit colUnit = col.FirstCollisionCheck(Unit.Relation.ENEMY).owner;

                        if (colUnit != null)
                        {
                            float dirToTarget = VEasyCalculator.GetDirection(owner, target);
                            direction = dirToTarget;
                        }
                    }
                }
                break;
            case BounceType.BOUNCE_ENEMY_REVERSE:
                {
                    Collidable col = (Collidable)owner.GetOperable(typeof(Collidable));
                    if (col != null)
                    {
                        Unit colUnit = col.FirstCollisionCheck(Unit.Relation.ENEMY).owner;

                        if (colUnit != null)
                        {
                            direction += 180f;
                            VEasyCalculator.GetNormalizedDirection(ref direction);
                        }
                    }
                }
                break;
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
