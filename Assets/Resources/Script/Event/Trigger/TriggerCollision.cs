using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerCollision : Trigger
{
    public TriggerCollision(Unit _owner)
        : base(_owner)
    { }

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(Unit hitter, Unit _target)
    {
        for (int i = 0; i < hitter.triggerList.Count;++i)
        {
            TriggerCollision tc = null;
            if (hitter.triggerList[i] is TriggerCollision)
            {
                tc = hitter.triggerList[i] as TriggerCollision;
            }
            else continue;

            // TODO 이부분 부하 있을 수 있음.
            if (_target.GetType().IsSubclassOf(tc.targetType))
            {
                tc.target = _target;
                tc.ActivateTrigger();
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
