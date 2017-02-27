using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionLog : Action
{
    public string text;

    public override void Activate(Trigger trigger)
    {
        Debug.Log(text);
    }
}
