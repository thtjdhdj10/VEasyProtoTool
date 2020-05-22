using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    public CndEnable target;
    public bool state;

    public ActSetConditionBool(Trigger trigger, CndEnable _target, bool _state)
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
        Vector2 mouseWorldPos = VEasyCalc.ScreenToWorldPos(Input.mousePosition);
        target.targetDir = VEasyCalc.GetDirection(target.transform.position, mouseWorldPos);
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
        from.targetDir = VEasyCalc.GetDirection(from, to);
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
                target.moveDir = VEasyCalc.GetDirection(triggerCol.target, target);
            }
        }
        else
        {
            target.moveDir += 180f;
        }

        GameManager.gm.StartCoroutine(DecelerationProcess(trigger));
    }

    // 기존 Movable 일시 중지, 새로운 Movable 생성해서 넉백 이동
    private IEnumerator DecelerationProcess(Trigger trigger)
    {
        List<Movable> moves = target.GetOperableList<Movable>();

        if (moves != null)
            foreach (var move in moves)
                move.state.SetState(Multistat.EStateType.KNOCKBACK, true);

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
                move.state.SetState(Multistat.EStateType.KNOCKBACK, false);
    }
}

public class ActBlockMove : Action
{
    public float escapeSpeed = 1f;

    public ActBlockMove(Trigger trigger, float _escapeSpeed)
        :base(trigger)
    {
        escapeSpeed = _escapeSpeed;
    }

    // TODO 최적화 필요
    protected override void ActionProcess(Trigger trigger)
    {
        float targetDir = 0f;

        if (trigger is TrgCollision)
        {
            Actor target = (trigger as TrgCollision).target;
            targetDir = VEasyCalc.GetDirection(trigger.owner, target);
        }
        else if (trigger is TrgBoundaryTouch)
        {
            targetDir = (trigger as TrgBoundaryTouch).bounceTo;
        }
        else if (trigger is TrgBoundaryOut)
        {
            targetDir = (trigger as TrgBoundaryOut).bounceTo;
        }
        else return;

        Vector2 moveVector = VEasyCalc.GetRotatedPosition(trigger.owner.moveDir, 1f);
        Vector2 targetVector = VEasyCalc.GetRotatedPosition(targetDir, 1f);

        float inner = VEasyCalc.Inner(moveVector, targetVector);

        Vector3 escapeVector = VEasyCalc.GetRotatedPosition(
            targetDir + 180f, inner * escapeSpeed * Time.fixedDeltaTime);

        trigger.owner.transform.position += escapeVector;
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
    protected Multistat.EStateType stateType;

    public ActActiveOperable(Trigger trigger, Multistat.EStateType _type, bool _doActive)
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
        Multistat.EStateType _type, bool _doActive)
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
            move.owner.moveDir = direction;
            move.speed = speed;
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

    Dictionary<Const.EDirection, KeyManager.EKeyCommand> dirKeyDic =
        new Dictionary<Const.EDirection, KeyManager.EKeyCommand>();
    Dictionary<Const.EDirection, Const.EDirection> dirRevdirDic =
        new Dictionary<Const.EDirection, Const.EDirection>();

    public ActVectorMoveActor(Trigger trigger, float _speed)
        : base(trigger)
    {
        speed = _speed;

        dirKeyDic[Const.EDirection.LEFT] = KeyManager.EKeyCommand.MOVE_LEFT;
        dirKeyDic[Const.EDirection.RIGHT] = KeyManager.EKeyCommand.MOVE_RIGHT;
        dirKeyDic[Const.EDirection.UP] = KeyManager.EKeyCommand.MOVE_UP;
        dirKeyDic[Const.EDirection.DOWN] = KeyManager.EKeyCommand.MOVE_DOWN;

        dirRevdirDic[Const.EDirection.LEFT] = Const.EDirection.RIGHT;
        dirRevdirDic[Const.EDirection.RIGHT] = Const.EDirection.LEFT;
        dirRevdirDic[Const.EDirection.UP] = Const.EDirection.DOWN;
        dirRevdirDic[Const.EDirection.DOWN] = Const.EDirection.UP;
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

    void UpdateMoveState(KeyManager.EKeyCommand command, KeyManager.EKeyPressType type)
    {
        for (int d = 0; d < 4; ++d)
        {
            Const.EDirection dir = (Const.EDirection)d;
            if (command == dirKeyDic[dir])
            {
                if (type == KeyManager.EKeyPressType.DOWN)
                {
                    moveDir[(int)dir] = true;

                    moveDir[(int)dirRevdirDic[dir]] = false;
                }
                else if (type == KeyManager.EKeyPressType.PRESS)
                {
                    if (moveDir[(int)dirRevdirDic[dir]] == false)
                    {
                        moveDir[(int)dir] = true;
                    }
                }
                else if (type == KeyManager.EKeyPressType.UP)
                {
                    moveDir[(int)dir] = false;
                }

                return;
            }
        }
    }
}
