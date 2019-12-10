using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    private float speed = 3f;
    private float hitSpeed = 5f;

    protected override void Start()
    {
        base.Start();

        TriggerKeyInputs trgKeyInput = new TriggerKeyInputs(this);
        new ActionVectorMoveUnit(trgKeyInput, 2f);

        TriggerKeyInput trgMouseDown = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        new ActionActiveOperable<Shootable>(trgMouseDown, Multistat.StateType.CLICK, true);

        TriggerKeyInput trgMouseUp = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.UP);
        new ActionActiveOperable<Shootable>(trgMouseUp, Multistat.StateType.CLICK, false);

        TriggerCollision trgCol = new TriggerCollision(this, typeof(Bullet), typeof(Enemy));
        new ActionGetDamage(trgCol, 1);
        new ActionKnockback(trgCol, 3f, 2f);
        new ActionSetSpeed(trgCol, hitSpeed);
        new ActionSetSpeed(trgCol, speed) { delay = 1f };
        new ActionActiveOperable<Controlable>(trgCol, Multistat.StateType.KNOCKBACK, true);
        new ActionActiveOperable<Controlable>(trgCol, Multistat.StateType.KNOCKBACK, false)
        { delay = 1f };
        new ActionActiveOperable<Collidable>(trgCol, Multistat.StateType.KNOCKBACK, true);
        new ActionActiveOperable<Collidable>(trgCol, Multistat.StateType.KNOCKBACK, false)
        { delay = 2f };
        new ActionPrintLog(trgCol, "Player Hitted!");

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
