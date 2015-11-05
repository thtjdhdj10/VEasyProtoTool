using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using DicKeyNumber = System.Collections.Generic.Dictionary<int,
System.Collections.Generic.Dictionary<UnityEngine.KeyCode,
    KeyManager.KeyNumber>>;

public class KeyManager : MonoBehaviour {
    

    public int keySetNumber = 0;
    private int keySetCount = 0;

    public static DicKeyNumber keySettings = new DicKeyNumber();

    // 이하의 코드를 KeyManager 클래스를 만들고, 그 안에 넣기
    // 

    void Update()
    {
        // 사용중인 key 값으로 dictionary 순회

        List<GameObject> controlableUnitList =
            VEasyPoolerManager.RefObjectListAtLayer(LayerSetting.StringToMask("Controlable"));

        if (controlableUnitList != null)
        {
            for (int i = 0; i < keySettings[keySetNumber].Count; ++i)
            {
                KeyCode keyCode = keySettings[keySetNumber].Keys.ToList()[i];

                if (Input.GetKeyDown(keyCode))
                {
                    for(int j = 0; j < controlableUnitList.Count; ++j)
                    {
                        var controler = controlableUnitList[j].GetComponent<ControlableUnit>();
                        if (controler == null)
                            continue;
                    }
                }
            }
        }
    }

    public enum KeyNumber
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
    Dictionary<KeyCode, KeyNumber> GetDefaultKeySetting0()
    {
        var ret = new Dictionary<KeyCode, KeyNumber>();

        {
            ret[KeyCode.LeftArrow] = KeyNumber.MOVE_LEFT;
            ret[KeyCode.RightArrow] = KeyNumber.MOVE_RIGHT;
            ret[KeyCode.UpArrow] = KeyNumber.MOVE_UP;
            ret[KeyCode.DownArrow] = KeyNumber.MOVE_DOWN;

            ret[KeyCode.A] = KeyNumber.SKILL_01;
            ret[KeyCode.S] = KeyNumber.SKILL_02;
            ret[KeyCode.D] = KeyNumber.SKILL_03;
            ret[KeyCode.F] = KeyNumber.SKILL_04;
            ret[KeyCode.G] = KeyNumber.SKILL_05;
            ret[KeyCode.H] = KeyNumber.SKILL_06;
            ret[KeyCode.Q] = KeyNumber.SKILL_07;
            ret[KeyCode.W] = KeyNumber.SKILL_08;
            ret[KeyCode.E] = KeyNumber.SKILL_09;
            ret[KeyCode.R] = KeyNumber.SKILL_10;
            ret[KeyCode.T] = KeyNumber.SKILL_11;
            ret[KeyCode.Y] = KeyNumber.SKILL_12;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;

            ret[KeyCode.Z] = KeyNumber.COMMAND_SKILL;
            ret[KeyCode.X] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.C] = KeyNumber.COMMAND_JUMP;
            ret[KeyCode.Space] = KeyNumber.COMMAND_SPECIAL;
        }

        return ret;
    }

    // FPS
    Dictionary<KeyCode, KeyNumber> GetDefaultKeySetting1()
    {
        var ret = new Dictionary<KeyCode, KeyNumber>();

        {
            ret[KeyCode.W] = KeyNumber.MOVE_LEFT;
            ret[KeyCode.A] = KeyNumber.MOVE_RIGHT;
            ret[KeyCode.S] = KeyNumber.MOVE_UP;
            ret[KeyCode.D] = KeyNumber.MOVE_DOWN;

            ret[KeyCode.Space] = KeyNumber.COMMAND_JUMP;

            ret[KeyCode.R] = KeyNumber.COMMAND_RELOAD;
            ret[KeyCode.Q] = KeyNumber.COMMAND_SWAP;
            ret[KeyCode.Mouse0] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = KeyNumber.COMMAND_ZOOM;
            ret[KeyCode.LeftShift] = KeyNumber.COMMAND_SIT;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;
        }

        return ret;
    }

    // V_CODE
    Dictionary<KeyCode, KeyNumber> GetDefaultKeySetting2()
    {
        var ret = new Dictionary<KeyCode, KeyNumber>();

        {
            ret[KeyCode.W] = KeyNumber.MOVE_LEFT;
            ret[KeyCode.A] = KeyNumber.MOVE_RIGHT;
            ret[KeyCode.S] = KeyNumber.MOVE_UP;
            ret[KeyCode.D] = KeyNumber.MOVE_DOWN;

            ret[KeyCode.Mouse0] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = KeyNumber.COMMAND_SKILL;
            ret[KeyCode.Space] = KeyNumber.COMMAND_SPECIAL;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;
        }

        return ret;
    }

    // starcraft
    Dictionary<KeyCode, KeyNumber> GetDefaultKeySetting4()
    {
        var ret = new Dictionary<KeyCode, KeyNumber>();

        {
            ret[KeyCode.Space] = KeyNumber.COMMAND_VIEW_ME;

            ret[KeyCode.R] = KeyNumber.COMMAND_RELOAD;
            ret[KeyCode.A] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.M] = KeyNumber.COMMAND_MOVE;
            ret[KeyCode.H] = KeyNumber.COMMAND_HOLD;
            ret[KeyCode.S] = KeyNumber.COMMAND_STOP;

            ret[KeyCode.Mouse0] = KeyNumber.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = KeyNumber.COMMAND_MOVE_APPLY;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;
            ret[KeyCode.Alpha7] = KeyNumber.ITEM_7;
            ret[KeyCode.Alpha8] = KeyNumber.ITEM_8;
            ret[KeyCode.Alpha9] = KeyNumber.ITEM_9;
        }

        return ret;
    }

    // LOL
    Dictionary<KeyCode, KeyNumber> GetDefaultKeySetting5()
    {
        var ret = new Dictionary<KeyCode, KeyNumber>();

        {
            ret[KeyCode.Space] = KeyNumber.COMMAND_VIEW_ME;

            ret[KeyCode.Q] = KeyNumber.SKILL_01;
            ret[KeyCode.W] = KeyNumber.SKILL_02;
            ret[KeyCode.E] = KeyNumber.SKILL_03;
            ret[KeyCode.R] = KeyNumber.SKILL_04;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;

            ret[KeyCode.A] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.M] = KeyNumber.COMMAND_MOVE;
            ret[KeyCode.S] = KeyNumber.COMMAND_STOP;

            ret[KeyCode.Mouse0] = KeyNumber.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = KeyNumber.COMMAND_MOVE_APPLY;
        }

        return ret;
    }

    void CreateKeySettings(Dictionary<KeyCode, KeyNumber> keySet)
    {
        keySettings[keySetCount] = keySet;

        keySetCount++;
    }

    void EditKeySettings(Dictionary<KeyCode, KeyNumber> keySet, int idx)
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
