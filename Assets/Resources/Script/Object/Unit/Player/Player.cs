using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    protected override void Start()
    {
        base.Start();

        TriggerKeyInputs trgKeyInput = new TriggerKeyInputs(this);
        ActionVectorMoveUnit actKeyInput = new ActionVectorMoveUnit(trgKeyInput);

        TriggerFrame trgTrackingMouse = new TriggerFrame(this, 0);
        ActionTrackingMouse actTackingMouse = new ActionTrackingMouse(trgTrackingMouse);

        TriggerKeyInput trgMouseDown = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        ActionActiveOperable<Shootable> actActiveShootable =
            new ActionActiveOperable<Shootable>(trgMouseDown, true);

        TriggerKeyInput trgMouseUp = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.UP);
        ActionActiveOperable<Shootable> actDeactiveShootable
            = new ActionActiveOperable<Shootable>(trgMouseUp, false);

        TriggerKeyInput trgRightClick = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_SKILL, KeyManager.KeyPressType.DOWN);
        //Pattern_Slayer_1 pattern_S1 = new Pattern_Slayer_1()
        //ActionActivatePattern actActivatePattern =
        //    new ActionActivatePattern(trgRightClick);
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
        direction = VEasyCalculator.GetDirection(transform.position, mouseWorldPos);
    }

    private void SetShootableDirection()
    {
        Shootable shoot = GetOperable<Shootable>();
        if(shoot != null) shoot.fireDirection = direction;
    }
}
