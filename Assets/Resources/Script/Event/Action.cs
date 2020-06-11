using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace VEPT
{
    // TODO: 각 action의 init 구현
    public abstract class Action
    {
        public float delay = 0f;

        public Action(Trigger trigger)
        {
            trigger.actionList.Add(this);
        }

        protected abstract void ActionProcess(Trigger trigger);

        public void Activate(Trigger trigger)
        {
            if (delay == 0f) ActionProcess(trigger);
            else GameManager.Instance.StartCoroutine(DelayedActivate(trigger));
        }

        public virtual void Init()
        {

        }

        private IEnumerator DelayedActivate(Trigger trigger)
        {
            yield return new WaitForSeconds(delay);
            ActionProcess(trigger);
        }
    }

    public class ActSetValue<T> : Action where T : struct
    {
        public ValueTypeWrapper<T> target;
        public T value;

        public ActSetValue(Trigger trigger, ValueTypeWrapper<T> _target, T _value)
            : base(trigger)
        {
            target = _target;
            value = _value;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            target.value = value;
        }
    }

    public class ActDirectionToMouse : Action
    {
        public Actor target;

        public ActDirectionToMouse(Trigger trigger, Actor _target)
            : base(trigger)
        {
            target = _target;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            Vector2 mouseWorldPos = VEasyCalc.ScreenToWorldPos(Input.mousePosition);
            target.targetDir = VEasyCalc.GetDirection(target.transform.position, mouseWorldPos);
        }
    }

    public class ActDirectionToTarget : Action
    {
        public Actor from;
        public Actor to;

        public ActDirectionToTarget(Trigger trigger, Actor _from, Actor _to)
            : base(trigger)
        {
            from = _from;
            to = _to;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            from.targetDir = VEasyCalc.GetDirection(from, to);
        }
    }

    public class ActSetAnimatorSpeed : Action
    {
        public GameObject target;
        public float speed;

        public ActSetAnimatorSpeed(Trigger trigger, GameObject _target, float _speed)
            : base(trigger)
        {
            target = _target;
            speed = _speed;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            if (target.TryGetComponent(out Animator anim))
            {
                anim.speed = speed;
            }
        }
    }

    public class ActSetComponent<T> : Action where T : Component
    {
        public GameObject target;
        public bool isAdd;

        public ActSetComponent(Trigger trigger, GameObject _target, bool _isAdd)
            : base(trigger)
        {
            target = _target;
            isAdd = _isAdd;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            if (isAdd) target.AddComponent<T>();
            else
            {
                if (target.TryGetComponent<T>(out T component))
                {
                    GameObject.Destroy(component);
                }
            }
        }
    }

    public class ActSetController : Action
    {
        public GameObject target;
        public RuntimeAnimatorController controller;

        public ActSetController(Trigger trigger, GameObject _target, RuntimeAnimatorController _controller)
            : base(trigger)
        {
            target = _target;
            controller = _controller;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            if (target.TryGetComponent(out Animator anim))
            {
                anim.runtimeAnimatorController = controller;
            }
        }
    }

    public class ActSetSprite : Action
    {
        public SpriteRenderer spriteRenderer;
        public Sprite sprite;

        public ActSetSprite(Trigger trigger, SpriteRenderer _spriteRenderer, Sprite _sprite)
            : base(trigger)
        {
            spriteRenderer = _spriteRenderer;
            sprite = _sprite;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    public class ActSetSpriteColor : Action
    {
        public SpriteRenderer sprite;
        public Color color;

        public ActSetSpriteColor(Trigger trigger, SpriteRenderer _sprite, Color _color)
            : base(trigger)
        {
            sprite = _sprite;
            color = _color;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            sprite.color = color;
        }
    }

    public class ActInitTrigger : Action
    {
        public Trigger target;

        public ActInitTrigger(Trigger trigger, Trigger _target)
            : base(trigger)
        {
            target = _target;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            target.Init();
        }
    }

    public class ActSetSpeed : Action
    {
        public float speed;

        public ActSetSpeed(Trigger trigger, float _speed)
            : base(trigger)
        {
            speed = _speed;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            Movable move = trigger.owner.GetOperable<Movable>();
            if (move == null) return;

            move.speed = speed;
        }
    }

    public class ActPrintLog : Action
    {
        public string log;

        public ActPrintLog(Trigger trigger, string _log)
            : base(trigger)
        {
            log = _log;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            Debug.Log(log);
        }
    }

    public class ActKnockback : Action
    {
        public Actor target;
        public float speed;
        public float deceleration; // 초당 속도 감소

        public ActKnockback(Trigger trigger, Actor _target, float _speed, float _deceleration)
            : base(trigger)
        {
            target = _target;
            speed = _speed;
            deceleration = _deceleration;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            if (VEasyCalc.TryCast(trigger, out TrgCollision trgCol))
            {
                if (trgCol.target != null)
                {
                    target.moveDir = VEasyCalc.GetDirection(trgCol.target, target);
                }
            }
            else
            {
                target.moveDir += 180f;
            }

            GameManager.Instance.StartCoroutine(DecelerationProcess(trigger));
        }

        // 기존 Movable 일시 중지, 새로운 Movable 생성해서 넉백 이동
        private IEnumerator DecelerationProcess(Trigger trigger)
        {
            List<Movable> moves = target.GetOperableList<Movable>();

            if (moves != null)
                foreach (var move in moves)
                    move.state.SetState(MultiState.EStateType.KNOCKBACK, true);

            Actor.ERotateTo originRotateTo = target.rotateTo;

            MovableStraight knockbackMove = target.gameObject.AddComponent<MovableStraight>();
            target.rotateTo = Actor.ERotateTo.NONE;

            float currentSpeed = speed;

            while (currentSpeed > 0f && deceleration > 0f)
            {
                knockbackMove.speed = currentSpeed;
                currentSpeed -= deceleration * Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            target.rotateTo = originRotateTo;
            knockbackMove.speed = 0f;

            GameObject.Destroy(knockbackMove);

            if (moves != null)
                foreach (var move in moves)
                    move.state.SetState(MultiState.EStateType.KNOCKBACK, false);
        }
    }

    public class ActBlockMove : Action
    {
        public ActBlockMove(Trigger trigger)
            : base(trigger)
        {

        }

        protected override void ActionProcess(Trigger trigger)
        {
            Vector2 targetPos;
            float targetDir;
            float distance = 0f;

            Collidable col = trigger.owner.GetOperable<Collidable>();

            if (VEasyCalc.TryCast(trigger, out TrgCollision trgCol))
            {
                Actor target = trgCol.target;
                targetPos = target.transform.position;
                targetDir = VEasyCalc.GetDirection(trigger.owner, target);

                // TODO 유닛간 block 처리 구현
                // Contact, Raycast 등 활용?
            }
            else if (VEasyCalc.TryCast(trigger, out TrgBoundaryTouch trgBndTch))
            {
                targetPos = trgBndTch.targetPos;
                targetDir = trgBndTch.bounceTo;

                if (VEasyCalc.TryCast(col?.collider, out CircleCollider2D circleCol))
                {
                    distance = circleCol.radius;
                }
                // TODO box collider 처리
            }
            else if (VEasyCalc.TryCast(trigger, out TrgBoundaryOut regBndOut))
            {
                targetPos = regBndOut.targetPos;
                targetDir = regBndOut.bounceTo;

                if (VEasyCalc.TryCast(col?.collider, out CircleCollider2D circleCol))
                {
                    distance = circleCol.radius;
                }
            }
            else return;

            Vector2 delta = VEasyCalc.GetRotatedPosition(targetDir + 180f, distance);

            trigger.owner.transform.position = targetPos + delta;
        }
    }

    public class ActTurnReverse : Action
    {
        public ActTurnReverse(Trigger trigger)
            : base(trigger)
        {

        }

        protected override void ActionProcess(Trigger trigger)
        {
            trigger.owner.moveDir += 180f;
        }
    }

    public class ActTurnReflect : Action
    {
        public ActTurnReflect(Trigger trigger)
            : base(trigger)
        {

        }

        protected override void ActionProcess(Trigger trigger)
        {
            Actor owner = trigger?.owner;

            owner.moveDir = VEasyCalc.GetReflectedDirection(owner.moveDir, owner.targetDir);
        }
    }

    public class ActTurnTarget : Action
    {
        public ActTurnTarget(Trigger trigger)
            : base(trigger)
        {

        }

        protected override void ActionProcess(Trigger trigger)
        {
            trigger.owner.moveDir = trigger.owner.targetDir;
        }
    }

    public class ActActivatePattern : Action
    {
        public Pattern pattern;

        public ActActivatePattern(Trigger trigger, Pattern _pattern)
            : base(trigger)
        {
            pattern = _pattern;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            pattern.Activate();
        }
    }

    public class ActActiveOperable<T> : Action where T : Operable
    {
        protected bool doActive;
        protected MultiState.EStateType stateType;

        public ActActiveOperable(Trigger trigger, MultiState.EStateType _type, bool _doActive)
            : base(trigger)
        {
            stateType = _type;
            doActive = _doActive;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            Operable operable = trigger.owner.GetOperable<T>();
            operable.state.SetState(stateType, doActive);
        }
    }

    public class ActActiveTargetOperable<T> : ActActiveOperable<T> where T : Operable
    {
        public ActActiveTargetOperable(TrgCollision trigger,
            MultiState.EStateType _type, bool _doActive)
            : base(trigger, _type, _doActive)
        {

        }

        protected override void ActionProcess(Trigger trigger)
        {
            if (trigger == null) return;

            Actor target = trigger.owner;
            if (trigger is TrgCollision)
                target = (trigger as TrgCollision).target;

            if (target == null) return;

            Operable operable = target.GetOperable<T>();
            operable.state.SetState(stateType, doActive);
        }
    }

    //public class ActAddOperable : Action
    //{
    //    public ActAddOperable(Trigger trigger)
    //        : base(trigger)
    //    {

    //    }

    //    protected override void ActionProcess(Trigger trigger)
    //    {
    //        // TODO

    //    }
    //}

    public class ActCreateActor : Action
    {
        public string prefabName;
        public Vector2 position;
        public bool isMovingActor;
        public float direction;
        public float speed;

        public ActCreateActor(Trigger trigger, string _prefabName)
            : base(trigger)
        {
            prefabName = _prefabName;
            position = new Vector2(0, 0);

            isMovingActor = false;
        }

        public ActCreateActor(Trigger trigger, string _prefabName, Vector2 _pos)
            : base(trigger)
        {
            prefabName = _prefabName;
            position = _pos;

            isMovingActor = false;
        }

        public ActCreateActor(Trigger trigger, string _prefabName, Vector2 _pos, float _direction, float _speed)
            : base(trigger)
        {
            prefabName = _prefabName;
            position = _pos;

            isMovingActor = true;

            direction = _direction;
            speed = _speed;
        }

        public ActCreateActor(Trigger trigger, EResourceName _prefabName)
            : this(trigger, _prefabName.ToString()) { }

        public ActCreateActor(Trigger trigger, EResourceName _prefabName, Vector2 _pos)
            : this(trigger, _prefabName.ToString(), _pos) { }

        public ActCreateActor(Trigger trigger, EResourceName _prefabName,
            Vector2 _pos, float _direction, float _speed)
            : this(trigger, _prefabName.ToString(), _pos, _direction, _speed) { }

        protected override void ActionProcess(Trigger trigger)
        {
            Actor actor = PoolerManager.GetObjectRequest(prefabName).GetComponent<Actor>();

            try
            {
                actor.transform.position = position;
                actor.moveDir = direction;

                if (isMovingActor)
                {
                    Movable move = actor.GetOperable<Movable>();
                    move.speed = speed;
                }
            }
            catch(NullReferenceException e)
            {
                Debug.LogError(e);
            }
        }
    }

    public class ActCreateObject : Action
    {
        public string prefabName;
        public Vector2 position;
        public float direction;

        public ActCreateObject(Trigger trigger, string _prefabName, Vector2 _position, float _direction)
            : base(trigger)
        {
            prefabName = _prefabName;
            position = _position;
            direction = _direction;
        }

        public ActCreateObject(Trigger trigger, EResourceName _prefabName, Vector2 _position, float _direction)
            : this(trigger, _prefabName.ToString(), _position, _direction) { }

        protected override void ActionProcess(Trigger trigger)
        {
            try
            {
                GameObject go = PoolerManager.GetObjectRequest(prefabName);

                go.transform.position = position;
                go.transform.rotation = Quaternion.Euler(0, 0, direction);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }

    public class ActCreateObjectDynamic : Action
    {
        public GameObject prefab;
        public Transform transform;

        public ActCreateObjectDynamic(Trigger trigger, GameObject _prefab, Transform _transform)
            : base(trigger)
        {
            prefab = _prefab;
            transform = _transform;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            try
            {
                GameObject go = UnityEngine.Object.Instantiate(prefab);

                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
            }
            catch (NullReferenceException e)
            {
                Debug.LogError(e);
            }
        }
    }

    public class ActDealDamage : Action
    {
        public int damage;

        public ActDealDamage(TrgCollision trigger, int _damage)
            : base(trigger)
        {
            damage = _damage;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            TrgCollision trgCol = trigger as TrgCollision;
            if (trgCol == null) return;

            if (trgCol.target == null) return;

            Unit targetUnit = trgCol.target as Unit;
            if (targetUnit == null) return;

            if (targetUnit.unitStatus == null) return;

            if (targetUnit.TryGetOperable(out ShieldOwnable shield) &&
                shield.enableShield)
                shield.ShieldBreak();
            else targetUnit.unitStatus.CurrentHp -= damage;
        }
    }

    public class ActGetDamage : Action
    {
        public int damage;

        public ActGetDamage(Trigger trigger, int _damage)
            : base(trigger)
        {
            damage = _damage;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            Unit unit = trigger.owner as Unit;
            if (unit == null) return;
            if (unit.unitStatus == null) return;

            if (unit.TryGetOperable(out ShieldOwnable shield) &&
                shield.enableShield)
                shield.ShieldBreak();
            else unit.unitStatus.CurrentHp -= damage;
        }
    }

    public class ActDestroyActor : Action
    {
        public Actor target;

        public ActDestroyActor(Trigger trigger, Actor _target)
            : base(trigger)
        {
            target = _target;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            target.willDestroy = true;
        }
    }

    public class ActVectorMoveActor : Action
    {
        public override void Init()
        {
            base.Init();
            moveVector = new bool[4];
        }
        public float speed;

        bool[] moveVector = new bool[4];

        Dictionary<EDirection, KeyManager.EKeyCommand> dirKeyDic =
            new Dictionary<EDirection, KeyManager.EKeyCommand>();
        Dictionary<EDirection, EDirection> dirRevdirDic =
            new Dictionary<EDirection, EDirection>();

        public ActVectorMoveActor(TrgKeyInputs trigger, float _speed)
            : base(trigger)
        {
            speed = _speed;

            dirKeyDic[EDirection.LEFT] = KeyManager.EKeyCommand.MOVE_LEFT;
            dirKeyDic[EDirection.RIGHT] = KeyManager.EKeyCommand.MOVE_RIGHT;
            dirKeyDic[EDirection.UP] = KeyManager.EKeyCommand.MOVE_UP;
            dirKeyDic[EDirection.DOWN] = KeyManager.EKeyCommand.MOVE_DOWN;

            dirRevdirDic[EDirection.LEFT] = EDirection.RIGHT;
            dirRevdirDic[EDirection.RIGHT] = EDirection.LEFT;
            dirRevdirDic[EDirection.UP] = EDirection.DOWN;
            dirRevdirDic[EDirection.DOWN] = EDirection.UP;
        }

        protected override void ActionProcess(Trigger trigger)
        {
            if (VEasyCalc.TryCast(trigger, out TrgKeyInputs inpputs))
            {
                // TODO
            }

            TrgKeyInputs inputs = (TrgKeyInputs)trigger;

            UpdateMoveState(inputs.command, inputs.pressType);

            MovableVector mv = inputs.owner.GetOperable<Movable>() as MovableVector;

            if (mv == null)
            {
                mv = inputs.owner.gameObject.AddComponent<MovableVector>();
                mv.speed = speed;
            }

            mv.moveVector = moveVector;
        }

        void UpdateMoveState(KeyManager.EKeyCommand command, KeyManager.EKeyPressType type)
        {
            for (int d = 0; d < 4; ++d)
            {
                EDirection dir = (EDirection)d;
                if (command == dirKeyDic[dir])
                {
                    if (type == KeyManager.EKeyPressType.DOWN)
                    {
                        moveVector[(int)dir] = true;

                        moveVector[(int)dirRevdirDic[dir]] = false;
                    }
                    else if (type == KeyManager.EKeyPressType.PRESS)
                    {
                        if (moveVector[(int)dirRevdirDic[dir]] == false)
                        {
                            moveVector[(int)dir] = true;
                        }
                    }
                    else if (type == KeyManager.EKeyPressType.UP)
                    {
                        moveVector[(int)dir] = false;
                    }

                    return;
                }
            }
        }
    }
}