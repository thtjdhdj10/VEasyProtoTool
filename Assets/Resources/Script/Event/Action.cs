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

public class ActionSetConditionBool : Action
{
    public ConditionBool target;
    public bool state;

    public ActionSetConditionBool(Trigger trigger, ConditionBool _target, bool _state)
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

public class ActionDirectionToMouse : Action
{
    public Unit target;
    
    public ActionDirectionToMouse(Trigger trigger, Unit _target)
        : base(trigger)
    {
        target = _target;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        Vector2 mouseWorldPos = VEasyCalculator.ScreenToWorldPos(Input.mousePosition);
        target.direction = VEasyCalculator.GetDirection(target.transform.position, mouseWorldPos);
    }
}

public class ActionDirectionToTarget : Action
{
    public Unit from;
    public Unit to;

    public ActionDirectionToTarget(Trigger trigger, Unit _from, Unit _to)
        :base(trigger)
    {
        from = _from;
        to = _to;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        from.direction = VEasyCalculator.GetDirection(from, to);
    }
}

public class ActionSetAnimatorSpeed : Action
{
    public GameObject target;
    public float speed;

    public ActionSetAnimatorSpeed(Trigger trigger, GameObject _target, float _speed)
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

public class ActionSetComponent<T> : Action where T : Component
{
    public GameObject target;
    public bool isAdd;

    public ActionSetComponent(Trigger trigger, GameObject _target, bool _isAdd)
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

public class ActionSetController : Action
{
    public GameObject target;
    public RuntimeAnimatorController controller;

    public ActionSetController(Trigger trigger, GameObject _target, RuntimeAnimatorController _controller)
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

public class ActionSetSprite : Action
{
    public SpriteRenderer spriteRenderer;
    public Sprite sprite;

    public ActionSetSprite(Trigger trigger, SpriteRenderer _spriteRenderer, Sprite _sprite)
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

public class ActionSetSpriteColor : Action
{
    public SpriteRenderer sprite;
    public Color color;

    public ActionSetSpriteColor(Trigger trigger, SpriteRenderer _sprite, Color _color)
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

public class ActionInitTrigger : Action
{
    public Trigger target;

    public ActionInitTrigger(Trigger trigger, Trigger _target)
        : base(trigger)
    {
        target = _target;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        target.Init();
    }
}

public class ActionSetSpeed : Action
{
    public float speed;

    public ActionSetSpeed(Trigger trigger, float _speed)
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

public class ActionPrintLog : Action
{
    public string log;

    public ActionPrintLog(Trigger trigger, string _log)
        : base(trigger)
    {
        log = _log;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        Debug.Log(log);
    }
}

public class ActionKnockback : Action
{
    public Unit target;
    public float speed;
    public float deceleration; // 초당 속도 감소

    public ActionKnockback(Trigger trigger, Unit _target, float _speed, float _deceleration)
        : base(trigger)
    {
        target = _target;
        speed = _speed;
        deceleration = _deceleration;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        if (trigger is TriggerCollision)
        {
            TriggerCollision triggerCol = trigger as TriggerCollision;
            if (triggerCol.target != null)
            {
                target.direction = VEasyCalculator.GetDirection(
                    triggerCol.target, target);
            }
        }
        else
        {
            target.direction += 180f;
        }

        GameManager.gm.StartCoroutine(DecelerationProcess(trigger));
    }

    private IEnumerator DecelerationProcess(Trigger trigger)
    {
        List<Operable> moves = target.GetOperables<Movable>();

        if (moves != null)
            foreach (var move in moves)
                move.state.SetState(Multistat.StateType.KNOCKBACK, true);

        MovableStraight knockbackMove = target.gameObject.AddComponent<MovableStraight>();
        knockbackMove.isRotate = false;

        float currentSpeed = speed;

        while (currentSpeed > 0f && deceleration > 0f)
        {
            knockbackMove.speed = currentSpeed;
            currentSpeed -= deceleration * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        knockbackMove.speed = 0f;

        GameObject.Destroy(knockbackMove);

        if (moves != null)
            foreach (var move in moves)
                move.state.SetState(Multistat.StateType.KNOCKBACK, false);
    }
}

public class ActionActivatePattern : Action
{
    public Pattern pattern;

    public ActionActivatePattern(Trigger trigger, Pattern _pattern)
        : base(trigger)
    {
        pattern = _pattern;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        pattern.Activate();
    }
}

public class ActionActiveOperable<T> : Action where T : Operable
{
    protected bool doActive;
    protected Multistat.StateType stateType;

    public ActionActiveOperable(Trigger trigger, Multistat.StateType _type, bool _doActive)
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

public class ActionActiveTargetOperable<T> : ActionActiveOperable<T> where T : Operable
{
    public ActionActiveTargetOperable(Trigger trigger, Multistat.StateType _type, bool _doActive)
        : base(trigger, _type, _doActive)
    {

    }

    protected override void ActionProcess(Trigger trigger)
    {
        if (trigger == null) return;

        Unit target = trigger.owner;
        if (trigger is TriggerCollision)
            target = (trigger as TriggerCollision).target;

        if (target == null) return;

        Operable operable = target.GetOperable<T>();
        operable.state.SetState(stateType, doActive);
    }
}

public class ActionAddOperable : Action
{
    public ActionAddOperable(Trigger trigger)
        : base(trigger)
    {

    }

    protected override void ActionProcess(Trigger trigger)
    {
        // TODO

    }
}

public class ActionCreateUnit : Action
{
    public Unit target;
    public Vector2 pos;
    public bool isMovingUnit;
    public float direction;
    public float speed;

    public ActionCreateUnit(Trigger trigger, Unit _target, Vector2 _pos)
        : base(trigger)
    {
        target = _target;
        pos = _pos;

        isMovingUnit = false;
    }

    public ActionCreateUnit(Trigger trigger, Unit _target, Vector2 _pos, float _direction, float _speed)
        : base(trigger)
    {
        target = _target;
        pos = _pos;

        isMovingUnit = true;

        direction = _direction;
        speed = _speed;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        Unit unit = GameObject.Instantiate(target);
        unit.transform.position = pos;

        if (isMovingUnit)
        {
            Movable move = unit.GetOperable<Movable>();
            unit.direction = direction;
            move.speed = speed;
        }
    }
}

public class ActionDealDamage : Action
{
    public int damage;

    public ActionDealDamage(Trigger trigger, int _damage)
        : base(trigger)
    {
        damage = _damage;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        if (trigger is TriggerCollision == false) return;

        TriggerCollision trgCol = trigger as TriggerCollision;

        if (trgCol.target == null) return;
        if (trgCol.target.unitStatus == null) return;

        if (trgCol.target.TryGetOperable(out ShieldOwnable shield))
        {
            if (shield.enableShield) shield.ShieldBreak();
            else trgCol.target.unitStatus.CurrentHp -= damage;
        }
    }
}

public class ActionGetDamage : Action
{
    public int damage;

    public ActionGetDamage(Trigger trigger, int _damage)
        : base(trigger)
    {
        damage = _damage;
    }

    protected override void ActionProcess(Trigger trigger)
    {
        if (trigger is TriggerCollision == false) return;

        if (trigger.owner == null) return;
        if (trigger.owner.unitStatus == null) return;

        if (trigger.owner.TryGetOperable(out ShieldOwnable shield))
        {
            if(shield.enableShield) shield.ShieldBreak();
            else trigger.owner.unitStatus.CurrentHp -= damage;
        }
    }
}

public class ActionDestroyUnit : Action
{
    public Unit target;

    public ActionDestroyUnit(Trigger trigger, Unit _target)
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
public class ActionVectorMoveUnit : Action
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

    public ActionVectorMoveUnit(Trigger trigger, float _speed)
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
        if (trigger.GetType() != typeof(TriggerKeyInputs))
            return;

        TriggerKeyInputs triggerKeyInputs = (TriggerKeyInputs)trigger;

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
