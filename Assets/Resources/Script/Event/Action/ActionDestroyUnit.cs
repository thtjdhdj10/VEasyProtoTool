using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDestroyUnit : Action
{
    public Unit target;

    public ActionDestroyUnit(Trigger trigger, Unit _target)
        :base(trigger)
    {
        target = _target;
    }

    public override void Activate(Trigger trigger)
    {
        GameObject.Destroy(target.gameObject);
    }
}
