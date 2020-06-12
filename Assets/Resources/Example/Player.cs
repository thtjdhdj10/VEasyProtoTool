using UnityEngine;

namespace VEPT
{
    public class Player : Unit
    {
        private float _speed = 3.5f;
        private float _hitSpeed = 5f;

        public override void Init()
        {
            base.Init();

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
            new ActActivateOperable<Shootable>(triMouseDown, MultiState.EStateType.CLICK, true);

            TrgKeyInput triMouseUp = new TrgKeyInput(
                this, KeyManager.EKeyCommand.COMMAND_ATTACK, KeyManager.EKeyPressType.UP);
            new ActActivateOperable<Shootable>(triMouseUp, MultiState.EStateType.CLICK, false);

            // 플레이어 피격 처리
            float knockbackTime = 0.38f;
            float dodgeTime = 1.3f;

            TrgCollision triCol = new TrgCollision(this, typeof(Bullet), typeof(Enemy));
            new ActInitTrigger(triCol, triKeyInput);

            new ActGetDamage(triCol, 1);

            new ActKnockback(triCol, this, 8f, 20f);
            new ActSetSpeed(triCol, _hitSpeed);
            new ActSetSpeed(triCol, _speed) { delay = dodgeTime };

            new ActActivateOperable<Controllable>(triCol, MultiState.EStateType.KNOCKBACK, true);
            new ActActivateOperable<Controllable>(triCol, MultiState.EStateType.KNOCKBACK, false) { delay = knockbackTime };
            new ActActivateOperable<Collidable>(triCol, MultiState.EStateType.KNOCKBACK, true);
            new ActActivateOperable<Collidable>(triCol, MultiState.EStateType.KNOCKBACK, false) { delay = dodgeTime };
            new ActActivateOperable<Shootable>(triCol, MultiState.EStateType.KNOCKBACK, false);
            new ActActivateOperable<Shootable>(triCol, MultiState.EStateType.KNOCKBACK, true) { delay = dodgeTime };

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

            new ActSetValue<bool>(triCol, doTrackMouse, false);
            new ActSetValue<bool>(triCol, doTrackMouse, true) { delay = knockbackTime };

            // test code: 키보드 숫자키 클릭 시 작동
            TrgKeyInput trgPress1 = new TrgKeyInput(
                this, KeyManager.EKeyCommand.ITEM_1, KeyManager.EKeyPressType.DOWN);
            new ActSpawnActor(trgPress1, EResourceName.Enemy_Wing);

            TrgKeyInput trgPress2 = new TrgKeyInput(
                this, KeyManager.EKeyCommand.ITEM_2, KeyManager.EKeyPressType.DOWN);
            new ActDestroyRandomRandom(trgPress2);

            TrgKeyInput trgPress3 = new TrgKeyInput(
                this, KeyManager.EKeyCommand.ITEM_3, KeyManager.EKeyPressType.DOWN);
            new ActTest(trgPress3);

            //Pattern_Slayer_1 pattern_S1 = new Pattern_Slayer_1(this);
            //ActionActivatePattern actActivatePattern =
            //    new ActionActivatePattern(trgRightClick, pattern_S1);
        }
    }

    // test code

    public class ActTest : Action
    {
        public ActTest(Trigger trigger)
            : base(trigger) { }

        protected override void ActionProcess(Trigger trigger)
        {

        }
    }

    public class ActDestroyRandomRandom : Action
    {
        public ActDestroyRandomRandom(Trigger trigger)
            : base(trigger) { }

        protected override void ActionProcess(Trigger trigger)
        {
            var enemy = Object.FindObjectOfType<Enemy>();
            enemy.Destroy(enemy.gameObject);
        }
    }
}