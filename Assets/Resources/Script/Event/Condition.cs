using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VEPT
{
    // TODO: 각 condition의 init 구현
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
        public ValueTypeWrapper<bool> state;

        public CndEnable(Trigger trigger, ValueTypeWrapper<bool> _state)
            : base(trigger)
        {
            state = _state;
        }

        public override bool CheckCondition()
        {
            return state.value;
        }
    }
}