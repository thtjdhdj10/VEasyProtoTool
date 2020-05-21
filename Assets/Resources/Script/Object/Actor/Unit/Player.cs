using UnityEngine;

public class Player : Unit
{
    private float _speed = 3.5f;
    private float _hitSpeed = 5f;

    protected override void Start()
    {
        base.Start();

        GetOperable<Movable>().speed = _speed;

        // 플레이어 이동, 회전 처리
        TrgKeyInputs triKeyInput = new TrgKeyInputs(this);
        new ActVectorMoveActor(triKeyInput, 2f);

        TrgFrame triAllways = new TrgFrame(this, 0);
        ConditionBool conMouseTrack = new ConditionBool(triAllways, true);
        new ActDirectionToMouse(triAllways, this);

        // 플레이어 공격 처리
        GetOperable<Shootable>().state.SetState(Multistat.StateType.CLICK, false);

        TrgKeyInput triMouseDown = new TrgKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        new ActActiveOperable<Shootable>(triMouseDown, Multistat.StateType.CLICK, true);

        TrgKeyInput triMouseUp = new TrgKeyInput(
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.UP);
        new ActActiveOperable<Shootable>(triMouseUp, Multistat.StateType.CLICK, false);

        // 플레이어 피격 처리
        float knockbackTime = 0.38f;
        float dodgeTime = 1.3f;

        TrgCollision triCol = new TrgCollision(this, GetOperable<Collidable>(), typeof(Bullet), typeof(Enemy));
        new ActInitTrigger(triCol, triKeyInput);

        new ActKnockback(triCol, this, 8f, 20f);
        new ActSetSpeed(triCol, _hitSpeed);
        new ActSetSpeed(triCol, _speed) { delay = dodgeTime };

        new ActActiveOperable<Controllable>(triCol, Multistat.StateType.KNOCKBACK, true);
        new ActActiveOperable<Controllable>(triCol, Multistat.StateType.KNOCKBACK, false) { delay = knockbackTime };
        new ActActiveOperable<Collidable>(triCol, Multistat.StateType.KNOCKBACK, true);
        new ActActiveOperable<Collidable>(triCol, Multistat.StateType.KNOCKBACK, false) { delay = dodgeTime };
        new ActActiveOperable<Shootable>(triCol, Multistat.StateType.KNOCKBACK, false);
        new ActActiveOperable<Shootable>(triCol, Multistat.StateType.KNOCKBACK, true) { delay = dodgeTime };

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        new ActSetSpriteColor(triCol, sprite, new Color(1f, 1f, 1f, 0.5f));
        new ActSetSpriteColor(triCol, sprite, new Color(1f, 1f, 1f, 1f)) { delay = dodgeTime };
        new ActSetSprite(triCol, sprite,
            ResourcesManager.LoadResource<Sprite>(
                ResourcesManager.ResName.Player_Damaged_strip5));
        new ActSetSprite(triCol, sprite,
            ResourcesManager.LoadResource<Sprite>(
                ResourcesManager.ResName.Player))
        { delay = dodgeTime };
        new ActSetController(triCol, gameObject,
            ResourcesManager.LoadResource<RuntimeAnimatorController>(
                ResourcesManager.ResName.Player_Damaged_Controller));
        new ActSetController(triCol, gameObject, null) { delay = knockbackTime };
        new ActSetAnimatorSpeed(triCol, gameObject, 0.8f);

        new ActSetConditionBool(triCol, conMouseTrack, false);
        new ActSetConditionBool(triCol, conMouseTrack, true) { delay = knockbackTime };

        // 우클릭 시 테스트
        //TriggerKeyInput trgRightClick = new TriggerKeyInput(
        //    this, KeyManager.KeyCommand.COMMAND_SKILL, KeyManager.KeyPressType.DOWN);
        //Pattern_Slayer_1 pattern_S1 = new Pattern_Slayer_1(this);
        //ActionActivatePattern actActivatePattern =
        //    new ActionActivatePattern(trgRightClick, pattern_S1);
    }

    private void Update()
    {
        TextDisplay.TextUpdate(gameObject, "PLAYER HP", "HP: " + unitStatus.CurrentHp);
    }
}
