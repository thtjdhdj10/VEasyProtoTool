using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionActiveTargetOperable<T> : ActionActiveOperable<T> where T : Operable
{
    public ActionActiveTargetOperable(Trigger trigger, bool _doActive)
        : base(trigger, _doActive)
    {

    }

    public ActionActiveTargetOperable(Trigger trigger, bool _doActive, float _delay)
        : base(trigger, _doActive, _delay)
    {

    }

    protected override void ApplyActive(Trigger trigger)
    {
        if (trigger == null) return;

        Unit target = trigger.owner;
        if(trigger is TriggerCollision)
            target = (trigger as TriggerCollision).target;

        if (target == null) return;

        Operable operable = target.GetOperable<T>();
        operable.active = doActive;
    }

}
