using UnityEngine;
using System.Collections.Generic;

// TODO: 동맹관계를 따져서 공격하도록 수정

public class Shootable : Operable
{
    public bool active;

    public bool isRangeless;
    public float range;

    public int damage;

    public float attackDelay;
    private float remainAttackDelay;
    public bool loadOnDeactive; // 비활성 중 장전 여부

    public bool fireToTarget;
    public Unit target;
    public float fireDirection;

    // TODO
    // range 관련 구현 수정

    void Start()
    {
        if (fireToTarget) target = GetTarget();
    }

    void FixedUpdate()
    {
        if (AttackDelayCheck() == true)
        {
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
        List<KeyValuePair<Targetable, float>> targetList;

        targetList = SearchTargetInRange(range);
        if(ChioceTargetByDistance(targetList) != null)
        {
            return ChioceTargetByDistance(targetList).owner;
        }

        return null;
    }

    // 사정거리 내의 것을 대상으로.
    public virtual List<KeyValuePair<Targetable, float>> SearchTargetInRange(float range)
    {
        List<KeyValuePair<Targetable, float>> ret = new List<KeyValuePair<Targetable, float>>();

        for (int i = 0; i < allOperableListDic[typeof(Targetable)].Count; ++i)
        {
            Targetable target = allOperableListDic[typeof(Targetable)][i] as Targetable;

            float sqrDistance = VEasyCalculator.GetSqrDistance(owner, target.owner);

            if (sqrDistance <= range * range)
            {
                ret.Add(new KeyValuePair<Targetable, float>(target, sqrDistance));
            }
        }

        return ret;
    }

    // 가장 가까운 것을 target으로 지정.
    public virtual Targetable ChioceTargetByDistance(List<KeyValuePair<Targetable, float>> targetList)
    {
        Targetable ret = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < targetList.Count; ++i)
        {
            Targetable target = targetList[i].Key;
            float distance = targetList[i].Value;
            VEasyCalculator.GetSqrDistance(owner, target.owner);
            if (distance < minDistance)
            {
                minDistance = distance;
                ret = target;
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
