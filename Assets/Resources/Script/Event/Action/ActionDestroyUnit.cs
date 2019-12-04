using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDestroyUnit : Action
{
    public override void Activate(Trigger trigger)
    {
        GameObject.Destroy(trigger.owner);
    }
}
