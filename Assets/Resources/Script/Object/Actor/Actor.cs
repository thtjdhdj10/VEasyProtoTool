using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actor : MyObject
{
    public Force _force = Force.NONE;

    public bool _willDestroy = false; // 바로 삭제하면 충돌처리할때 문제됨

    public Dictionary<Type, List<Operable>> _operableListDic
        = new Dictionary<Type, List<Operable>>();

    public List<Trigger> triggerList = new List<Trigger>();

    public float _moveDirection;
    public float _targetDirection;

    public RotateTo _rotateTo = RotateTo.TARGET;

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

        NEUTRAL,
    }

    public enum RotateTo
    {
        NONE = 0,
        TARGET,
        MOVE,
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

    public static void DoNothing() { }
    public delegate void AwakeDelegate();
    public AwakeDelegate awakeDelegate = new AwakeDelegate(DoNothing);

    public delegate void OnDestroyDelegate();
    public OnDestroyDelegate onDestroyDelegate = new OnDestroyDelegate(DoNothing);

    public delegate void InitDelegate();
    public InitDelegate initDelegate = new InitDelegate(DoNothing);

    public delegate void FixedUpdateDelegate();
    public FixedUpdateDelegate fixedUpdateDelegate = new FixedUpdateDelegate(DoNothing);

    public static void DoNothing(Actor actor) { }
    public delegate void OnActorAddedDelegate(Actor actor);
    public static OnActorAddedDelegate onActorAddedDelegate = new OnActorAddedDelegate(DoNothing);

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

        SetSpriteAngle();

        if (_willDestroy) Destroy(gameObject);
    }

    public virtual void Init()
    {
        initDelegate();
    }

    //

    public bool TryGetOperable<T>(out T operable) where T : Operable
    {
        if (_operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            operable = operables[0] as T;
            return true;
        }

        operable = null;
        return false;
    }

    public T GetOperable<T>() where T : Operable
    {
        if (_operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            return operables[0] as T;
        }

        return null;
    }

    public bool TryGetOperableList<T>(out List<T> operableList) where T : Operable
    {
        if (_operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            operableList = new List<T>(operables.Select(x => x as T));
            return true;
        }

        operableList = null;
        return false;
    }

    public List<T> GetOperableList<T>() where T : Operable
    {
        if (_operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            return new List<T>(operables.Select(x => x as T));
        }
        
        return null;
    }

    public void SetOperablesState(bool state)
    {
        _operableListDic.Values.ToList().ForEach(
            ol => ol.ForEach(o => o.state.SetStateForce(state)));
    }

    //

    public virtual void SetSpriteAngle()
    {
        Vector3 rot = transform.eulerAngles;
        switch (_rotateTo)
        {
            case RotateTo.TARGET:
                rot.z = _targetDirection;
                break;
            case RotateTo.MOVE:
                rot.z = _moveDirection;
                break;
        }
        transform.eulerAngles = rot;
    }
}
