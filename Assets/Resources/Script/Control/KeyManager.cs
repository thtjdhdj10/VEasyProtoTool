using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace VEPT
{
    using DicKeyCodeCommand = System.Collections.Generic.Dictionary<int,
        System.Collections.Generic.Dictionary<UnityEngine.KeyCode,
        KeyManager.EKeyCommand>>;

    public class KeyManager : MonoBehaviour
    {

        private int keySetNumber = 0;
        private int keySetCount = 0;

        public static DicKeyCodeCommand keySettings = new DicKeyCodeCommand();

        //

        private delegate bool GetKeyEachType(KeyCode kc);

        private Dictionary<EKeyPressType, GetKeyEachType> GetKeyFunctions =
            new Dictionary<EKeyPressType, GetKeyEachType>();

        public enum EKeyPressType
        {
            NONE = 0,
            DOWN,
            UP,
            PRESS,
        }

        private void GetFunctionMatch()
        {
            GetKeyFunctions[EKeyPressType.DOWN] = Input.GetKeyDown;
            GetKeyFunctions[EKeyPressType.UP] = Input.GetKeyUp;
            GetKeyFunctions[EKeyPressType.PRESS] = Input.GetKey;
        }

        //

        void Start()
        {
            // 임시로 V_CODE 에 해당하는 KeySetting 사용

            int number = CreateKeySettings(GetDefaultKeySetting2());
            SetKeySetting(number);
        }

        void Awake()
        {
            GetFunctionMatch();

        }

        void Update()
        {
            GiveCommand();

        }

        // 사용중인 key 값으로 dictionary 순회.
        // 유효한 KeyCode 들이 선택되어, Controlable 레이어에 있는 모든 유닛들에게
        // KeyCode 와 매칭되는 KeyCommand 가 전달된다.
        void GiveCommand()
        {
            List<GameObject> controlableList = new List<GameObject>();

            Controllable[] unitArr = FindObjectsOfType<Controllable>();
            for (int i = 0; i < unitArr.Length; ++i)
            {
                controlableList.Add(unitArr[i].gameObject);
            }

            if (controlableList == null)
                return;

            List<KeyCode> keyCodeList = keySettings[keySetNumber].Keys.ToList();

            for (int i = 0; i < keyCodeList.Count; ++i)
            {
                KeyCode keyCode = keyCodeList[i];

                foreach (EKeyPressType type in GetKeyFunctions.Keys)
                {
                    if (GetKeyFunctions[type](keyCode) == false)
                        continue;

                    for (int j = 0; j < controlableList.Count; ++j)
                    {
                        var controler = controlableList[j].GetComponent<Controllable>();
                        if (controler == null)
                            continue;

                        EKeyCommand command = keySettings[keySetNumber][keyCode];

                        controler.KeyInput(command, type);
                    }
                }
            }
        }

        public enum EKeyCommand
        {
            NONE = 0,

            MOVE_LEFT,
            MOVE_RIGHT,
            MOVE_UP,
            MOVE_DOWN,

            SKILL_01,
            SKILL_02,
            SKILL_03,
            SKILL_04,
            SKILL_05,
            SKILL_06,
            SKILL_07,
            SKILL_08,
            SKILL_09,
            SKILL_10,
            SKILL_11,
            SKILL_12,

            ITEM_1,
            ITEM_2,
            ITEM_3,
            ITEM_4,
            ITEM_5,
            ITEM_6,
            ITEM_7,
            ITEM_8,
            ITEM_9,

            COMMAND_SKILL,
            COMMAND_ATTACK,
            COMMAND_JUMP,

            COMMAND_SPECIAL,
            COMMAND_RELOAD,
            COMMAND_SWAP,

            COMMAND_ZOOM,
            COMMAND_VIEW_ME,
            COMMAND_SIT,

            COMMAND_STOP,
            COMMAND_HOLD,
            COMMAND_MOVE,

            COMMAND_APPLY,
            COMMAND_MOVE_APPLY,

        }

        // DNF
        Dictionary<KeyCode, EKeyCommand> GetDefaultKeySetting0()
        {
            var ret = new Dictionary<KeyCode, EKeyCommand>();

            {
                ret[KeyCode.LeftArrow] = EKeyCommand.MOVE_LEFT;
                ret[KeyCode.RightArrow] = EKeyCommand.MOVE_RIGHT;
                ret[KeyCode.UpArrow] = EKeyCommand.MOVE_UP;
                ret[KeyCode.DownArrow] = EKeyCommand.MOVE_DOWN;

                ret[KeyCode.A] = EKeyCommand.SKILL_01;
                ret[KeyCode.S] = EKeyCommand.SKILL_02;
                ret[KeyCode.D] = EKeyCommand.SKILL_03;
                ret[KeyCode.F] = EKeyCommand.SKILL_04;
                ret[KeyCode.G] = EKeyCommand.SKILL_05;
                ret[KeyCode.H] = EKeyCommand.SKILL_06;
                ret[KeyCode.Q] = EKeyCommand.SKILL_07;
                ret[KeyCode.W] = EKeyCommand.SKILL_08;
                ret[KeyCode.E] = EKeyCommand.SKILL_09;
                ret[KeyCode.R] = EKeyCommand.SKILL_10;
                ret[KeyCode.T] = EKeyCommand.SKILL_11;
                ret[KeyCode.Y] = EKeyCommand.SKILL_12;

                ret[KeyCode.Alpha1] = EKeyCommand.ITEM_1;
                ret[KeyCode.Alpha2] = EKeyCommand.ITEM_2;
                ret[KeyCode.Alpha3] = EKeyCommand.ITEM_3;
                ret[KeyCode.Alpha4] = EKeyCommand.ITEM_4;
                ret[KeyCode.Alpha5] = EKeyCommand.ITEM_5;
                ret[KeyCode.Alpha6] = EKeyCommand.ITEM_6;

                ret[KeyCode.Z] = EKeyCommand.COMMAND_SKILL;
                ret[KeyCode.X] = EKeyCommand.COMMAND_ATTACK;
                ret[KeyCode.C] = EKeyCommand.COMMAND_JUMP;
                ret[KeyCode.Space] = EKeyCommand.COMMAND_SPECIAL;
            }

            return ret;
        }

        // FPS
        Dictionary<KeyCode, EKeyCommand> GetDefaultKeySetting1()
        {
            var ret = new Dictionary<KeyCode, EKeyCommand>();

            {
                ret[KeyCode.A] = EKeyCommand.MOVE_LEFT;
                ret[KeyCode.D] = EKeyCommand.MOVE_RIGHT;
                ret[KeyCode.W] = EKeyCommand.MOVE_UP;
                ret[KeyCode.S] = EKeyCommand.MOVE_DOWN;

                ret[KeyCode.Space] = EKeyCommand.COMMAND_JUMP;

                ret[KeyCode.R] = EKeyCommand.COMMAND_RELOAD;
                ret[KeyCode.Q] = EKeyCommand.COMMAND_SWAP;
                ret[KeyCode.Mouse0] = EKeyCommand.COMMAND_ATTACK;
                ret[KeyCode.Mouse1] = EKeyCommand.COMMAND_ZOOM;
                ret[KeyCode.LeftShift] = EKeyCommand.COMMAND_SIT;

                ret[KeyCode.Alpha1] = EKeyCommand.ITEM_1;
                ret[KeyCode.Alpha2] = EKeyCommand.ITEM_2;
                ret[KeyCode.Alpha3] = EKeyCommand.ITEM_3;
                ret[KeyCode.Alpha4] = EKeyCommand.ITEM_4;
                ret[KeyCode.Alpha5] = EKeyCommand.ITEM_5;
                ret[KeyCode.Alpha6] = EKeyCommand.ITEM_6;
            }

            return ret;
        }

        // V_CODE
        Dictionary<KeyCode, EKeyCommand> GetDefaultKeySetting2()
        {
            var ret = new Dictionary<KeyCode, EKeyCommand>();

            {
                ret[KeyCode.A] = EKeyCommand.MOVE_LEFT;
                ret[KeyCode.D] = EKeyCommand.MOVE_RIGHT;
                ret[KeyCode.W] = EKeyCommand.MOVE_UP;
                ret[KeyCode.S] = EKeyCommand.MOVE_DOWN;

                ret[KeyCode.Mouse0] = EKeyCommand.COMMAND_ATTACK;
                ret[KeyCode.Mouse1] = EKeyCommand.COMMAND_SKILL;
                ret[KeyCode.Space] = EKeyCommand.COMMAND_SPECIAL;

                ret[KeyCode.Alpha1] = EKeyCommand.ITEM_1;
                ret[KeyCode.Alpha2] = EKeyCommand.ITEM_2;
                ret[KeyCode.Alpha3] = EKeyCommand.ITEM_3;
                ret[KeyCode.Alpha4] = EKeyCommand.ITEM_4;
                ret[KeyCode.Alpha5] = EKeyCommand.ITEM_5;
                ret[KeyCode.Alpha6] = EKeyCommand.ITEM_6;
            }

            return ret;
        }

        // starcraft
        Dictionary<KeyCode, EKeyCommand> GetDefaultKeySetting4()
        {
            var ret = new Dictionary<KeyCode, EKeyCommand>();

            {
                ret[KeyCode.Space] = EKeyCommand.COMMAND_VIEW_ME;

                ret[KeyCode.R] = EKeyCommand.COMMAND_RELOAD;
                ret[KeyCode.A] = EKeyCommand.COMMAND_ATTACK;
                ret[KeyCode.M] = EKeyCommand.COMMAND_MOVE;
                ret[KeyCode.H] = EKeyCommand.COMMAND_HOLD;
                ret[KeyCode.S] = EKeyCommand.COMMAND_STOP;

                ret[KeyCode.Mouse0] = EKeyCommand.COMMAND_APPLY;
                ret[KeyCode.Mouse1] = EKeyCommand.COMMAND_MOVE_APPLY;

                ret[KeyCode.Alpha1] = EKeyCommand.ITEM_1;
                ret[KeyCode.Alpha2] = EKeyCommand.ITEM_2;
                ret[KeyCode.Alpha3] = EKeyCommand.ITEM_3;
                ret[KeyCode.Alpha4] = EKeyCommand.ITEM_4;
                ret[KeyCode.Alpha5] = EKeyCommand.ITEM_5;
                ret[KeyCode.Alpha6] = EKeyCommand.ITEM_6;
                ret[KeyCode.Alpha7] = EKeyCommand.ITEM_7;
                ret[KeyCode.Alpha8] = EKeyCommand.ITEM_8;
                ret[KeyCode.Alpha9] = EKeyCommand.ITEM_9;
            }

            return ret;
        }

        // LOL
        Dictionary<KeyCode, EKeyCommand> GetDefaultKeySetting5()
        {
            var ret = new Dictionary<KeyCode, EKeyCommand>();

            {
                ret[KeyCode.Space] = EKeyCommand.COMMAND_VIEW_ME;

                ret[KeyCode.Q] = EKeyCommand.SKILL_01;
                ret[KeyCode.W] = EKeyCommand.SKILL_02;
                ret[KeyCode.E] = EKeyCommand.SKILL_03;
                ret[KeyCode.R] = EKeyCommand.SKILL_04;

                ret[KeyCode.Alpha1] = EKeyCommand.ITEM_1;
                ret[KeyCode.Alpha2] = EKeyCommand.ITEM_2;
                ret[KeyCode.Alpha3] = EKeyCommand.ITEM_3;
                ret[KeyCode.Alpha4] = EKeyCommand.ITEM_4;
                ret[KeyCode.Alpha5] = EKeyCommand.ITEM_5;
                ret[KeyCode.Alpha6] = EKeyCommand.ITEM_6;

                ret[KeyCode.A] = EKeyCommand.COMMAND_ATTACK;
                ret[KeyCode.M] = EKeyCommand.COMMAND_MOVE;
                ret[KeyCode.S] = EKeyCommand.COMMAND_STOP;

                ret[KeyCode.Mouse0] = EKeyCommand.COMMAND_APPLY;
                ret[KeyCode.Mouse1] = EKeyCommand.COMMAND_MOVE_APPLY;
            }

            return ret;
        }

        int CreateKeySettings(Dictionary<KeyCode, EKeyCommand> keySet)
        {
            keySettings[keySetCount] = keySet;

            keySetCount++;

            return keySetCount - 1;
        }

        void EditKeySettings(Dictionary<KeyCode, EKeyCommand> keySet, int idx)
        {
            if (idx < 0 || idx >= keySetCount)
                return;

            keySettings[idx] = keySet;
        }

        void SetKeySetting(int number)
        {
            if (number < 0 || number >= keySetCount)
                return;

            keySetNumber = number;
        }
    }
}