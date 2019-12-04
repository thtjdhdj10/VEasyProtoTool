using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    protected override void Start()
    {
        base.Start();

        TriggerKeyInputs trgKeyInput = new TriggerKeyInputs(this);
        ActionVectorMoveUnit actKeyInput = new ActionVectorMoveUnit();
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
        trgMouseUp.actionList.Add(actDeactiveShootable);
    }
     
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        SetDirectionToMouse();

        SetShootableDirection();
    }

    private void SetDirectionToMouse()
    {
        Vector2 mouseWorldPos = VEasyCalculator.ScreenToWorldPos(Input.mousePosition);

        Movable move = GetOperable<Movable>();
        if (move != null)
        {
            move.direction = VEasyCalculator.GetDirection(transform.position, mouseWorldPos);
        }
    }

    private void SetShootableDirection()
    {
        Shootable shoot = GetOperable<Shootable>();
        Movable move = GetOperable<Movable>();
        if(shoot != null &&
            move != null)
        {
            shoot.fireDirection = move.direction;
        }
    }
}
