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

        System.Type operableType = this.GetType();
        if (operableType.BaseType != typeof(Operable))
            operableType = operableType.BaseType;

        if (owner.operableListDic.ContainsKey(operableType) == false)
            owner.operableListDic.Add(operableType, new List<Operable>());

        owner.operableListDic[operableType].Add(this);

        if (allOperableListDic.ContainsKey(operableType) == false)
            allOperableListDic.Add(operableType, new List<Operable>());

        allOperableListDic[operableType].Add(this);
    }

    protected virtual void OnDestroy()
    {
        System.Type operableType = this.GetType();
        if (operableType.BaseType != typeof(Operable))
            operableType = operableType.BaseType;

        owner.operableListDic[operableType].Remove(this);
        allOperableListDic[operableType].Remove(this);
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
