using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            foreach(var trigger in unitTriggerListDic[target])
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
