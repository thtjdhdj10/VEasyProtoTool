//using UnityEngine;
//using System.Collections.Generic;

//public class Unit_unused : MonoBehaviour
//{
//    public Player.PlayerType unitOwner = Player.PlayerType.ME;

//    public Targetable targetUnit;
    
//    Vector3 toPosition;

//    public enum AttackMode
//    {
//        NONE = 0,
//        ATTACK_GROUND,
//        ATTACK_TARGET,
//        HOLDING,
//        MOVE_POSITION,
//    }

//    public AttackMode attackMode;

//    //

//    public class Ability
//    {
//        public float attackPoint;
//        public float defencePoint;
//        public float currentHp;
//        public float currentMp;
//    }

//    public class AttackAbility
//    {
//        public float minAttackRange; // default : 0
//        public float attackStartRange; // default : basicAttackRange
//        public float basicAttackRange;

//        public float searchRange; // default : sight

//        public float attackDelay;
//    }

//    public class ExtraAbility
//    {
//        // property of attack/heal
//        public float importance;

//        public float sight;

//        // priority of calling SearchEnemy() per sec
//        public float reflexes;

//        public float logicalSize;
//        public float visualSize;

//        public float moveSpeed;
//    }

//    public class LogicalPosition
//    {
//        public float x;
//        public float y;
//        public float height;
//    }

//    public Ability originalAbility = new Ability();
//    public Ability currentAbility = new Ability();

//    public AttackAbility originalAttackAbility = new AttackAbility();
//    public AttackAbility currentAttackAbility = new AttackAbility();

//    public ExtraAbility originalExtraAbility = new ExtraAbility();
//    public ExtraAbility currentExtraAbility = new ExtraAbility();

//    public LogicalPosition logicalPosition = new LogicalPosition();

//    public void Init()
//    {
//        currentAbility = originalAbility;
//        currentAttackAbility = originalAttackAbility;
//        currentExtraAbility = originalExtraAbility;
//    }

//    public void CompleteInit()
//    {
//        // model object 이용
//    }

//    //public class TempValue
//    //{
//    //    public float importance;
//    //}

//    //public TempValue tempValue;

//    //

//    void Awake()
//    {

//    }

//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        logicalPosition.x = transform.position.x;
//        logicalPosition.y = transform.position.z;

//        if(Input.GetKeyDown(KeyCode.E))
//        {
//            SearchEnemy();
//        }
//    }

//    void AutomaticAttack()
//    {

//    }

//    void CompellingAttack()
//    {

//    }

//    void HoldingAttack()
//    {

//    }

//    void MoveToPosition()
//    {

//    }
    
//    private int SortByImportance(Unit o1, Unit o2)
//    {
//        if (o1.currentExtraAbility.importance > o2.currentExtraAbility.importance)
//        {
//            return 1;
//        }
//        else if (o1.currentExtraAbility.importance < o2.currentExtraAbility.importance)
//        {
//            return -1;
//        }

//        return 0;
//    }
//}
