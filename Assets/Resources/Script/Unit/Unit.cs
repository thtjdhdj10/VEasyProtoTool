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
        public float currentHp;
        public float currentMp;
    }

    public class AttackAbility
    {
        public float minAttackRange; // default : 0
        public float attackStartRange; // default : basicAttackRange
        public float basicAttackRange;

        public float searchRange; // default : sight

        public float attackDelay;
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

        public float moveSpeed;
    }

    public class LogicalPosition
    {
        public float x;
        public float y;
        public float height;
    }

    public Ability originalAbility = new Ability();
    public Ability currentAbility = new Ability();

    public AttackAbility originalAttackAbility = new AttackAbility();
    public AttackAbility currentAttackAbility = new AttackAbility();

    public ExtraAbility originalExtraAbility = new ExtraAbility();
    public ExtraAbility currentExtraAbility = new ExtraAbility();

    public LogicalPosition logicalPosition = new LogicalPosition();

    public void Init()
    {
        currentAbility = originalAbility;
        currentAttackAbility = originalAttackAbility;
        currentExtraAbility = originalExtraAbility;
    }

    public void CompleteInit()
    {
        // model object 이용
    }

    //public class TempValue
    //{
    //    public float importance;
    //}

    //public TempValue tempValue;

    //

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        logicalPosition.x = transform.position.x;
        logicalPosition.y = transform.position.z;

        if(Input.GetKeyDown(KeyCode.E))
        {
            SearchEnemy();
        }
    }

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
        if (unitOwner != Player.PlayerType.ME)
            return;

        List<GameObject> targetableUnitList = new List<GameObject>();

        TargetableUnit[] targetArr = FindObjectsOfType<TargetableUnit>();
        for (int i = 0; i < targetArr.Length; ++i)
        {
            targetableUnitList.Add(targetArr[i].gameObject);
        }

        //            VEasyPoolerManager.RefObjectListAtLayer(LayerManager.StringToMask("Targetable"));

        List<Unit> inRangeRectUnits = new List<Unit>();
        List<TargetableUnit> targetList = new List<TargetableUnit>();

        for (int i = 0; i < targetableUnitList.Count; ++i)
        {
            var target = targetableUnitList[i].GetComponent<TargetableUnit>();

            if (target == null)
                continue;

            var unit = targetableUnitList[i].GetComponent<Unit>();

            if (unit == null)
                continue;

            if (Player.TypeToRelations(unit.unitOwner) == Player.Relations.ENEMY)
            {
                // search 에 따라 대충 계산한 사각형 내에 있는 유닛들을 후보로 둔다.
                if (VEasyCalculator.CheckMyRect(logicalPosition, unit.logicalPosition, currentAttackAbility.searchRange))
                {
                    inRangeRectUnits.Add(unit);
                    targetList.Add(target);
                }
            }
        }

        if (inRangeRectUnits.Count == 0)
            return;

        // 후보 내의 유닛을 중요도 순으로 정렬
        inRangeRectUnits.Sort(SortByImportance);

        {
            // 가장 중요한 유닛이 공격 범위 안에 있다면, target 으로 선택
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
