using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionActivatePattern : Action
{
    public Pattern pattern;

    public ActionActivatePattern(Trigger trigger, Pattern _pattern)
        :base(trigger)
    {
        pattern = _pattern;
    }

    public override void Activate(Trigger trigger)
    {
        pattern.Activate();
    }

}
