using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDeactiveShootable : Action
{
    public override void Activate(Trigger trigger)
    {
        Shootable shootable = trigger.owner.GetOperable<Shootable>();
        shootable.active = false;
    }
}
