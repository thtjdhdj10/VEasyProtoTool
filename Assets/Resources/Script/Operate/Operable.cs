using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Operable : MonoBehaviour
{
    [System.NonSerialized]
    public Actor _owner;

    public Multistat _state = new Multistat();

    protected static Dictionary<Type, List<Operable>> _allOperableListDic
        = new Dictionary<Type, List<Operable>>();

    public virtual void Init()
    {

    }

    protected virtual void Awake()
    {
        if(_owner == null) _owner = GetComponent<Actor>();

        Type originType = GetOperableOriginType();

        if (_allOperableListDic.TryGetValue(originType, out List<Operable> operableList))
        {
            operableList?.Add(this);
            // Note: UnityEngine.Object인 GameObject, Componenet에 대해 ?? 연산자가 동작하지 않음
            // _allOperableListDic[originType] ??= new List<Operable>() { this };
            if (_allOperableListDic[originType] == null)
                _allOperableListDic[originType] = new List<Operable>() { this };
        }
        else _allOperableListDic.Add(originType, new List<Operable>() { this });

        if (_owner._operableListDic.TryGetValue(originType, out List<Operable> ownerOperableList))
        {
            ownerOperableList?.Add(this);
            if (_owner._operableListDic[originType] == null)
                _owner._operableListDic[originType] = new List<Operable>() { this };
        }
        else _owner._operableListDic.Add(originType, new List<Operable>() { this });

        _state.updateDelegate += HandleUpdateState;
    }

    protected virtual void OnDestroy()
    {
        Type type = GetOperableOriginType();

        _owner._operableListDic[type].Remove(this);
        _allOperableListDic[type].Remove(this);
    }

    //

    public Type GetOperableOriginType()
    {
        Type ret = this.GetType();

        if (ret.IsSubclassOf(typeof(Operable)))
        {
            while (ret.BaseType != typeof(Operable))
            {
                ret = ret.BaseType;
            }
        }

        return ret;
    }

    //

    public static bool TryGetOperableList<T>(out List<T> operableList) where T : Operable
    {
        if (_allOperableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            operableList = operables.Select(x => x as T).ToList();
            return true;
        }

        operableList = null;
        return false;
    }

    public static List<T> GetOperableList<T>() where T : Operable
    {
        if (_allOperableListDic.TryGetValue(typeof(T), out List<Operable> operables))
        {
            return operables.Select(x => x as T).ToList();
        }
        else return null;
    }

    //

    protected virtual void HandleUpdateState(bool _state)
    {
        Init();
    }
}
