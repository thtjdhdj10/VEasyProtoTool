using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using DicKeyCommand = System.Collections.Generic.Dictionary<int,
System.Collections.Generic.Dictionary<UnityEngine.KeyCode,
    KeyManager.KeyCommand>>;

// TODO: 조합 키 지원

public class KeyManager : MonoBehaviour {
    
    private int keySetNumber = 0;
    private int keySetCount = 0;

    public static DicKeyCommand keySettings = new DicKeyCommand();

    //

    private delegate bool GetKeyEachType(KeyCode kc);
    
    private Dictionary<KeyPressType, GetKeyEachType> GetKeyFunctions = new Dictionary<KeyPressType, GetKeyEachType>();

    public enum KeyPressType
    {
        NONE = 0,
        DOWN,
        UP,
        PRESS,
    }

    private void GetFunctionMatch()
    {
        GetKeyFunctions[KeyPressType.DOWN] = Input.GetKeyDown;
        GetKeyFunctions[KeyPressType.UP] = Input.GetKeyUp;
        GetKeyFunctions[KeyPressType.PRESS] = Input.GetKey;
    }

    //

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

        Controlable[] unitArr = FindObjectsOfType<Controlable>();
        for (int i = 0; i < unitArr.Length; ++i)
        {
            controlableList.Add(unitArr[i].gameObject);
        }

//               VEasyPoolerManager.RefObjectListAtLayer(LayerManager.StringToMask("Controlable"));

        if (controlableList == null)
            return;

        List<KeyCode> keyCodeList = keySettings[keySetNumber].Keys.ToList();

        for (int i = 0; i < keyCodeList.Count; ++i)
        {
            KeyCode keyCode = keyCodeList[i];
            
            foreach(KeyPressType type in GetKeyFunctions.Keys)
            {
                if (GetKeyFunctions[type](keyCode) == false)
                    continue;

                for (int j = 0; j < controlableList.Count; ++j)
                {
                    var controler = controlableList[j].GetComponent<Controlable>();
                    if (controler == null)
                        continue;

                    KeyCommand command = keySettings[keySetNumber][keyCode];

                    controler.ReceiveCommand(command, type);
                }
            }  
        }

    }

    void Start()
    {
        // 임시로 V_CODE 에 해당하는 KeySetting 사용

        int number = CreateKeySettings(GetDefaultKeySetting2());
        SetKeySetting(number);
    }

    public enum KeyCommand
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
    Dictionary<KeyCode, KeyCommand> GetDefaultKeySetting0()
    {
        var ret = new Dictionary<KeyCode, KeyCommand>();

        {
            ret[KeyCode.LeftArrow] = KeyCommand.MOVE_LEFT;
            ret[KeyCode.RightArrow] = KeyCommand.MOVE_RIGHT;
            ret[KeyCode.UpArrow] = KeyCommand.MOVE_UP;
            ret[KeyCode.DownArrow] = KeyCommand.MOVE_DOWN;

            ret[KeyCode.A] = KeyCommand.SKILL_01;
            ret[KeyCode.S] = KeyCommand.SKILL_02;
            ret[KeyCode.D] = KeyCommand.SKILL_03;
            ret[KeyCode.F] = KeyCommand.SKILL_04;
            ret[KeyCode.G] = KeyCommand.SKILL_05;
            ret[KeyCode.H] = KeyCommand.SKILL_06;
            ret[KeyCode.Q] = KeyCommand.SKILL_07;
            ret[KeyCode.W] = KeyCommand.SKILL_08;
            ret[KeyCode.E] = KeyCommand.SKILL_09;
            ret[KeyCode.R] = KeyCommand.SKILL_10;
            ret[KeyCode.T] = KeyCommand.SKILL_11;
            ret[KeyCode.Y] = KeyCommand.SKILL_12;

            ret[KeyCode.Alpha1] = KeyCommand.ITEM_1;
            ret[KeyCode.Alpha2] = KeyCommand.ITEM_2;
            ret[KeyCode.Alpha3] = KeyCommand.ITEM_3;
            ret[KeyCode.Alpha4] = KeyCommand.ITEM_4;
            ret[KeyCode.Alpha5] = KeyCommand.ITEM_5;
            ret[KeyCode.Alpha6] = KeyCommand.ITEM_6;

            ret[KeyCode.Z] = KeyCommand.COMMAND_SKILL;
            ret[KeyCode.X] = KeyCommand.COMMAND_ATTACK;
            ret[KeyCode.C] = KeyCommand.COMMAND_JUMP;
            ret[KeyCode.Space] = KeyCommand.COMMAND_SPECIAL;
        }

        return ret;
    }

    // FPS
    Dictionary<KeyCode, KeyCommand> GetDefaultKeySetting1()
    {
        var ret = new Dictionary<KeyCode, KeyCommand>();

        {
            ret[KeyCode.A] = KeyCommand.MOVE_LEFT;
            ret[KeyCode.D] = KeyCommand.MOVE_RIGHT;
            ret[KeyCode.W] = KeyCommand.MOVE_UP;
            ret[KeyCode.S] = KeyCommand.MOVE_DOWN;

            ret[KeyCode.Space] = KeyCommand.COMMAND_JUMP;

            ret[KeyCode.R] = KeyCommand.COMMAND_RELOAD;
            ret[KeyCode.Q] = KeyCommand.COMMAND_SWAP;
            ret[KeyCode.Mouse0] = KeyCommand.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = KeyCommand.COMMAND_ZOOM;
            ret[KeyCode.LeftShift] = KeyCommand.COMMAND_SIT;

            ret[KeyCode.Alpha1] = KeyCommand.ITEM_1;
            ret[KeyCode.Alpha2] = KeyCommand.ITEM_2;
            ret[KeyCode.Alpha3] = KeyCommand.ITEM_3;
            ret[KeyCode.Alpha4] = KeyCommand.ITEM_4;
            ret[KeyCode.Alpha5] = KeyCommand.ITEM_5;
            ret[KeyCode.Alpha6] = KeyCommand.ITEM_6;
        }

        return ret;
    }

    // V_CODE
    Dictionary<KeyCode, KeyCommand> GetDefaultKeySetting2()
    {
        var ret = new Dictionary<KeyCode, KeyCommand>();

        {
            ret[KeyCode.A] = KeyCommand.MOVE_LEFT;
            ret[KeyCode.D] = KeyCommand.MOVE_RIGHT;
            ret[KeyCode.W] = KeyCommand.MOVE_UP;
            ret[KeyCode.S] = KeyCommand.MOVE_DOWN;

            ret[KeyCode.Mouse0] = KeyCommand.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = KeyCommand.COMMAND_SKILL;
            ret[KeyCode.Space] = KeyCommand.COMMAND_SPECIAL;

            ret[KeyCode.Alpha1] = KeyCommand.ITEM_1;
            ret[KeyCode.Alpha2] = KeyCommand.ITEM_2;
            ret[KeyCode.Alpha3] = KeyCommand.ITEM_3;
            ret[KeyCode.Alpha4] = KeyCommand.ITEM_4;
            ret[KeyCode.Alpha5] = KeyCommand.ITEM_5;
            ret[KeyCode.Alpha6] = KeyCommand.ITEM_6;
        }

        return ret;
    }

    // starcraft
    Dictionary<KeyCode, KeyCommand> GetDefaultKeySetting4()
    {
        var ret = new Dictionary<KeyCode, KeyCommand>();

        {
            ret[KeyCode.Space] = KeyCommand.COMMAND_VIEW_ME;

            ret[KeyCode.R] = KeyCommand.COMMAND_RELOAD;
            ret[KeyCode.A] = KeyCommand.COMMAND_ATTACK;
            ret[KeyCode.M] = KeyCommand.COMMAND_MOVE;
            ret[KeyCode.H] = KeyCommand.COMMAND_HOLD;
            ret[KeyCode.S] = KeyCommand.COMMAND_STOP;

            ret[KeyCode.Mouse0] = KeyCommand.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = KeyCommand.COMMAND_MOVE_APPLY;

            ret[KeyCode.Alpha1] = KeyCommand.ITEM_1;
            ret[KeyCode.Alpha2] = KeyCommand.ITEM_2;
            ret[KeyCode.Alpha3] = KeyCommand.ITEM_3;
            ret[KeyCode.Alpha4] = KeyCommand.ITEM_4;
            ret[KeyCode.Alpha5] = KeyCommand.ITEM_5;
            ret[KeyCode.Alpha6] = KeyCommand.ITEM_6;
            ret[KeyCode.Alpha7] = KeyCommand.ITEM_7;
            ret[KeyCode.Alpha8] = KeyCommand.ITEM_8;
            ret[KeyCode.Alpha9] = KeyCommand.ITEM_9;
        }

        return ret;
    }

    // LOL
    Dictionary<KeyCode, KeyCommand> GetDefaultKeySetting5()
    {
        var ret = new Dictionary<KeyCode, KeyCommand>();

        {
            ret[KeyCode.Space] = KeyCommand.COMMAND_VIEW_ME;

            ret[KeyCode.Q] = KeyCommand.SKILL_01;
            ret[KeyCode.W] = KeyCommand.SKILL_02;
            ret[KeyCode.E] = KeyCommand.SKILL_03;
            ret[KeyCode.R] = KeyCommand.SKILL_04;

            ret[KeyCode.Alpha1] = KeyCommand.ITEM_1;
            ret[KeyCode.Alpha2] = KeyCommand.ITEM_2;
            ret[KeyCode.Alpha3] = KeyCommand.ITEM_3;
            ret[KeyCode.Alpha4] = KeyCommand.ITEM_4;
            ret[KeyCode.Alpha5] = KeyCommand.ITEM_5;
            ret[KeyCode.Alpha6] = KeyCommand.ITEM_6;

            ret[KeyCode.A] = KeyCommand.COMMAND_ATTACK;
            ret[KeyCode.M] = KeyCommand.COMMAND_MOVE;
            ret[KeyCode.S] = KeyCommand.COMMAND_STOP;

            ret[KeyCode.Mouse0] = KeyCommand.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = KeyCommand.COMMAND_MOVE_APPLY;
        }

        return ret;
    }

    int CreateKeySettings(Dictionary<KeyCode, KeyCommand> keySet)
    {
        keySettings[keySetCount] = keySet;

        keySetCount++;

        return keySetCount - 1;
    }

    void EditKeySettings(Dictionary<KeyCode, KeyCommand> keySet, int idx)
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
