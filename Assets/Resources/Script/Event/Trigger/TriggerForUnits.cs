using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO
// 현재는 어떤 Unit(의 하위) 클래스를 가졌는지로 구분.
// Prefab 의 이름을 기준으로도 구분할 수 있도록.
// (문제는 Prefab 으로부터 복사된 오브젝트의 이름에서 원본 이름을 얻어야 한다는 것.
// VEasyPooler 의 ObjectsStatus 를 사용하면 가능.
// 하지만 둘이 커플링되는 문제

// 특정 종류의 유닛들을 대상으로 하는 Trigger
public class TriggerForUnits : Trigger
{
    static Dictionary<System.Type, TriggerForUnits> unitTriggerBindingDic
        = new Dictionary<System.Type, TriggerForUnits>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(Unit unit, TriggerForUnits.Type type)
    {
        if (unitTriggerBindingDic.ContainsKey(unit.GetType()) == true)
        {
            if (type == unitTriggerBindingDic[unit.GetType()].type)
            {
                unitTriggerBindingDic[unit.GetType()].ActivateTrigger();
            }
        }
    }

    //
    public Type type;

    public Unit target;

    public enum Type
    {
        NONE = 0,
        CREATE_UNIT,
        INIT_UNIT,
        DESTROY_UNIT,
//        DELETE_UNIT,
    }

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork,
        Type _type, Unit _target)
    {
        Init(_isDisposableTrigger, _isDiposableAction, _isWork);

        type = _type;
        target = _target;
    }

    public override void RefreshTriggerAttribute()
    {
        System.Type prevKey;
        if (VEasyCalculator.TryGetKey<System.Type, TriggerForUnits>
            (unitTriggerBindingDic, this, out prevKey) == false)
        {
            return;
        }

        System.Type targetType = target.GetType();

        VEasyCalculator.ChangeKey<System.Type, TriggerForUnits>(
            unitTriggerBindingDic, prevKey, targetType);
    }

    void Start()
    {
        if(type == Type.NONE)
        {
            CustomLog.CompleteLogWarning(this.name + ": type is not set.");
            return;
        }

        if(target == null)
        {
            CustomLog.CompleteLogWarning(this.name + ": target unit is not set.");
            return;
        }

        unitTriggerBindingDic.Add(target.GetType(), this);
    }

}
