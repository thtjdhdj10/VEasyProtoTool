using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Condition
{
    public Condition(Trigger trigger)
    {
        trigger.conditionList.Add(this);
    }

    public abstract bool CheckCondition();

    public virtual void Init()
    {

    }

}

public class CndEnable : Condition
{
    public RefValue<bool> state;

    public CndEnable(Trigger trigger, RefValue<bool> _state)
        : base(trigger)
    {
        state = _state;
    }

    public override bool CheckCondition()
    {
        return state.value;
    }
}