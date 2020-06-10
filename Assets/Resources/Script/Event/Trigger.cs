using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace VEPT
{
    // TODO: 각 trigger의 init 구현
    public abstract class Trigger
    {
        public Actor owner;

        // 검사하기 쉬운 조건을 먼저 Add 할 것.
        public List<Condition> conditionList = new List<Condition>();
        public List<Action> actionList = new List<Action>();

        // Option
        public bool isDisposableTrigger = false; // Trigger 가 작동할 때 까지 존재.
        public bool isDisposableAction = false; // Action 이 한 번 실행될 때 까지 존재.

        public Trigger(Actor _owner)
        {
            owner = _owner;
            owner.triggerList.Add(this);
        }

        ~Trigger()
        {
            owner.triggerList.Remove(this);
        }

        public virtual void Init()
        {
            actionList.ForEach(a => a.Init());
            conditionList.ForEach(c => c.Init());
        }

        public virtual void ActivateTrigger()
        {
            if (conditionList.Any(c => c.CheckCondition() == false)) return;

            actionList.ForEach(a =>
            {
                a.Activate(this);
                if (isDisposableAction)
                    owner.triggerList.Remove(this);
            });

            if (isDisposableTrigger == true)
                owner.triggerList.Remove(this);
        }

    }

    public class TrgCollision : Trigger
    {
        public Type[] targetTypes;
        public Actor target;

        public TrgCollision(Actor _owner, params Type[] _targetTypes)
            : base(_owner)
        {
            if (owner.TryGetOperable(out Collidable collidable))
            {
                collidable.onHitDlg += HandleOnHit;
            }
            else
            {
                Debug.LogWarning(owner + " doesn't have collidable");
            }

            targetTypes = _targetTypes;
        }

        public override void Init()
        {
            base.Init();
            target = null;
        }

        private void HandleOnHit(Actor from, Actor to)
        {
            if (from != owner) return;

            foreach (var targetType in targetTypes)
            {
                if (to.GetType().IsSubclassOf(targetType) ||
                    to.GetType() == targetType)
                {
                    target = to;
                    ActivateTrigger();
                }
            }
        }
    }

    public class TrgBoundaryTouch : Trigger
    {
        public Vector2 targetPos;
        public float bounceTo;

        public TrgBoundaryTouch(Actor _owner)
            : base(_owner)
        {
            _owner.fixedUpdateDlg += HandleFixedUpdate;
        }

        private void HandleFixedUpdate()
        {
            Collidable col = owner.GetOperable<Collidable>();
            EDirection bounceByDir = VEasyCalc.CheckTerritory2D(col.collider);

            switch (bounceByDir)
            {
                case EDirection.UP:
                    targetPos = new Vector2(owner.transform.position.x, CameraManager.WorldHeightHalf);
                    bounceTo = 90f;
                    ActivateTrigger();
                    break;
                case EDirection.DOWN:
                    targetPos = new Vector2(owner.transform.position.x, -CameraManager.WorldHeightHalf);
                    bounceTo = 270f;
                    ActivateTrigger();
                    break;
                case EDirection.LEFT:
                    targetPos = new Vector2(-CameraManager.WorldWidthHalf, owner.transform.position.y);
                    bounceTo = 180f;
                    ActivateTrigger();
                    break;
                case EDirection.RIGHT:
                    targetPos = new Vector2(CameraManager.WorldWidthHalf, owner.transform.position.y);
                    bounceTo = 0f;
                    ActivateTrigger();
                    break;
            }
        }
    }

    public class TrgBoundaryOut : Trigger
    {
        public Vector2 targetPos;
        public float bounceTo;

        public TrgBoundaryOut(Actor _owner)
            : base(_owner)
        {
            _owner.fixedUpdateDlg += HandleFixedUpdate;
        }

        private void HandleFixedUpdate()
        {
            Collidable col = owner.GetOperable<Collidable>();
            EDirection bounceByDir = VEasyCalc.CheckOutside2D(col.collider);

            switch (bounceByDir)
            {
                case EDirection.UP:
                    targetPos = new Vector2(owner.transform.position.x, CameraManager.WorldHeightHalf);
                    bounceTo = 90f;
                    ActivateTrigger();
                    break;
                case EDirection.DOWN:
                    targetPos = new Vector2(owner.transform.position.x, -CameraManager.WorldHeightHalf);
                    bounceTo = 270f;
                    ActivateTrigger();
                    break;
                case EDirection.LEFT:
                    targetPos = new Vector2(-CameraManager.WorldWidthHalf, owner.transform.position.y);
                    bounceTo = 180f;
                    ActivateTrigger();
                    break;
                case EDirection.RIGHT:
                    targetPos = new Vector2(CameraManager.WorldWidthHalf, owner.transform.position.y);
                    bounceTo = 0f;
                    ActivateTrigger();
                    break;
            }
        }
    }

    public class TrgFrame : Trigger
    {
        public TrgFrame(Actor _owner, int _passCount)
            : base(_owner)
        {
            passCount = _passCount;

            _owner.fixedUpdateDlg += HandleFixedUpdate;
        }

        public int passCount = 0;
        int counter;

        public override void Init()
        {
            base.Init();

            counter = 0;
        }

        private void HandleFixedUpdate()
        {
            if (passCount == 0)
            {
                ActivateTrigger();
            }
            else
            {
                if (counter < passCount)
                {
                    counter++;
                }
                else
                {
                    counter = 0;
                    ActivateTrigger();
                }
            }
        }
    }

    // TrgKeyInput 은 정해진 키 입력에 대해서만 Activate() 를 호출.
    public class TrgKeyInput : Trigger
    {
        private KeyManager.EKeyCommand command;
        private KeyManager.EKeyPressType pressType;

        public TrgKeyInput(Actor _owner, KeyManager.EKeyCommand _command, KeyManager.EKeyPressType _pressType)
            : base(_owner)
        {
            command = _command;
            pressType = _pressType;

            if (owner.TryGetOperable(out Controllable control))
            {
                control.keyInputDlg += HandleKeyInput;
            }
            else
            {
                Debug.LogWarning(owner + " doesn't have controllable");
            }
        }

        ~TrgKeyInput()
        {
            if (owner.TryGetOperable(out Controllable control))
            {
                control.keyInputDlg -= HandleKeyInput;
            }
        }

        private void HandleKeyInput(KeyManager.EKeyCommand _command, KeyManager.EKeyPressType _pressType)
        {
            if (command == _command && pressType == _pressType)
                ActivateTrigger();
        }
    }

    // TrKeyInputs 는 정해진 대상 actor의  Activate() 호출
    // Action 내에서 명령어를 선별해서 사용.
    // 하나의 Action 에 복수의 명령어가 입력될 수 있을 때 사용할 것.
    public class TrgKeyInputs : Trigger
    {
        public KeyManager.EKeyCommand command;
        public KeyManager.EKeyPressType pressType;

        public TrgKeyInputs(Actor _owner)
            : base(_owner)
        {
            if (owner.TryGetOperable(out Controllable control))
            {
                control.keyInputDlg += HandleKeyInput;
            }
            else
            {
                Debug.LogWarning(owner + " doesn't have controllable");
            }
        }

        ~TrgKeyInputs()
        {
            if (owner.TryGetOperable(out Controllable control))
            {
                control.keyInputDlg -= HandleKeyInput;
            }
        }

        public override void Init()
        {
            base.Init();
            command = KeyManager.EKeyCommand.NONE;
            pressType = KeyManager.EKeyPressType.NONE;
        }

        private void HandleKeyInput(KeyManager.EKeyCommand _command, KeyManager.EKeyPressType _pressType)
        {
            command = _command;
            pressType = _pressType;
            ActivateTrigger();
        }
    }

    public class TrgTimer : Trigger
    {
        public TrgTimer(Actor _owner, float _delay, bool _isActivateOnStart)
            : base(_owner)
        {
            delay = _delay;
            isActivateOnStart = _isActivateOnStart;

            _owner.fixedUpdateDlg += HandleFixedUpdate;

            if (isActivateOnStart == true)
                remainDelay = delay;
            else remainDelay = 0f;
        }

        public float delay;
        private float remainDelay;
        private bool isActivateOnStart;

        public override void Init()
        {
            base.Init();

            if (isActivateOnStart == true)
                remainDelay = delay;
            else remainDelay = 0f;
        }

        private void HandleFixedUpdate()
        {
            if (remainDelay >= delay)
            {
                ActivateTrigger();
                remainDelay = 0f;
            }
            else
            {
                remainDelay += Time.fixedDeltaTime;
            }
        }
    }

    // 특정한 유닛의 생성/파괴/초기화 시 Activate
    public class TrgUnitEvent : Trigger
    {
        private Unit target;
        private ETriggerType type;

        public TrgUnitEvent(Actor _owner, Unit _target, ETriggerType _type)
            : base(_owner)
        {
            target = _target;
            type = _type;

            switch (type)
            {
                case ETriggerType.AWAKE:
                    target.awakeDlg += ActivateTrigger;
                    break;
                case ETriggerType.INIT:
                    target.initDlg += ActivateTrigger;
                    break;
                case ETriggerType.ON_DESTROY:
                    target.onDestroyDlg += ActivateTrigger;
                    break;
            }
        }

        ~TrgUnitEvent()
        {
            switch (type)
            {
                case ETriggerType.AWAKE:
                    target.awakeDlg -= ActivateTrigger;
                    break;
                case ETriggerType.INIT:
                    target.initDlg -= ActivateTrigger;
                    break;
                case ETriggerType.ON_DESTROY:
                    target.onDestroyDlg -= ActivateTrigger;
                    break;
            }
        }

        public enum ETriggerType
        {
            AWAKE,
            INIT,
            ON_DESTROY,
        }
    }

    // 특정 type의 유닛 생성/파괴/초기화 시 동작
    public class TrgAnyUnitEvent : Trigger
    {
        private Type targetType;
        private ETriggerType type;

        public TrgAnyUnitEvent(Actor _owner, Type _unitType, ETriggerType _type)
            : base(_owner)
        {
            targetType = _unitType;
            type = _type;

            foreach (var unit in Unit.unitList)
            {
                LinkEventHandle(unit, true);
            }

            Actor.onActorAddedDlg += HandleAddedUnit;
        }

        ~TrgAnyUnitEvent()
        {
            foreach (var unit in Unit.unitList)
            {
                LinkEventHandle(unit, false);
            }
        }

        private void LinkEventHandle(Unit unit, bool isAdd)
        {
            switch (type)
            {
                case ETriggerType.AWAKE:
                    if (isAdd) unit.awakeDlg += ActivateTrigger;
                    else unit.awakeDlg -= ActivateTrigger;
                    break;
                case ETriggerType.INIT:
                    if (isAdd) unit.initDlg += ActivateTrigger;
                    else unit.initDlg -= ActivateTrigger;
                    break;
                case ETriggerType.ON_DESTROY:
                    if (isAdd) unit.onDestroyDlg += ActivateTrigger;
                    else unit.onDestroyDlg -= ActivateTrigger;
                    break;
            }
        }

        private void HandleAddedUnit(Actor actor)
        {
            Unit unit = actor as Unit;
            if (unit == null) return;

            if (actor.GetType().IsSubclassOf(targetType) ||
                actor.GetType() == targetType)
            {
                LinkEventHandle(unit, true);
            }
        }

        public enum ETriggerType
        {
            AWAKE,
            INIT,
            ON_DESTROY,
        }
    }
}