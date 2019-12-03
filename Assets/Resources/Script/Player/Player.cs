using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    protected override void Start()
    {
        base.Start();

        TriggerKeyInputs keyTrigger = GetComponent<TriggerKeyInputs>();
        if (keyTrigger != null)
        {
            keyTrigger.actionList.Add(new ActionVectorMoveUnit());
        }
    }



}
