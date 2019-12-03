using UnityEngine;
using System.Collections.Generic;

/* Unit 구조 설명
 * 
 * Unit 은 GameObject 의 Operable 들을 멤버로 가짐.
 * ( GetComponent 사용을 줄이기 위하여 )
 * 각 Operable 은 static 으로 동종의 Operable List 를 가짐.
 * ( Operable 순회 작업을 쉽게 하기 위하여 )
 */

public class Unit : MyObject
{
    public UnitStatus unitStatus;

    public bool unitActive = true;

    public List<Operable> operateList = new List<Operable>();
    public List<Trigger> triggerList = new List<Trigger>();

    public static List<Unit> unitList = new List<Unit>();

    public enum ColliderType // Collidable 의 속성으로 이동
    {
        NONE = 0,
        CIRCLE,
        RECT,
    }

    public ColliderType colType;

    public float colCircle;

    public Vector2 colRect;

    public Force force = Force.NONE;

    public enum Force
    {
        NONE = 0,
        A,
        B,
        C,
    }

    public enum Relation
    {
        NONE = 0,
        ALLY,
        ENEMY,
        ALL,
    }

    public static Relation GetRelation(Force a, Force b)
    {
        if (a == Force.NONE ||
            b == Force.NONE)
            return Relation.NONE;

        if (a == b)
            return Relation.ALLY;
        else return Relation.ENEMY;
    }

    //

    protected virtual void Awake()
    {
        TriggerUnits.UnitEventReceive(this, TriggerUnits.TriggerType.CREATE_UNIT);

        Init();
    }

    protected virtual void Start()
    {
        operateList = new List<Operable>(GetComponents<Operable>());
    }

    protected virtual void OnDestroy()
    {
        TriggerUnits.UnitEventReceive(this, TriggerUnits.TriggerType.DESTROY_UNIT);
    }

    protected virtual void OnEnable()
    {
        unitList.Add(this);
    }

    protected virtual void OnDisable()
    {
        unitList.Remove(this);
    }

    public virtual void Init()
    {
        TriggerUnits.UnitEventReceive(this, TriggerUnits.TriggerType.INIT_UNIT);
    }

    public virtual void InitSprite()
    {

    }
    
    //

    public Operable GetOperable(System.Type type)
    {
        for (int i = 0; i < operateList.Count; ++i)
            if(operateList[i].GetType() == type)
                return operateList[i];

        return null;
    }
}
