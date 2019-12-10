using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    protected override void Start()
    {
        base.Start();

        TriggerKeyInputs trgKeyInput = new TriggerKeyInputs(this);
        new ActionVectorMoveUnit(trgKeyInput);

        TriggerFrame trgTrackingMouse = new TriggerFrame(this, 0);
        new ActionTrackingMouse(trgTrackingMouse);

        TriggerKeyInput trgMouseDown = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        new ActionActiveOperable<Shootable>(trgMouseDown, Multistat.StateType.CLICK, true);

        TriggerKeyInput trgMouseUp = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.UP);
        new ActionActiveOperable<Shootable>(trgMouseUp, Multistat.StateType.CLICK, false);

        float knockSpeed = 1f;
        float knockDecel = 1.5f;
        float knockTime = knockSpeed / knockDecel;

        TriggerCollision trgCol = new TriggerCollision(this, typeof(Bullet), typeof(Enemy));
        new ActionDealDamage(trgCol, 1);
        new ActionKnockback(trgCol, knockSpeed, knockDecel);
        new ActionActiveOperable<Collidable>(trgCol, Multistat.StateType.KNOCKBACK, true);
        new ActionActiveOperable<Collidable>(trgCol, Multistat.StateType.KNOCKBACK, false, knockTime);
        new ActionPrintLog(trgCol, "dd");

        //TriggerKeyInput trgRightClick = new TriggerKeyInput(
        //    this, KeyManager.KeyCommand.COMMAND_SKILL, KeyManager.KeyPressType.DOWN);
        //Pattern_Slayer_1 pattern_S1 = new Pattern_Slayer_1(this);
        //ActionActivatePattern actActivatePattern =
        //    new ActionActivatePattern(trgRightClick, pattern_S1);
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
