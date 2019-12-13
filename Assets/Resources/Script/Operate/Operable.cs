using UnityEngine;
using System.Collections.Generic;

public abstract class Operable : MonoBehaviour
{
    [System.NonSerialized]
    public Actor owner;

    public Multistat state = new Multistat();

    protected static Dictionary<System.Type, List<Operable>> allOperableListDic
        = new Dictionary<System.Type, List<Operable>>();

    public virtual void Init()
    {

    }

    protected virtual void Awake()
    {
        if(owner == null) owner = GetComponent<Actor>();

        System.Type type = GetOperableOriginType();

        if (allOperableListDic.TryGetValue(type, out List<Operable> operableList))
        {
            if (operableList != null) operableList.Add(this);
            else allOperableListDic[type] = new List<Operable>() { this };
        }
        else allOperableListDic.Add(type, new List<Operable>() { this });

        if (owner.operableListDic.TryGetValue(type, out List<Operable> ownerOperableList))
        {
            if (ownerOperableList != null) ownerOperableList.Add(this);
            else owner.operableListDic[type] = new List<Operable>() { this };
        }
        else owner.operableListDic.Add(type, new List<Operable>() { this });

        state.updateDelegate += HandleUpdateState;
    }

    protected virtual void OnDestroy()
    {
        System.Type type = GetOperableOriginType();

        owner.operableListDic[type].Remove(this);
        allOperableListDic[type].Remove(this);
    }

    //

    public System.Type GetOperableOriginType()
    {
        System.Type ret = this.GetType();

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
        if (allOperableListDic.TryGetValue(typeof(T), out List<Operable> operables))
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

    public static List<T> GetOperableList<T>() where T : Operable
    {
        if (allOperableListDic.TryGetValue(typeof(T), out List<Operable> operables))
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

    //

    protected virtual void HandleUpdateState(bool _state)
    {
        Init();
    }

    //protected bool ConfirmExistence()
    //{
    //    if (owner == null ||
    //        owner.isActiveAndEnabled == false)
    //    {
    //        return false;
    //    }

    //    return true;
    //}
}
