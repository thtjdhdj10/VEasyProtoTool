using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionActiveOperable<T> : Action where T : Operable
{
    protected bool doActive;
    protected float delay = 0f;

    public ActionActiveOperable(Trigger trigger, bool _doActive)
        :base(trigger)
    {
        doActive = _doActive;
    }

    public ActionActiveOperable(Trigger trigger, bool _doActive, float _delay)
        :base(trigger)
    {
        doActive = _doActive;
        delay = _delay;
    }

    public override void Activate(Trigger trigger)
    {
        if (delay == 0f) ApplyActive(trigger);
        else GameManager.gm.StartCoroutine(DelayedActive(trigger));
    }

    IEnumerator DelayedActive(Trigger trigger)
    {
        yield return new WaitForSeconds(delay);
        ApplyActive(trigger);
    }

    protected virtual void ApplyActive(Trigger trigger)
    {
        Operable operable = trigger.owner.GetOperable<T>();
        operable.active = doActive;
    }

}
