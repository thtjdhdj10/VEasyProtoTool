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
    }

    public int passCount = 0;
    int counter;

    public override void Init()
    {
        base.Init();

        counter = 0;
    }

    public void HandleFixedUpdate()
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

        CmdType ct = new CmdType(command, pressType);
        CmdTypeObject cto = new CmdTypeObject(ct, owner);
        unitTriggerBindingDic.Add(cto, this);
    }

    ~TriggerKeyInput()
    {
        CmdType ct = new CmdType(command, pressType);
        CmdTypeObject cto = new CmdTypeObject(ct, owner);
        unitTriggerBindingDic.Remove(cto);
    }

    static Dictionary<CmdTypeObject, TriggerKeyInput> unitTriggerBindingDic
        = new Dictionary<CmdTypeObject, TriggerKeyInput>();

    // Controlable 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(
        MyObject obj, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        CmdType cp = new CmdType(_command, _pressType);

        CmdTypeObject cpo = new CmdTypeObject(cp, obj);

        if (unitTriggerBindingDic.ContainsKey(cpo) == true)
            unitTriggerBindingDic[cpo].ActivateTrigger();
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
        unitTriggerDic.Add(owner, this);
    }

    ~TriggerKeyInputs()
    {
        unitTriggerDic.Remove(owner);
    }

    static Dictionary<MyObject, TriggerKeyInputs> unitTriggerDic
        = new Dictionary<MyObject, TriggerKeyInputs>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(
        MyObject obj, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        if (unitTriggerDic.ContainsKey(obj) == true)
        {
            unitTriggerDic[obj].command = _command;
            unitTriggerDic[obj].pressType = _pressType;
            unitTriggerDic[obj].ActivateTrigger();
        }
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

    public void HandleFixedUpdate()
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

        if (unitTriggerListDic.ContainsKey(target))
            unitTriggerListDic[target].Add(this);
        else unitTriggerListDic.Add(target,
            new List<TriggerUnit>(new TriggerUnit[] { this }));
    }

    ~TriggerUnit()
    {
        unitTriggerListDic.Remove(target);
    }

    static Dictionary<Unit, List<TriggerUnit>> unitTriggerListDic
        = new Dictionary<Unit, List<TriggerUnit>>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(Unit target, TriggerType type)
    {
        if (unitTriggerListDic.ContainsKey(target))
        {
            foreach (var trigger in unitTriggerListDic[target])
            {
                if (trigger.type == type) trigger.ActivateTrigger();
            }
        }
    }

    private Unit target;
    private TriggerType type;

    public enum TriggerType
    {
        NONE = 0,
        CREATE_UNIT,
        INIT_UNIT,
        DESTROY_UNIT,
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

        if (unitTypeTriggerListDic.ContainsKey(unitType))
            unitTypeTriggerListDic[unitType].Add(this);
        else unitTypeTriggerListDic.Add(unitType,
            new List<TriggerUnits>(new TriggerUnits[] { this }));
    }

    ~TriggerUnits()
    {
        unitTypeTriggerListDic.Remove(unitType);
    }

    public static Dictionary<System.Type, List<TriggerUnits>> unitTypeTriggerListDic
        = new Dictionary<System.Type, List<TriggerUnits>>();

    public static void UnitEventReceive(System.Type _unitType, TriggerType _type)
    {
        // TODO 이부분 성능 구릴 수 있음
        // issubclass 부하 확인

        if (unitTypeTriggerListDic.ContainsKey(_unitType))
        {
            foreach (var key in unitTypeTriggerListDic.Keys)
            {
                if (_unitType.IsSubclassOf(key))
                {
                    foreach (var trigger in unitTypeTriggerListDic[key])
                    {
                        if (trigger.type == _type) trigger.ActivateTrigger();
                    }
                }
            }
        }

    }

    private System.Type unitType;
    private TriggerType type;

    public enum TriggerType
    {
        NONE = 0,
        CREATE_UNIT,
        INIT_UNIT,
        DESTROY_UNIT,
    }
}
