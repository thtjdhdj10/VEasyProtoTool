using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    protected override void Start()
    {
        base.Start();

        TriggerKeyInputs inputsTrigger = new TriggerKeyInputs(this);
        if (inputsTrigger != null)
        {
            ActionVectorMoveUnit move = new ActionVectorMoveUnit();
            move.speed = 2f;
            move.isRotate = false;
            inputsTrigger.actionList.Add(move);
        }
    }



}
