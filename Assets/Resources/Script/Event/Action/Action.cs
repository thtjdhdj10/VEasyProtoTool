using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Action
{
    public virtual void Init()
    {

    }

    public abstract void Activate(Trigger trigger);

    public Action(Trigger trigger)
    {
        trigger.actionList.Add(this);
    }
}