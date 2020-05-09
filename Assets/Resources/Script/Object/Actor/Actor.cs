using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actor : MyObject
{
    public Force force = Force.NONE;

    public bool willDestroy = false; // 바로 삭제하면 충돌처리할때 문제됨

    public Dictionary<Type, List<Operable>> operableListDic
        = new Dictionary<Type, List<Operable>>();

    public List<Trigger> triggerList = new List<Trigger>();

    public float moveDirection;
    public float targetDirection;

    public RotateTo rotateTo = RotateTo.TARGET;

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

    public static void EmptyMethod() { }
    public delegate void AwakeDel();
    public AwakeDel awakeDel = new AwakeDel(EmptyMethod);

    public delegate void OnDestroyDel();
    public OnDestroyDel onDestroyDel = new OnDestroyDel(EmptyMethod);

    public delegate void InitDel();
    public InitDel initDel = new InitDel(EmptyMethod);

    public delegate void FixedUpdateDel();
    public FixedUpdateDel fixedUpdateDel = new FixedUpdateDel(EmptyMethod);

    public static void EmptyMethod(Actor actor) { }
    public delegate void OnActorAddedDel(Actor actor);
    public static OnActorAddedDel onActorAddedDel = new OnActorAddedDel(EmptyMethod);

    //

    protected virtual void Awake()
    {
        awakeDel();
    }

    protected virtual void Start()
    {
        onActorAddedDel(this);
        Init();
    }

    protected virtual void OnDestroy()
    {
        onDestroyDel();
    }

    protected virtual void FixedUpdate()
    {
        fixedUpdateDel();

        SetSpriteAngle();

        if (willDestroy) Destroy(gameObject);
    }

    public virtual void Init()
    {
        initDel();
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
            operableList = operables.Select(x => x as T).ToList();
            return true;
        }

        operableList = null;
        return false;
    }

    public List<T> GetOperableList<T>() where T : Operable
    {
        if (operableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            return operables.Select(x => x as T).ToList();
        }
        
        return null;
    }

    public void SetOperablesState(bool state)
    {
        operableListDic.Values.ToList().ForEach(
            ol => ol.ForEach(o => o.state.SetStateForce(state)));
    }

    //

    public virtual void SetSpriteAngle()
    {
        Vector3 rot = transform.eulerAngles;
        switch (rotateTo)
        {
            case RotateTo.TARGET:
                rot.z = targetDirection;
                break;
            case RotateTo.MOVE:
                rot.z = moveDirection;
                break;
        }
        transform.eulerAngles = rot;
    }
}
