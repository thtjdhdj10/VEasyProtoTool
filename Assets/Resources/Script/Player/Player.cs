using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    protected override void Start()
    {
        base.Start();

        TriggerKeyInputs trgKeyInput = new TriggerKeyInputs(this);
        ActionVectorMoveUnit actKeyInput = new ActionVectorMoveUnit();
        actKeyInput.speed = 2f;
        actKeyInput.isRotate = true;
        trgKeyInput.actionList.Add(actKeyInput);

        TriggerFrame trgTrackingMouse = new TriggerFrame(this, 0);
        ActionTrackingMouse actTackingMouse = new ActionTrackingMouse();
        trgTrackingMouse.actionList.Add(actTackingMouse);

        TriggerKeyInput trgMouseDown = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        ActionActiveShootable actActiveShootable = new ActionActiveShootable();
        trgMouseDown.actionList.Add(actActiveShootable);

        TriggerKeyInput trgMouseUp = new TriggerKeyInput(
                    this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.UP);
        ActionDeactiveShootable actDeactiveShootable = new ActionDeactiveShootable();
        trgMouseUp.actionList.Add(actActiveShootable);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        SetDirectionToMouse();


    }

    private void SetDirectionToMouse()
    {
        Vector2 mouseWorldPos = VEasyCalculator.ScreenToWorldPos(Input.mousePosition);

        Movable move = GetOperable(typeof(Movable)) as Movable;
        if (move != null)
        {
            move.direction = VEasyCalculator.GetDirection(transform.position, mouseWorldPos);
        }
    }
}
