using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actor : MyObject
{
    public EForce force = EForce.NONE;

    public bool willDestroy = false; // 바로 삭제하면 충돌처리할때 문제됨

    public Dictionary<Type, List<Operable>> operableListDic =
        new Dictionary<Type, List<Operable>>();

    public List<Trigger> triggerList = new List<Trigger>();

    public float moveDir;
    public float targetDir;

    public ERotateTo rotateTo = ERotateTo.TARGET;

    //

    public enum EForce
    {
        NONE = 0,
        A, // Player
        B, // Enemy
    }

    public enum ERelation
    {
        NONE = 0,
        ALLY,
        ENEMY,

        NEUTRAL,
    }

    public enum ERotateTo
    {
        NONE = 0,
        TARGET,
        MOVE,
    }

    public static ERelation GetRelation(EForce a, EForce b)
    {
        if (a == EForce.NONE ||
            b == EForce.NONE)
            return ERelation.NONE;

        if (a == b) return ERelation.ALLY;
        else return ERelation.ENEMY;
    }

    //

    public static void EmptyMethod() { }
    public delegate void AwakeDel();
    public AwakeDel awakeDlg = new AwakeDel(EmptyMethod);

    public delegate void OnDestroyDel();
    public OnDestroyDel onDestroyDlg = new OnDestroyDel(EmptyMethod);

    public delegate void InitDel();
    public InitDel initDlg = new InitDel(EmptyMethod);

    public delegate void FixedUpdateDel();
    public FixedUpdateDel fixedUpdateDlg = new FixedUpdateDel(EmptyMethod);

    public static void EmptyMethod(Actor actor) { }
    public delegate void OnActorAddedDel(Actor actor);
    public static OnActorAddedDel onActorAddedDlg = new OnActorAddedDel(EmptyMethod);

    //

    protected virtual void Awake()
    {
        awakeDlg();
    }

    protected virtual void Start()
    {
        onActorAddedDlg(this);
        Init();
    }

    protected virtual void OnDestroy()
    {
        onDestroyDlg();
    }

    protected virtual void FixedUpdate()
    {
        fixedUpdateDlg();

        SetSpriteAngle();

        if (willDestroy) Destroy(gameObject);
    }

    public virtual void Init()
    {
        initDlg();
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
            case ERotateTo.TARGET:
                rot.z = targetDir;
                break;
            case ERotateTo.MOVE:
                rot.z = moveDir;
                break;
        }
        transform.eulerAngles = rot;
    }
}
