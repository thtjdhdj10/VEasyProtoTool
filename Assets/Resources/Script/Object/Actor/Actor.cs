using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MyObject
{
    public Force force = Force.NONE;

    public bool willDestroy = false; // 바로 삭제하면 충돌처리할때 문제됨

    public Dictionary<System.Type, List<Operable>> operableListDic
        = new Dictionary<System.Type, List<Operable>>();

    public List<Trigger> triggerList = new List<Trigger>();

    public float MoveDirection
    {
        get
        {
            return GetOperable<Movable>().direction;
        }
        set
        {
            if (TryGetOperable(out Movable move))
                move.direction = value;
        }
    }
    public float TargetDirection
    {
        get
        {
            return GetOperable<Targetable>().direction;
        }
        set
        {
            if (TryGetOperable(out Targetable move))
                move.direction = value;
        }
    }

    //

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

    public delegate void OnActorAddedDelegate(Actor actor);
    public static OnActorAddedDelegate onActorAddedDelegate = new OnActorAddedDelegate(OnActorAdded);
    public static void OnActorAdded(Actor actor) { }

    //

    protected virtual void Awake()
    {
        awakeDelegate();
    }

    protected virtual void Start()
    {
        onActorAddedDelegate(this);
        Init();
    }

    protected virtual void OnDestroy()
    {
        onDestroyDelegate();
    }

    protected virtual void FixedUpdate()
    {
        fixedUpdateDelegate();

        if (willDestroy) Destroy(gameObject);
    }

    public virtual void Init()
    {
        initDelegate();
    }

    //

    public bool TryGetOperable<T>(out T operable) where T : Operable
    {
        if (operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            operable = operables[0] as T;
            return true;
        }

        operable = null;
        return false;
    }

    public T GetOperable<T>() where T : Operable
    {
        if (operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            return operables[0] as T;
        }

        return null;
    }

    public bool TryGetOperableList<T>(out List<T> operableList) where T : Operable
    {
        if (operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            List<T> retList = new List<T>();
            foreach (var o in operables)
            {
                retList.Add(o as T);
            }
            operableList = retList;
            return true;
        }

        operableList = null;
        return false;
    }

    public List<T> GetOperableList<T>() where T : Operable
    {
        if (operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            List<T> retList = new List<T>();
            foreach (var o in operables)
            {
                retList.Add(o as T);
            }
            return retList;
        }
        else return null;
    }

    public void SetOperablesState(bool state)
    {
        foreach (var type in operableListDic.Keys)
        {
            foreach (var o in operableListDic[type])
            {
                o.state.SetStateForce(state);
            }
        }
    }

}
