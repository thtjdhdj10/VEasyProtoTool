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
    public bool unitActive = true;

    public static List<Unit> unitList = new List<Unit>();

    public List<Operable> operateList = new List<Operable>();

    public enum ColliderType
    {
        NONE = 0,
        CIRCLE,
        RECT,
    }

    public ColliderType colType;

    public float colCircle;

    public Vector2 colRect;

    public List<SpriteRenderer> spriteList = new List<SpriteRenderer>();

    public Force force = Force.NONE;

    public enum Force
    {
        NONE = 0,
        RED,
        BLUE,
        GREEN,
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
        TriggerForUnits.UnitEventReceive(this, TriggerForUnits.Type.CREATE_UNIT);
    }

    protected virtual void Start()
    {
        operateList = new List<Operable>(GetComponents<Operable>());
    }

    protected virtual void OnDestroy()
    {
        TriggerForUnits.UnitEventReceive(this, TriggerForUnits.Type.DESTROY_UNIT);
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
        TriggerForUnits.UnitEventReceive(this, TriggerForUnits.Type.INIT_UNIT);
    }

    public virtual void InitSprite()
    {

    }
    
    //

    public Operable GetOperable(System.Type type)
    {
        for (int i = 0; i < operateList.Count; ++i)
        {
            if(operateList[i].GetType() == type)
            {
                return operateList[i];
            }
        }

        return null;
    }
}
