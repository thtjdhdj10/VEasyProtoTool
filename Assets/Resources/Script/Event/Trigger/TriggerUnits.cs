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
public class TriggerUnits : Trigger
{
    public TriggerUnits(Unit _owner)
        : base(_owner) { }

    static Dictionary<System.Type, TriggerUnits> unitTriggerDic
        = new Dictionary<System.Type, TriggerUnits>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(Unit unit, TriggerType type)
    {
        if (unitTriggerDic.ContainsKey(unit.GetType()) == true)
            if (type == unitTriggerDic[unit.GetType()].type)
                unitTriggerDic[unit.GetType()].ActivateTrigger();
    }

    //
    public TriggerType type;

    public Unit target;

    public enum TriggerType
    {
        NONE = 0,
        CREATE_UNIT,
        INIT_UNIT,
        DESTROY_UNIT,
    }

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork,
        TriggerType _type, Unit _target)
    {
        Init(_isDisposableTrigger, _isDiposableAction, _isWork);

        type = _type;
        target = _target;
    }

    public override void RefreshTriggerAttribute()
    {
        System.Type prevKey;
        if (VEasyCalculator.TryGetKey(unitTriggerDic, this, out prevKey) == false)
            return;

        System.Type targetType = target.GetType();

        VEasyCalculator.ChangeKey(unitTriggerDic, prevKey, targetType);
    }

    void Start()
    {
        if(type == TriggerType.NONE)
        {
            Debug.LogWarning(this.ToString() + ": type is not set.");
            return;
        }

        if(target == null)
        {
            Debug.LogWarning(this.ToString() + ": target unit is not set.");
            return;
        }

        unitTriggerDic.Add(target.GetType(), this);
    }

}
