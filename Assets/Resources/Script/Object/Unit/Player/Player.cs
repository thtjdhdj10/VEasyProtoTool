﻿using UnityEngine;
using System.Collections.Generic;

public class Player : Unit
{
    private float speed = 2.5f;
    private float hitSpeed = 5f;

    protected override void Start()
    {
        base.Start();

        GetOperable<Movable>().speed = speed;

        TriggerKeyInputs triKeyInput = new TriggerKeyInputs(this);
        new ActionVectorMoveUnit(triKeyInput, 2f);

        TriggerFrame triAllways = new TriggerFrame(this, 0);
        ConditionBool conMouseTrack = new ConditionBool(triAllways, true);
        new ActionDirectionToMouse(triAllways, this);

        TriggerKeyInput triMouseDown = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        new ActionActiveOperable<Shootable>(triMouseDown, Multistat.StateType.CLICK, true);

        TriggerKeyInput triMouseUp = new TriggerKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.UP);
        new ActionActiveOperable<Shootable>(triMouseUp, Multistat.StateType.CLICK, false);

        {
            // 플레이어 피격 처리
            TriggerCollision triCol = new TriggerCollision(this, typeof(Bullet), typeof(Enemy));
            new ActionInitTrigger(triCol, triKeyInput);

            new ActionKnockback(triCol, this, 8f, 15f);
            new ActionSetSpeed(triCol, hitSpeed);
            new ActionSetSpeed(triCol, speed) { delay = 1.8f };

            new ActionActiveOperable<Controllable>(triCol, Multistat.StateType.KNOCKBACK, true);
            new ActionActiveOperable<Controllable>(triCol, Multistat.StateType.KNOCKBACK, false) { delay = 0.8f };
            new ActionActiveOperable<Collidable>(triCol, Multistat.StateType.KNOCKBACK, true);
            new ActionActiveOperable<Collidable>(triCol, Multistat.StateType.KNOCKBACK, false) { delay = 1.8f };
            new ActionActiveOperable<Shootable>(triCol, Multistat.StateType.KNOCKBACK, false);
            new ActionActiveOperable<Shootable>(triCol, Multistat.StateType.KNOCKBACK, true) { delay = 1.8f };

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            new ActionSetSpriteColor(triCol, sprite, new Color(1f, 1f, 1f, 0.5f));
            new ActionSetSprite(triCol, sprite,
                ResourcesManager<Sprite>.LoadResource(
                    ResourcesManager<Sprite>.ResourceName.Player_BeHit_strip5));
            new ActionSetAnimatorSpeed(triCol, gameObject, 0.2f);
            new ActionSetController(triCol, gameObject,
                ResourcesManager<RuntimeAnimatorController>.LoadResource(
                    ResourcesManager<RuntimeAnimatorController>.ResourceName.Player_BeHit_strip5_0));
            new ActionSetSprite(triCol, sprite,
                ResourcesManager<Sprite>.LoadResource(
                    ResourcesManager<Sprite>.ResourceName.Player)) { delay = 1.8f };
            new ActionSetSpriteColor(triCol, sprite, new Color(1f, 1f, 1f, 1f)) { delay = 1.8f };

            new ActionSetConditionBool(triCol, conMouseTrack, false);
            new ActionSetConditionBool(triCol, conMouseTrack, true) { delay = 0.8f };

            new ActionPrintLog(triCol, "Player Hitted!");
        }
        
        //TriggerKeyInput trgRightClick = new TriggerKeyInput(
        //    this, KeyManager.KeyCommand.COMMAND_SKILL, KeyManager.KeyPressType.DOWN);
        //Pattern_Slayer_1 pattern_S1 = new Pattern_Slayer_1(this);
        //ActionActivatePattern actActivatePattern =
        //    new ActionActivatePattern(trgRightClick, pattern_S1);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        SetShootableDirection();
    }

    private void SetShootableDirection()
    {
        Shootable shoot = GetOperable<Shootable>();
        if(shoot != null) shoot.fireDirection = direction;
    }
}
