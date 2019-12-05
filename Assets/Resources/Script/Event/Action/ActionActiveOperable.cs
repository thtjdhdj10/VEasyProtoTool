using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionActiveOperable<T> : Action where T : Operable
{
    public bool doActive;

    public ActionActiveOperable(bool _doActive)
    {
        doActive = _doActive;
    }

    public override void Activate(Trigger trigger)
    {
        Operable operable = trigger.owner.GetOperable<T>();
        operable.active = doActive;
    }

}
