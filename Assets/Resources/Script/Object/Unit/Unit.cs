using UnityEngine;
using System.Collections.Generic;

/* Unit 구조 설명
 * 
 * Unit 은 GameObject 의 Operable 들을 멤버로 가짐. (operableListDic)
 * ( GetComponent 사용을 줄이기 위하여 )
 * 각 Operable 은 static 으로 동종의 Operable List 를 가짐.
 * ( Operable 순회 작업을 쉽게 하기 위하여 )
 */

public class Unit : MyObject
{
    public static List<Unit> unitList = new List<Unit>();

    [System.NonSerialized]
    public UnitStatus unitStatus;

    public bool unitActive = true;

    public float direction;

    public Dictionary<System.Type, List<Operable>> operableListDic
        = new Dictionary<System.Type, List<Operable>>();

    public List<Trigger> triggerList = new List<Trigger>();

    public Force force = Force.NONE;

    public enum Force
    {
        NONE = 0,
        A, // Player
        B, // Enemy
    }

    public enum Relation
    {
        NONE = 0,
        ALLY,
        ENEMY,

        ALLY_OR_ENEMY,
    }

    public static Relation GetRelation(Force a, Force b)
    {
        if (a == Force.NONE ||
            b == Force.NONE)
            return Relation.NONE;

        if (a == b) return Relation.ALLY;
        else return Relation.ENEMY;
    }

    //

    protected virtual void Awake()
    {
        Init();

        TriggerUnit.UnitEventReceive(this, TriggerUnit.TriggerType.CREATE_UNIT);
        TriggerUnits.UnitEventReceive(this.GetType(), TriggerUnits.TriggerType.CREATE_UNIT);
    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDestroy()
    {
        TriggerUnit.UnitEventReceive(this, TriggerUnit.TriggerType.DESTROY_UNIT);
        TriggerUnits.UnitEventReceive(this.GetType(), TriggerUnits.TriggerType.DESTROY_UNIT);
    }

    protected virtual void OnEnable()
    {
        unitList.Add(this);
    }

    protected virtual void OnDisable()
    {
        unitList.Remove(this);
    }

    protected virtual void FixedUpdate()
    {
        for (int i = 0; i < triggerList.Count; ++i)
        {
            if (triggerList[i] is TriggerFrame)
                (triggerList[i] as TriggerFrame).HandleFixedUpdate();
            if (triggerList[i] is TriggerTimer)
                (triggerList[i] as TriggerTimer).HandleFixedUpdate();
        }
    }

    public virtual void Init()
    {
        TriggerUnit.UnitEventReceive(this, TriggerUnit.TriggerType.INIT_UNIT);
        TriggerUnits.UnitEventReceive(this.GetType(), TriggerUnits.TriggerType.INIT_UNIT);
    }

    public virtual void InitSprite()
    {

    }
    
    //

    public void SetOperablesState(bool state)
    {
        foreach(var type in operableListDic.Keys)
        {
            foreach(var o in operableListDic[type])
            {
                o.active.SetStateForce(state);
            }
        }
    }

    public T GetOperable<T>() where T : Operable
    {
        if (operableListDic.ContainsKey(typeof(T)) == false) return null;

        if (operableListDic[typeof(T)].Count > 0)
            return operableListDic[typeof(T)][0] as T;

        return null;
    }

    public List<T> GetOperables<T>() where T : Operable
    {
        if (operableListDic.ContainsKey(typeof(T)) == false) return null;

        return operableListDic[typeof(T)] as List<T>;
    }
}
