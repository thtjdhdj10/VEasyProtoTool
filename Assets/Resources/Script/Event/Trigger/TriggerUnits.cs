using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if(unitTypeTriggerListDic.ContainsKey(_unitType))
        {
            foreach(var key in unitTypeTriggerListDic.Keys)
            {
                if(_unitType.IsSubclassOf(key))
                {
                    foreach(var trigger in unitTypeTriggerListDic[key])
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
