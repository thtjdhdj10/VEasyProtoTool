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
    public bool willDestroy = false;

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

    public delegate void AwakeDelegate();
    public AwakeDelegate awakeDelegate = new AwakeDelegate(AwakeCallback);
    public static void AwakeCallback() { }

    public delegate void OnDestroyDelegate();
    public OnDestroyDelegate onDestroyDelegate = new OnDestroyDelegate(OnDestroyCallback);
    public static void OnDestroyCallback() { }

    public delegate void InitDelegate();
    public InitDelegate initDelegate = new InitDelegate(InitCallback);
    public static void InitCallback() { }

    public delegate void FixedUpdateDelegate();
    public FixedUpdateDelegate fixedUpdateDelegate = new FixedUpdateDelegate(fixedUpdateCallback);
    public static void fixedUpdateCallback() { }

    public delegate void OnUnitAddedDelegate(Unit unit);
    public static OnUnitAddedDelegate onUnitAddedDelegate = new OnUnitAddedDelegate(OnUnitAdded);
    public static void OnUnitAdded(Unit unit) { }

    //

    protected virtual void Awake()
    {
        unitList.Add(this);
        onUnitAddedDelegate(this);
        awakeDelegate();
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void OnDestroy()
    {
        onDestroyDelegate();
        unitList.Remove(this);
    }

    protected virtual void FixedUpdate()
    {
        fixedUpdateDelegate();
    }

    protected virtual void LateUpdate()
    {
        if (willDestroy) Destroy(gameObject);
    }

    public virtual void Init()
    {
        initDelegate();
    }
    
    //

    public void SetOperablesState(bool state)
    {
        foreach(var type in operableListDic.Keys)
        {
            foreach(var o in operableListDic[type])
            {
                o.state.SetStateForce(state);
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

    public List<Operable> GetOperables<T>() where T : Operable
    {
        if (operableListDic.ContainsKey(typeof(T)) == false) return null;

        return operableListDic[typeof(T)];
    }
}
