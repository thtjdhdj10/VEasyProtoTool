using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CmdType = System.Collections.Generic.KeyValuePair<KeyManager.KeyCommand,
    KeyManager.KeyPressType>;

using CmdTypeObject = System.Collections.Generic.KeyValuePair<
    System.Collections.Generic.KeyValuePair<KeyManager.KeyCommand,
    KeyManager.KeyPressType>, MyObject>;

public abstract class Trigger
{
    // 검사하기 쉬운 조건을 먼저 Add 할 것.
    public List<Condition> conditionList = new List<Condition>();
    public List<Action> actionList = new List<Action>();

    public Unit owner;

    // Option
    public bool isDisposableTrigger = false; // Trigger 가 작동할 때까지 존재.
    public bool isDisposableAction = false; // Action 이 한 번 실행될 때까지 존재.

    public Trigger(Unit _owner)
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
        for (int i = 0; i < actionList.Count; ++i)
            actionList[i].Init();
        for (int i = 0; i < conditionList.Count; ++i)
            conditionList[i].Init();
    }

    public virtual void ActivateTrigger()
    {
        for (int i = 0; i < conditionList.Count; ++i)
            if (conditionList[i].CheckCondition() == false)
                return;

        for (int i = 0; i < actionList.Count;++i )
        {
            actionList[i].Activate(this);

            if (isDisposableAction == true)
                owner.triggerList.Remove(this);
        }

        if (isDisposableTrigger == true)
            owner.triggerList.Remove(this);
    }

}

public class TriggerCollision : Trigger
{
    public TriggerCollision(Unit _owner, params System.Type[] _targetTypes)
        : base(_owner)
    {
        targetTypes = _targetTypes;
    }

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(Unit hitter, Unit _target)
    {
        for (int i = 0; i < hitter.triggerList.Count; ++i)
        {
            if (hitter.triggerList[i] is TriggerCollision == false) continue;
            TriggerCollision trgCol = hitter.triggerList[i] as TriggerCollision;

            // TODO 이부분 부하 있을 수 있음
            foreach(var targetType in trgCol.targetTypes)
            {
                if (_target.GetType() == targetType ||
                    _target.GetType().IsSubclassOf(targetType))
                {
                    trgCol.target = _target;
                    trgCol.ActivateTrigger();
                    return;
                }
            }
        }
    }

    public System.Type[] targetTypes;

    public Unit target;

    public override void Init()
    {
        base.Init();
        target = null;
    }
}

public class TriggerFrame : Trigger
{
    public TriggerFrame(Unit _owner, int _passCount)
        : base(_owner)
    {
        passCount = _passCount;

        _owner.fixedUpdateDelegate += HandleFixedUpdate;
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

// TriggerForKeyInput 은 정해진 키 입력에 대해서만 Activate() 를 호출.
public class TriggerKeyInput : Trigger
{
    public TriggerKeyInput(Unit _owner, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
        : base(_owner)
    {
        command = _command;
        pressType = _pressType;

        Controllable control = owner.GetOperable<Controllable>();
        if (control != null)
        {
            control.keyInputDelegate += HandleKeyInput;
        }
    }

    ~TriggerKeyInput()
    {
        Controllable control = owner.GetOperable<Controllable>();
        if (control != null)
        {
            control.keyInputDelegate -= HandleKeyInput;
        }
    }

    private void HandleKeyInput(KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        if (command == _command && pressType == _pressType)
            ActivateTrigger();
    }

    public KeyManager.KeyCommand command;
    public KeyManager.KeyPressType pressType;
}

// TriggerKeyInput 과 달리 정해진 Object 에의 모든 키 입력에서 Activate() 호출
// Action 내에서 명령어를 선별해서 사용.
// 하나의 Action 에 복수의 명령어가 입력될 수 있을 때 사용할 것.
public class TriggerKeyInputs : Trigger
{
    public TriggerKeyInputs(Unit _owner)
        : base(_owner)
    {
        Controllable control = owner.GetOperable<Controllable>();
        if (control != null)
        {
            control.keyInputDelegate += HandleKeyInput;
        }
    }

    ~TriggerKeyInputs()
    {
        Controllable control = owner.GetOperable<Controllable>();
        if (control != null)
        {
            control.keyInputDelegate -= HandleKeyInput;
        }
    }

    public override void Init()
    {
        base.Init();
        command = KeyManager.KeyCommand.NONE;
        pressType = KeyManager.KeyPressType.NONE;
    }

    private void HandleKeyInput(KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        command = _command;
        pressType = _pressType;
        ActivateTrigger();
    }

    public KeyManager.KeyCommand command;
    public KeyManager.KeyPressType pressType;
}

public class TriggerTimer : Trigger
{
    public TriggerTimer(Unit _owner, float _delay, bool _isActivateOnStart)
        : base(_owner)
    {
        delay = _delay;
        isActivateOnStart = _isActivateOnStart;

        _owner.fixedUpdateDelegate += HandleFixedUpdate;

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
public class TriggerUnit : Trigger
{
    public TriggerUnit(Unit _owner, Unit _target, TriggerType _type)
        : base(_owner)
    {
        target = _target;
        type = _type;

        switch (type)
        {
            case TriggerType.AWAKE:
                target.awakeDelegate += ActivateTrigger;
                break;
            case TriggerType.INIT:
                target.initDelegate += ActivateTrigger;
                break;
            case TriggerType.ON_DESTROY:
                target.onDestroyDelegate += ActivateTrigger;
                break;
        }
    }

    ~TriggerUnit()
    {
        switch (type)
        {
            case TriggerType.AWAKE:
                target.awakeDelegate -= ActivateTrigger;
                break;
            case TriggerType.INIT:
                target.initDelegate -= ActivateTrigger;
                break;
            case TriggerType.ON_DESTROY:
                target.onDestroyDelegate -= ActivateTrigger;
                break;
        }
    }

    private Unit target;
    private TriggerType type;

    public enum TriggerType
    {
        AWAKE,
        INIT,
        ON_DESTROY,
    }
}

// 특정 type의 유닛 생성/파괴/초기화 시 동작
public class TriggerUnits : Trigger
{
    public TriggerUnits(Unit _owner, System.Type _unitType, TriggerType _type)
        : base(_owner)
    {
        unitType = _unitType;
        type = _type;

        foreach (var unit in Unit.unitList)
        {
            LinkEventHandle(unit, true);
        }

        Unit.onUnitAddedDelegate += HandleAddedUnit;
    }

    ~TriggerUnits()
    {
        foreach(var unit in Unit.unitList)
        {
            LinkEventHandle(unit, false);
        }
    }

    private void LinkEventHandle(Unit unit, bool isAdd)
    {
        switch (type)
        {
            case TriggerType.AWAKE:
                if(isAdd) unit.awakeDelegate += ActivateTrigger;
                else unit.awakeDelegate -= ActivateTrigger;
                break;
            case TriggerType.INIT:
                if(isAdd) unit.initDelegate += ActivateTrigger;
                else unit.initDelegate -= ActivateTrigger;
                break;
            case TriggerType.ON_DESTROY:
                if(isAdd) unit.onDestroyDelegate += ActivateTrigger;
                else unit.onDestroyDelegate -= ActivateTrigger;
                break;
        }
    }

    private void HandleAddedUnit(Unit unit)
    {
        if (unit.GetType().IsSubclassOf(unitType))
        {
            LinkEventHandle(unit, true);
        }
    }

    private System.Type unitType;
    private TriggerType type;

    public enum TriggerType
    {
        AWAKE,
        INIT,
        ON_DESTROY,
    }
}
