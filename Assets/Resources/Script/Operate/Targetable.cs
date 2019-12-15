using UnityEngine;
using System.Collections.Generic;

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
        List<Unit> targetList = Unit.unitList;

        TargetingByForce(targetList);

        TargetingByRange(targetList, range);

        target = TargetingByDistance(targetList);
    }

    // 적만 대상
    private void TargetingByForce(List<Unit> targetList)
    {
        foreach (var target in targetList)
        {
            if (target.force == owner.force)
            {
                targetList.Remove(target);
            }
        }
    }

    // 사정거리 내의 것만 대상
    private void TargetingByRange(List<Unit> targetList, float range)
    {
        if (isRangeless) return;

        foreach (var target in targetList)
        {
            float sqrDistance = VEasyCalculator.GetSqrDistance(owner, target);

            if (sqrDistance > range * range)
            {
                targetList.Remove(target);
            }
        }
    }

    // 가장 가까운거 리턴
    private Unit TargetingByDistance(List<Unit> targetList)
    {
        Unit ret = null;
        float minDistance = float.MaxValue;

        foreach (var target in targetList)
        {
            float distance = VEasyCalculator.GetSqrDistance(owner, target);

            if (distance < minDistance)
            {
                minDistance = distance;
                ret = target;
            }
        }

        return ret;
    }
}
