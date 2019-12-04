using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrackingMouse : Action
{
    public override void Activate(Trigger trigger)
    {
        Vector2 mouseWorldPos = VEasyCalculator.ScreenToWorldPos(Input.mousePosition);

        float dir = VEasyCalculator.GetDirection(trigger.owner.transform.position, mouseWorldPos);
        
        Movable move = trigger.owner.GetComponent<Movable>();

        if(move != null)
        {
            move.direction = dir;
        }
    }

}
