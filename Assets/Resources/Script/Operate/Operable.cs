using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Operable : MonoBehaviour
{
    [System.NonSerialized]
    public Actor owner;

    public Multistat state = new Multistat();

    // key: operable의 직속 하위클래스. GetOperableOriginType() 로 확인
    protected static Dictionary<Type, List<Operable>> _allOperableListDic
        = new Dictionary<Type, List<Operable>>();

    public virtual void Init()
    {

    }

    protected virtual void Awake()
    {
        if(owner == null) owner = GetComponent<Actor>();

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

        if (owner.operableListDic.TryGetValue(originType, out List<Operable> ownerOperableList))
        {
            ownerOperableList?.Add(this);

            if (owner.operableListDic[originType] == null)
                owner.operableListDic[originType] = new List<Operable>() { this };
        }
        else owner.operableListDic.Add(originType, new List<Operable>() { this });

        state.updateDelegate += HandleUpdateState;
    }

    protected virtual void OnDestroy()
    {
        Type type = GetOperableOriginType();

        owner.operableListDic[type].Remove(this);
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

    }
}
