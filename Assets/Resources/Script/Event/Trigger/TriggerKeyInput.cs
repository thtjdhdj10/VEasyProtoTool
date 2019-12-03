using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CmdType = System.Collections.Generic.KeyValuePair<KeyManager.KeyCommand,
    KeyManager.KeyPressType>;

using CmdTypeObject = System.Collections.Generic.KeyValuePair<
    System.Collections.Generic.KeyValuePair<KeyManager.KeyCommand,
    KeyManager.KeyPressType>, MyObject>;

// TriggerForKeyInput 은 정해진 키 입력에 대해서만 Activate() 를 호출.

public class TriggerKeyInput : Trigger
{
    public TriggerKeyInput(Unit _owner, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
        : base(_owner)
    {
        command = _command;
        pressType = _pressType;

        CmdType ct = new CmdType(command, pressType);
        CmdTypeObject cto = new CmdTypeObject(ct, owner);
        unitTriggerBindingDic.Add(cto, this);
    }

    ~TriggerKeyInput()
    {
        CmdType ct = new CmdType(command, pressType);
        CmdTypeObject cto = new CmdTypeObject(ct, owner);
        unitTriggerBindingDic.Remove(cto);
    }

    static Dictionary<CmdTypeObject, TriggerKeyInput> unitTriggerBindingDic
        = new Dictionary<CmdTypeObject, TriggerKeyInput>();

    // Controlable 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(
        MyObject obj, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        CmdType cp = new CmdType(_command, _pressType);

        CmdTypeObject cpo = new CmdTypeObject(cp, obj);

        if (unitTriggerBindingDic.ContainsKey(cpo) == true)
            unitTriggerBindingDic[cpo].ActivateTrigger();
    }

    public KeyManager.KeyCommand command;
    public KeyManager.KeyPressType pressType;
}
