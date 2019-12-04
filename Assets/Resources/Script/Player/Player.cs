using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    protected override void Start()
    {
        base.Start();

        TriggerKeyInputs trgInputs = new TriggerKeyInputs(this);
        ActionVectorMoveUnit move = new ActionVectorMoveUnit();
        move.speed = 2f;
        move.isRotate = true;
        trgInputs.actionList.Add(move);

        TriggerFrame trgFrame = new TriggerFrame(this, 0);
        ActionTrackingMouse tracking = new ActionTrackingMouse();
        trgFrame.actionList.Add(tracking);
    }



}
