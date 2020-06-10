using UnityEngine;

namespace VEPT
{
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

            BooleanWrapper doTrackMouse = new BooleanWrapper(true);
            TrgFrame TrgAlways = new TrgFrame(this, 0);
            new CndEnable(TrgAlways, doTrackMouse);
            new ActDirectionToMouse(TrgAlways, this);

            // 플레이어 공격 처리
            GetOperable<Shootable>().state.SetState(MultiState.EStateType.CLICK, false);

            TrgKeyInput triMouseDown = new TrgKeyInput(
                this, KeyManager.EKeyCommand.COMMAND_ATTACK, KeyManager.EKeyPressType.DOWN);
            new ActActiveOperable<Shootable>(triMouseDown, MultiState.EStateType.CLICK, true);

            TrgKeyInput triMouseUp = new TrgKeyInput(
                this, KeyManager.EKeyCommand.COMMAND_ATTACK, KeyManager.EKeyPressType.UP);
            new ActActiveOperable<Shootable>(triMouseUp, MultiState.EStateType.CLICK, false);

            // 플레이어 피격 처리
            float knockbackTime = 0.38f;
            float dodgeTime = 1.3f;

            TrgCollision triCol = new TrgCollision(this, typeof(Bullet), typeof(Enemy));
            new ActInitTrigger(triCol, triKeyInput);

            new ActKnockback(triCol, this, 8f, 20f);
            new ActSetSpeed(triCol, _hitSpeed);
            new ActSetSpeed(triCol, _speed) { delay = dodgeTime };

            new ActActiveOperable<Controllable>(triCol, MultiState.EStateType.KNOCKBACK, true);
            new ActActiveOperable<Controllable>(triCol, MultiState.EStateType.KNOCKBACK, false) { delay = knockbackTime };
            new ActActiveOperable<Collidable>(triCol, MultiState.EStateType.KNOCKBACK, true);
            new ActActiveOperable<Collidable>(triCol, MultiState.EStateType.KNOCKBACK, false) { delay = dodgeTime };
            new ActActiveOperable<Shootable>(triCol, MultiState.EStateType.KNOCKBACK, false);
            new ActActiveOperable<Shootable>(triCol, MultiState.EStateType.KNOCKBACK, true) { delay = dodgeTime };

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            new ActSetSpriteColor(triCol, sprite, new Color(1f, 1f, 1f, 0.5f));
            new ActSetSpriteColor(triCol, sprite, new Color(1f, 1f, 1f, 1f)) { delay = dodgeTime };
            new ActSetSprite(triCol, sprite,
                ResourcesManager.LoadResource<Sprite>(EResourceName.Player_Damaged_strip5));
            new ActSetSprite(triCol, sprite,
                ResourcesManager.LoadResource<Sprite>(EResourceName.Player))
            { delay = dodgeTime };
            new ActSetController(triCol, gameObject,
                ResourcesManager.LoadResource<RuntimeAnimatorController>(
                    EResourceName.Player_Damaged_Controller));
            new ActSetController(triCol, gameObject, null) { delay = knockbackTime };
            new ActSetAnimatorSpeed(triCol, gameObject, 0.8f);

            new ActSetRefValue<bool>(triCol, doTrackMouse, false);
            new ActSetRefValue<bool>(triCol, doTrackMouse, true) { delay = knockbackTime };

            // 우클릭 시 테스트
            //TriggerKeyInput trgRightClick = new TriggerKeyInput(
            //    this, KeyManager.KeyCommand.COMMAND_SKILL, KeyManager.KeyPressType.DOWN);
            //Pattern_Slayer_1 pattern_S1 = new Pattern_Slayer_1(this);
            //ActionActivatePattern actActivatePattern =
            //    new ActionActivatePattern(trgRightClick, pattern_S1);
        }
    }
}