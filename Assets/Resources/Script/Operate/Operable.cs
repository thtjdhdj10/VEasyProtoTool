using UnityEngine;
using System.Collections.Generic;

public abstract class Operable : MonoBehaviour
{
    [System.NonSerialized]
    public Unit owner;

    public Multistat state = new Multistat();

    public static Dictionary<System.Type, List<Operable>> allOperableListDic
        = new Dictionary<System.Type, List<Operable>>();

    public virtual void Init()
    {

    }

    protected virtual void Awake()
    {
        if(owner == null) owner = GetComponent<Unit>();

        System.Type operableType = this.GetType();
        if (operableType.BaseType != typeof(Operable))
            operableType = operableType.BaseType;
        // TODO base의 base 타입도 지원되게 수정

        if (owner.operableListDic.ContainsKey(operableType) == false)
            owner.operableListDic.Add(operableType, new List<Operable>());

        owner.operableListDic[operableType].Add(this);

        if (allOperableListDic.ContainsKey(operableType) == false)
            allOperableListDic.Add(operableType, new List<Operable>());

        allOperableListDic[operableType].Add(this);

        state.updateDelegate += HandleUpdateState;
    }

    protected virtual void OnDestroy()
    {
        System.Type operableType = this.GetType();
        if (operableType.BaseType != typeof(Operable))
            operableType = operableType.BaseType;

        owner.operableListDic[operableType].Remove(this);
        allOperableListDic[operableType].Remove(this);
    }

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
