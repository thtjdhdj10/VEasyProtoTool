using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerCollision : Trigger
{
    public TriggerCollision(Unit _owner, System.Type _targetType)
        : base(_owner)
    {
        targetType = _targetType;
    }

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(Unit hitter, Unit _target)
    {
        for (int i = 0; i < hitter.triggerList.Count;++i)
        {
            if (hitter.triggerList[i] is TriggerCollision == false) continue;
            TriggerCollision trgCol = hitter.triggerList[i] as TriggerCollision;

            // TODO 이부분 부하 있을 수 있음.
            if (_target.GetType() == trgCol.targetType ||
                _target.GetType().IsSubclassOf(trgCol.targetType))
            {
                trgCol.target = _target;
                trgCol.ActivateTrigger();
            }
        }
    }

    public System.Type targetType;

    public Unit target;

    public override void Init()
    {
        base.Init();

        target = null;
    }
}
