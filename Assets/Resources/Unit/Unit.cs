using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{ 

    public Player.PlayerType unitOwner = Player.PlayerType.ME;

    public TargetableUnit targetUnit;
    Vector3 toPosition;

    public enum AttackMode
    {
        NONE = 0,
        ATTACK_GROUND,
        ATTACK_TARGET,
        HOLDING,
        MOVE_POSITION,
    }

    public AttackMode attackMode;

    //

    public class Ability
    {
        public float attackPoint;
        public float defencePoint;

        public float maxHp;
        public float currentHp;
                 
        public float maxMp;
        public float currentMp;
    }

    public class AttackAbility
    {
        public float minAttackRange; // default : 0
        public float attackStartRange; // default : basicAttackRange
        public float basicAttackRange;

        public float searchRange; // default : sight

        public float attackDelay;
        float currentAttackDelay;
    }

    public class ExtraAbility
    {
        // property of attack/heal
        public float importance;

        public float sight;

        // priority of calling SearchEnemy() per sec
        public float reflexes;

        public float logicalSize;
        public float visualSize;
    }

    public class LogicalPosition
    {
        public float x;
        public float y;
        public float height;
    }

    public Ability originalAbility;
    public Ability currentAbility;

    public AttackAbility originalAttackAbility;
    public AttackAbility currentAttackAbility;

    public ExtraAbility originalExtraAbility;
    public ExtraAbility currentExtraAbility;

    public LogicalPosition logicalPosition;

    //public class TempValue
    //{
    //    public float importance;
    //}

    //public TempValue tempValue;

    //

    void AutomaticAttack()
    {

    }

    void CompellingAttack()
    {

    }

    void HoldingAttack()
    {

    }

    void MoveToPosition()
    {

    }

    // search range 내의 가장 importance 가 높은 적을 target Unit 으로 한다.
    // 단, attack start range 를 벗어난 적은 그 차이만큼 importance 를 낮게 책정한다.
    void SearchEnemy()
    {
        List<GameObject> targetableUnitList =
            VEasyPoolerManager.RefObjectListAtLayer(LayerManager.StringToMask("Targetable"));

        // GetComponent 하기 싫어서 두개의 List 사용
        List<Unit> inRangeRectUnits = new List<Unit>();
        List<TargetableUnit> targetList = new List<TargetableUnit>();

        for (int i = 0; i < targetableUnitList.Count; ++i)
        {
            var target = targetableUnitList[i].GetComponent<TargetableUnit>();

            if (target == null)
                continue;

            var targetUnit = targetableUnitList[i].GetComponent<Unit>();

            if (targetUnit == null)
                continue;

            if (Player.TypeToRelations(targetUnit.unitOwner) == Player.Relations.ENEMY)
            {
                if (VEasyCalculator.CheckMyRect(logicalPosition, targetUnit.logicalPosition, currentAttackAbility.searchRange))
                {
                    inRangeRectUnits.Add(targetUnit);
                    targetList.Add(target);
                }
            }
        }
        
        if (inRangeRectUnits == null)
            return;
        
        inRangeRectUnits.Sort(SortByImportance);

        {
            float distanceSquare = VEasyCalculator.CalcDistanceSquare2D(logicalPosition, inRangeRectUnits[0].logicalPosition);

            float searchRangeSquare = currentAttackAbility.searchRange * currentAttackAbility.searchRange;
            if (distanceSquare < searchRangeSquare)
            {
                targetUnit = targetList[0];
                return;
            }
        }
        
        float mostImportant = 0f;

        for (int i = 1; i < inRangeRectUnits.Count; ++i)
        {
            float distance = VEasyCalculator.CalcDistance2D(logicalPosition, inRangeRectUnits[i].logicalPosition);

            if (distance < currentAttackAbility.attackStartRange)
            {
                if(inRangeRectUnits[i].currentExtraAbility.importance > mostImportant)
                {
                    targetUnit = targetList[i];
                }
            }
            else if(distance < currentAttackAbility.searchRange)
            {
                float deltaRange = currentAttackAbility.attackStartRange - distance;

                if (inRangeRectUnits[i].currentExtraAbility.importance + deltaRange > mostImportant)
                {
                    targetUnit = targetList[i];
                }
            }
            else
            {
                // searchRange 밖의 유닛들은 공격 대상에서 제외
            }
        }
        
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

	}



    private int SortByImportance(Unit o1, Unit o2)
    {
        if (o1.currentExtraAbility.importance > o2.currentExtraAbility.importance)
        {
            return 1;
        }
        else if (o1.currentExtraAbility.importance < o2.currentExtraAbility.importance)
        {
            return -1;
        }

        return 0;
    }
}
