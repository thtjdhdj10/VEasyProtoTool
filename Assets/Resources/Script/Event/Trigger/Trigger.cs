using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Trigger
{
    // 검사하기 쉬운 조건을 먼저 Add 할 것.
    public List<Condition> conditionList = new List<Condition>();
    public List<Action> actionList = new List<Action>();

    public Unit owner;

    // Option
    public bool isDisposableTrigger = false; // Trigger 가 작동할 때까지 존재.
    public bool isDisposableAction = false; // Action 이 한 번 실행될 때까지 존재.

    public Trigger(Unit _owner)
    {
        owner = _owner;
        owner.triggerList.Add(this);
    }

    ~Trigger()
    {
        owner.triggerList.Remove(this);
    }

    public virtual void Init()
    {
        for (int i = 0; i < actionList.Count; ++i)
            actionList[i].Init();
        for (int i = 0; i < conditionList.Count; ++i)
            conditionList[i].Init();
    }

    public virtual void ActivateTrigger()
    {
        for (int i = 0; i < conditionList.Count; ++i)
            if (conditionList[i].CheckCondition() == false)
                return;

        for (int i = 0; i < actionList.Count;++i )
        {
            actionList[i].Activate(this);

            if (isDisposableAction == true)
                owner.triggerList.Remove(this);
        }

        if (isDisposableTrigger == true)
            owner.triggerList.Remove(this);
    }

}