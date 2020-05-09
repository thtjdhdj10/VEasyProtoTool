﻿using UnityEngine;
using System.Collections.Generic;

public abstract class Movable : Operable
{
    public float speed = 1f;

    protected Vector2 targetPos;

    protected virtual void FixedUpdate()
    {
        if (_state == false) return;

        if(_owner.TryGetOperable(out Targetable ownerTarget))
        {
            if (ownerTarget.target != null)
                targetPos = ownerTarget.target.transform.position;
        }

        // TODO bounce랑 충돌 동시에 되면 동시에 수행이 안되고 하나가 먼저됨
        // 그거땜에 다른 하나가 동작안할때가있음
        // hit 충돌처리할때 다른 하나가 이미 destroy 됐을수도 있음

        MoveFrame();

        BounceProcessing();
    }

    protected abstract void MoveFrame();

    public bool enableBounce;
    public float bounceCooldown;
    private float remainBounceCooldown;
    public BounceBy bounceBy;
    public BounceTo bounceTo;

    public enum BounceBy
    {
        NONE = 0,
        ALL,
        BOUNDARY_TOUCH,
        BOUNDARY_OUT,
        UNIT,
        ENEMY,
        ALLY,
    }
    
    public enum BounceTo
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
        if (enableBounce == false) return;

        if (remainBounceCooldown > 0f)
        {
            remainBounceCooldown -= Time.fixedDeltaTime;
            return;
        }
        else remainBounceCooldown = bounceCooldown;

        bool isBounced = false;
        float targetDir = 0f;

        if (bounceBy == BounceBy.ALL ||
            bounceBy == BounceBy.BOUNDARY_TOUCH)
        {
            Collidable col = _owner.GetOperable<Collidable>();
            GameManager.Direction bounceByDir = VEasyCalculator.CheckTerritory2D(col.collider);

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
        else if (bounceBy == BounceBy.ALL ||
            bounceBy == BounceBy.BOUNDARY_OUT)
        {
            Collidable col = _owner.GetOperable<Collidable>();
            GameManager.Direction bounceByDir = VEasyCalculator.CheckOutside2D(col.collider);

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
        else if (bounceBy == BounceBy.ALL ||
            bounceBy == BounceBy.UNIT ||
            bounceBy == BounceBy.ALLY ||
            bounceBy == BounceBy.ENEMY)
        {
            if(_owner.TryGetOperable(out Collidable col))
            {
                Unit.Relation targetRelation = Unit.Relation.NONE;
                if (bounceBy == BounceBy.UNIT)
                    targetRelation = Unit.Relation.NEUTRAL;
                else if (bounceBy == BounceBy.ALLY)
                    targetRelation = Unit.Relation.ALLY;
                else if (bounceBy == BounceBy.ENEMY)
                    targetRelation = Unit.Relation.ENEMY;

                Collidable colTarget = col.FirstCollisionCheck(targetRelation);

                if (colTarget != null)
                {
                    isBounced = true;
                    targetDir = VEasyCalculator.GetDirection(_owner.transform.position, targetPos);
                }
            }
        }

        if(isBounced)
        {
            switch (bounceTo)
            {
                case BounceTo.TARGET:
                    _owner._moveDirection = targetDir;
                    break;
                case BounceTo.REVERSE:
                    _owner._moveDirection += 180f;
                    break;
                case BounceTo.REFLECT:
                    _owner._moveDirection = VEasyCalculator.GetReflectedDirection(_owner._moveDirection, targetDir);
                    break;
                case BounceTo.BLOCK:
                    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(
                        _owner._moveDirection, 1f);
                    Vector2 targetVector = VEasyCalculator.GetRotatedPosition(
                        targetDir, 1f);

                    float inner = VEasyCalculator.Inner(moveVector, targetVector);

                    Vector2 escapeVector = VEasyCalculator.GetRotatedPosition(
                        targetDir + 180f, inner * speed * Time.fixedDeltaTime);

                    _owner.transform.position = (Vector2)_owner.transform.position + escapeVector;
                    // TODO 최적화 및 벽에서 달달달 안하게
                    break;
                case BounceTo.DESTROY:
                    Destroy(_owner.gameObject);
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