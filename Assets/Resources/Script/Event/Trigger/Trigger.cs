using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{
    // 검사하기 쉬운 조건을 먼저 Add 할 것.
    public List<Condition> conditionList;
    public List<Action> actionList;

    // Trigger 가 작동할 때까지 존재.
    public bool isDisposableTrigger;

    // Action 이 한 번 실행될 때까지 존재.
    public bool isDiposableAction;

    // 트리거 동작 여부
    public bool isWork;

    void Awake()
    {
        conditionList = new List<Condition>();
        actionList = new List<Action>();
    }

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork)
    {
        isDisposableTrigger = _isDisposableTrigger;
        isDiposableAction = _isDiposableAction;
        isWork = _isWork;
    }

    public virtual void RefreshTriggerAttribute()
    {

    }

    public virtual void ActivateTrigger()
    {
        if (isWork == false)
            return;

        for (int i = 0; i < conditionList.Count;++i )
        {
            if(conditionList[i].CheckCondition() == false)
            {
                return;
            }
        }

        for (int i = 0; i < actionList.Count;++i )
        {
            actionList[i].Activate();

            if (isDiposableAction == true)
                Destroy(this);
        }

        if (isDisposableTrigger == true)
            Destroy(this);
    }

}