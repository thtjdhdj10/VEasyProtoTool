using UnityEngine;
using System.Collections.Generic;

public class Operable : MonoBehaviour
{
    [System.NonSerialized]
    public Unit owner;

    public static Dictionary<System.Type, List<Operable>> allOperableListDic
        = new Dictionary<System.Type, List<Operable>>();

    protected virtual void Awake()
    {
        owner = GetComponent<Unit>();

        if (owner.operableListDic[this.GetType()] == null)
            owner.operableListDic[this.GetType()] = new List<Operable>();

        owner.operableListDic[this.GetType()].Add(this);

        if (allOperableListDic[this.GetType()] == null)
            allOperableListDic[this.GetType()] = new List<Operable>();

        allOperableListDic[this.GetType()].Add(this);
    }

    protected virtual void OnDestroy()
    {
        owner.operableListDic[this.GetType()].Remove(this);
        allOperableListDic[this.GetType()].Remove(this);
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
