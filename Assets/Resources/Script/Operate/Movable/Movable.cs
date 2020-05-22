using UnityEngine;
using System.Collections.Generic;

public abstract class Movable : Operable
{
    public float speed = 1f;
    public float _bounceCooldown;

    public RefBoolean _enableBounceRef = new RefBoolean(false);
    public List<EBounceTrigger> _bounceTriggerList = new List<EBounceTrigger>();
    public EBounceTo _bounceTo;

    protected Vector2 _targetPos;

    protected abstract void MoveFrame();

    public enum EBounceTrigger
    {
        NONE = 0,
        BOUNDARY_TOUCH,
        BOUNDARY_OUT,
        COLLISION,
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

    protected virtual void FixedUpdate()
    {
        if (state == false) return;

        if (owner.TryGetOperable(out Targetable ownerTarget))
        {
            if (ownerTarget.target != null)
                _targetPos = ownerTarget.target.transform.position;
        }

        // TODO: 프로그램 실행 중간에 bounce trigger,bounce to 설정이 바뀌는 경우 처리
        SetTriggerAction();

        MoveFrame();
    }

    protected virtual void SetTriggerAction()
    {
        foreach(var bt in _bounceTriggerList)
        {
            Trigger trigger = null;
            switch (bt)
            {
                case EBounceTrigger.BOUNDARY_TOUCH:
                    trigger = new TrgBoundaryTouch(owner);
                    break;
                case EBounceTrigger.BOUNDARY_OUT:
                    trigger = new TrgBoundaryOut(owner);
                    break;
                case EBounceTrigger.COLLISION:
                    Collidable col = owner.GetOperable<Collidable>();
                    trigger = new TrgCollision(owner, col, typeof(Unit));
                    break;
            }

            if (trigger == null) return;

            new CndEnable(trigger, _enableBounceRef);

            switch (_bounceTo)
            {
                case EBounceTo.REVERSE:
                    new ActTurnReverse(trigger);
                    break;
                case EBounceTo.REFLECT:
                    new ActTurnReflect(trigger);
                    break;
                case EBounceTo.TARGET:
                    new ActTurnTarget(trigger);
                    break;
                case EBounceTo.BLOCK:
                    new ActBlockMove(trigger, speed);
                    break;
                case EBounceTo.DESTROY:
                    new ActDestroyActor(trigger, owner);
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
// TODO
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