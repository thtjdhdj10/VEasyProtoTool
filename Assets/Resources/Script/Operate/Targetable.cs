using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Targetable : Operable
{
    public Unit target;

    public bool enableTargetUpdate;

    public bool isRangeless = true;
    public float range;


    // TODO target update delay 기능 추가

    //

    private void FixedUpdate()
    {
        if (state == false) return;

        if (enableTargetUpdate) TargetUpdate();

        if (target != null)
            owner.targetDirection = VEasyCalculator.GetDirection(owner, target);
    }

    //

    protected virtual void TargetUpdate()
    {
        List<Unit> units = Unit.unitList;

        units = (from target in units
                     where target.force != owner.force
                     where isRangeless || VEasyCalculator.GetSqrDistance(owner, target) < range * range
                     select target).ToList();

        target = units.FirstOrDefault(x => VEasyCalculator.GetSqrDistance(owner, x) ==
            units.Min(y => VEasyCalculator.GetSqrDistance(owner, y)));
        // TODO 해당 코드가 자동으로 최적화되는지 확인
    }
}
