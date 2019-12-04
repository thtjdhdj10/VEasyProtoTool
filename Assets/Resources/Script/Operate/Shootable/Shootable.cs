using UnityEngine;
using System.Collections.Generic;

// TODO
// movable 에서도 target 쓸거고, 다른 패턴에서도 쓸거같으니까
// target 찾는 알고리즘 여기 말고 딴데로 빼기
public class Shootable : Operable
{
    public bool active;

    public bool isRangeless;
    public float range;

    public int damage;

    public float attackDelay;
    public float remainAttackDelay;
    public bool loadOnDeactive; // 비활성 중 장전 여부

    public bool fireToTarget;
    public Unit target;
    public bool targetingEachFire; // 공격 마다 타겟찾음
    public float fireDirection;

    void FixedUpdate()
    {
        if (AttackDelayCheck() == true)
        {
            if(fireToTarget)
            {
                if (target == null) target = GetTarget();
                else if (targetingEachFire == true) target = GetTarget();
            }

            Shoot();
        }
    }

    protected virtual void Shoot()
    {

    }

    bool AttackDelayCheck()
    {
        if (active == false)
        {
            if(remainAttackDelay > 0f &&
                loadOnDeactive == true)
                remainAttackDelay -= Time.fixedDeltaTime;

            return false;
        }

        if (remainAttackDelay > 0f)
        {
            remainAttackDelay -= Time.fixedDeltaTime;
        }
        else
        {
            remainAttackDelay = attackDelay;
            return true;
        }

        return false;
    }

    public Unit GetTarget()
    {
        List<KeyValuePair<Targetable, float>> targetList
            = new List<KeyValuePair<Targetable, float>>();

        foreach(var targetable in allOperableListDic[typeof(Targetable)])
        {
            targetList.Add(new KeyValuePair<Targetable, float>(
                targetable as Targetable, 0f));
        }

        TargetingByForce(targetList);

        TargetingByRange(targetList, range);

        return TargetingByDistance(targetList).owner;
    }

    // 적만 대상
    public virtual void TargetingByForce(List<KeyValuePair<Targetable,float>> targetList)
    {
        foreach(var target in targetList)
        {
            if (target.Key.owner.force == owner.force)
            {
                targetList.Remove(target);
            }
        }
    }

    // 사정거리 내의 것만 대상
    public virtual void TargetingByRange(List<KeyValuePair<Targetable, float>> targetList, float range)
    {
        if (isRangeless) return;

        foreach (var target in targetList)
        {
            float sqrDistance = VEasyCalculator.GetSqrDistance(owner, target.Key.owner);

            if (sqrDistance > range * range)
            {
                targetList.Remove(target);
            }
        }
    }

    // 가장 가까운거 리턴
    public virtual Targetable TargetingByDistance(List<KeyValuePair<Targetable, float>> targetList)
    {
        Targetable ret = null;
        float minDistance = float.MaxValue;

        foreach(var target in targetList)
        {
            VEasyCalculator.GetSqrDistance(owner, target.Key.owner);
            if (target.Value < minDistance)
            {
                minDistance = target.Value;
                ret = target.Key;
            }
        }
        
        return ret;
    }

    // TODO
    // '사정거리를 벗어난 정도' / '나의 초당 이동거리' 만큼 importance 를 낮게 책정.
    // importance 가 양수인 적 만을 대상으로 삼음.
    //public virtual List<Targetable> SearchImportantTarget(float range)
    //{
    //    List<Targetable> ret = new List<Targetable>();

    //    for (int i = 0; i < Targetable.targetableList.Count; ++i)
    //    {
    //        Targetable target = Targetable.targetableList[i];

    //        float sqrDistance = VEasyCalculator.GetSqrDistance(owner, target.owner);

    //        if (sqrDistance <= VEasyCalculator.Square(
    //            range + target.importance))
    //        {
    //            ret.Add(target);
    //        }
    //    }

    //    return ret;
    //}

    // search range 내의 가장 importance 가 높은 적을 target Unit 으로 한다.
    // 단, attack start range 를 벗어난 적은 그 차이만큼 importance 를 낮게 책정한다.
    //void SearchEnemy()
    //{
    //    List<GameObject> TargetableList = new List<GameObject>();

    //    Targetable[] targetArr = FindObjectsOfType<Targetable>();
    //    for (int i = 0; i < targetArr.Length; ++i)
    //    {
    //        TargetableList.Add(targetArr[i].gameObject);
    //    }

    //    //            VEasyPoolerManager.RefObjectListAtLayer(LayerManager.StringToMask("Targetable"));

    //    List<Unit> inRangeRectUnits = new List<Unit>();
    //    List<Targetable> targetList = new List<Targetable>();

    //    for (int i = 0; i < TargetableList.Count; ++i)
    //    {
    //        var target = TargetableList[i].GetComponent<Targetable>();

    //        if (target == null)
    //            continue;

    //        var unit = TargetableList[i].GetComponent<Unit>();

    //        if (unit == null)
    //            continue;

    //        if (Player.TypeToRelations(unit.unitOwner) == Player.Relations.ENEMY)
    //        {
    //            // search 에 따라 대충 계산한 사각형 내에 있는 유닛들을 후보로 둔다.
    //            if (VEasyCalculator.CheckMyRect(logicalPosition, unit.logicalPosition, currentAttackAbility.searchRange))
    //            {
    //                inRangeRectUnits.Add(unit);
    //                targetList.Add(target);
    //            }
    //        }
    //    }

    //    if (inRangeRectUnits.Count == 0)
    //        return;

    //    // 후보 내의 유닛을 중요도 순으로 정렬
    //    inRangeRectUnits.Sort(SortByImportance);

    //    {
    //        // 가장 중요한 유닛이 공격 범위 안에 있다면, target 으로 선택
    //        float distanceSquare = VEasyCalculator.CalcDistanceSquare2D(logicalPosition, inRangeRectUnits[0].logicalPosition);

    //        float searchRangeSquare = currentAttackAbility.searchRange * currentAttackAbility.searchRange;
    //        if (distanceSquare < searchRangeSquare)
    //        {
    //            targetUnit = targetList[0];
    //            return;
    //        }
    //    }

    //    float mostImportant = 0f;

    //    for (int i = 1; i < inRangeRectUnits.Count; ++i)
    //    {
    //        float distance = VEasyCalculator.CalcDistance2D(logicalPosition, inRangeRectUnits[i].logicalPosition);

    //        if (distance < currentAttackAbility.attackStartRange)
    //        {
    //            if (inRangeRectUnits[i].currentExtraAbility.importance > mostImportant)
    //            {
    //                targetUnit = targetList[i];
    //            }
    //        }
    //        else if (distance < currentAttackAbility.searchRange)
    //        {
    //            float deltaRange = currentAttackAbility.attackStartRange - distance;

    //            if (inRangeRectUnits[i].currentExtraAbility.importance + deltaRange > mostImportant)
    //            {
    //                targetUnit = targetList[i];
    //            }
    //        }
    //        else
    //        {
    //            // searchRange 밖의 유닛들은 공격 대상에서 제외
    //        }
    //    }

    //}

}
