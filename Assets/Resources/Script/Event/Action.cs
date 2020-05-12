using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        else GameManager.gm.StartCoroutine(DelayedActivate(trigger));
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

public class ActSetConditionBool : Action
{
    public ConditionBool target;
    public bool state;

    public ActSetConditionBool(Trigger trigger, ConditionBool _target, bool _state)
        : base(trigger)
    {
        state = _state;
        target = _target;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        target.state = state;
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
        Vector2 mouseWorldPos = VEasyCalculator.ScreenToWorldPos(Input.mousePosition);
        target.targetDirection = VEasyCalculator.GetDirection(target.transform.position, mouseWorldPos);
    }
}

public class ActDirectionToTarget : Action
{
    public Actor from;
    public Actor to;

    public ActDirectionToTarget(Trigger trigger, Actor _from, Actor _to)
        :base(trigger)
    {
        from = _from;
        to = _to;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        from.targetDirection = VEasyCalculator.GetDirection(from, to);
    }
}

public class ActSetAnimatorSpeed : Action
{
    public GameObject target;
    public float speed;

    public ActSetAnimatorSpeed(Trigger trigger, GameObject _target, float _speed)
        :base(trigger)
    {
        target = _target;
        speed = _speed;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        if(target.TryGetComponent(out Animator anim))
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
        :base(trigger)
    {
        target = _target;
        controller = _controller;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        if(target.TryGetComponent(out Animator anim))
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
        :base(trigger)
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
        if (trigger is TrgCollision)
        {
            TrgCollision triggerCol = trigger as TrgCollision;
            if (triggerCol.target != null)
            {
                target.moveDirection = VEasyCalculator.GetDirection(triggerCol.target, target);
            }
        }
        else
        {
            target.moveDirection += 180f;
        }

        GameManager.gm.StartCoroutine(DecelerationProcess(trigger));
    }

    private IEnumerator DecelerationProcess(Trigger trigger)
    {
        List<Movable> moves = target.GetOperableList<Movable>();

        if (moves != null)
            foreach (var move in moves)
                move.state.SetState(Multistat.StateType.KNOCKBACK, true);

        Actor.RotateTo originRotateTo = target.rotateTo;

        MovableStraight knockbackMove = target.gameObject.AddComponent<MovableStraight>();
        target.rotateTo = Actor.RotateTo.NONE;

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
                move.state.SetState(Multistat.StateType.KNOCKBACK, false);
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
    protected Multistat.StateType stateType;

    public ActActiveOperable(Trigger trigger, Multistat.StateType _type, bool _doActive)
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
    public ActActiveTargetOperable(Trigger trigger, Multistat.StateType _type, bool _doActive)
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

public class ActAddOperable : Action
{
    public ActAddOperable(Trigger trigger)
        : base(trigger)
    {

    }

    protected override void ActionProcess(Trigger trigger)
    {
        // TODO

    }
}

public class ActCreateActor : Action
{
    public Actor target;
    public Vector2 pos;
    public bool isMovingActor;
    public float direction;
    public float speed;

    public ActCreateActor(Trigger trigger, Actor _target, Vector2 _pos)
        : base(trigger)
    {
        target = _target;
        pos = _pos;

        isMovingActor = false;
    }

    public ActCreateActor(Trigger trigger, Actor _target, Vector2 _pos, float _direction, float _speed)
        : base(trigger)
    {
        target = _target;
        pos = _pos;

        isMovingActor = true;

        direction = _direction;
        speed = _speed;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        Actor actor = GameObject.Instantiate(target);
        actor.transform.position = pos;

        if (isMovingActor)
        {
            Movable move = actor.GetOperable<Movable>();
            move.owner.moveDirection = direction;
            move.speed = speed;
        }
    }
}

public class ActDealDamage : Action
{
    public int damage;

    public ActDealDamage(Trigger trigger, int _damage)
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

// TriggerKeyInputs 로만 활용이 가능한 액션
public class ActVectorMoveActor : Action
{
    public override void Init()
    {
        base.Init();
        moveDir = new bool[4];
    }
    public float speed;

    bool[] moveDir = new bool[4];

    Dictionary<GameManager.Direction, KeyManager.KeyCommand> dirKeyDic =
        new Dictionary<GameManager.Direction, KeyManager.KeyCommand>();
    Dictionary<GameManager.Direction, GameManager.Direction> dirRevdirDic =
        new Dictionary<GameManager.Direction, GameManager.Direction>();

    public ActVectorMoveActor(Trigger trigger, float _speed)
        : base(trigger)
    {
        speed = _speed;

        dirKeyDic[GameManager.Direction.LEFT] = KeyManager.KeyCommand.MOVE_LEFT;
        dirKeyDic[GameManager.Direction.RIGHT] = KeyManager.KeyCommand.MOVE_RIGHT;
        dirKeyDic[GameManager.Direction.UP] = KeyManager.KeyCommand.MOVE_UP;
        dirKeyDic[GameManager.Direction.DOWN] = KeyManager.KeyCommand.MOVE_DOWN;

        dirRevdirDic[GameManager.Direction.LEFT] = GameManager.Direction.RIGHT;
        dirRevdirDic[GameManager.Direction.RIGHT] = GameManager.Direction.LEFT;
        dirRevdirDic[GameManager.Direction.UP] = GameManager.Direction.DOWN;
        dirRevdirDic[GameManager.Direction.DOWN] = GameManager.Direction.UP;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        if (trigger.GetType() != typeof(TrgKeyInputs))
            return;

        TrgKeyInputs triggerKeyInputs = (TrgKeyInputs)trigger;

        UpdateMoveState(triggerKeyInputs.command, triggerKeyInputs.pressType);

        MovableVector vm = triggerKeyInputs.owner.GetOperable<Movable>() as MovableVector;

        if (vm == null)
        {
            vm = triggerKeyInputs.owner.gameObject.AddComponent<MovableVector>();
            vm.speed = speed;
        }

        vm.moveDir = moveDir;
    }

    void UpdateMoveState(KeyManager.KeyCommand command, KeyManager.KeyPressType type)
    {
        for (int d = 0; d < 4; ++d)
        {
            GameManager.Direction dir = (GameManager.Direction)d;
            if (command == dirKeyDic[dir])
            {
                if (type == KeyManager.KeyPressType.DOWN)
                {
                    moveDir[(int)dir] = true;

                    moveDir[(int)dirRevdirDic[dir]] = false;
                }
                else if (type == KeyManager.KeyPressType.PRESS)
                {
                    if (moveDir[(int)dirRevdirDic[dir]] == false)
                    {
                        moveDir[(int)dir] = true;
                    }
                }
                else if (type == KeyManager.KeyPressType.UP)
                {
                    moveDir[(int)dir] = false;
                }

                return;
            }
        }
    }
}
