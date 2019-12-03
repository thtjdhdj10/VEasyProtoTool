using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 특정한 유닛의 생성/파괴/초기화 시 Activate
public class TriggerUnit : Trigger
{
    public TriggerUnit(Unit _owner, TriggerType _type)
        : base(_owner)
    {
        type = _type;

        unitTriggerDic.Add(owner, this);
    }

    ~TriggerUnit()
    {
        unitTriggerDic.Remove(owner);
    }

    static Dictionary<Unit, TriggerUnit> unitTriggerDic
        = new Dictionary<Unit, TriggerUnit>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(Unit unit, TriggerType type)
    {
        if (unitTriggerDic.ContainsKey(unit) == true)
            if (type == unitTriggerDic[unit].type)
                unitTriggerDic[unit].ActivateTrigger();
    }

    public TriggerType type;

    public enum TriggerType
    {
        NONE = 0,
        CREATE_UNIT,
        INIT_UNIT,
        DESTROY_UNIT,
    }
}
