using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CmdType = System.Collections.Generic.KeyValuePair<KeyManager.KeyCommand,
    KeyManager.KeyPressType>;

using CmdTypeObject = System.Collections.Generic.KeyValuePair<
    System.Collections.Generic.KeyValuePair<KeyManager.KeyCommand,
    KeyManager.KeyPressType>, MyObject>;

// TriggerForKeyInput 은 정해진 키 입력에 대해서만 Activate() 를 호출.

public class TriggerForKeyInput : Trigger
{
    static Dictionary<CmdTypeObject, TriggerForKeyInput> unitTriggerBindingDic
        = new Dictionary<CmdTypeObject, TriggerForKeyInput>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(
        MyObject obj, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        CmdType kk = new CmdType(_command, _pressType);

        CmdTypeObject kko = new CmdTypeObject(kk, obj);

        if (unitTriggerBindingDic.ContainsKey(kko) == true)
            unitTriggerBindingDic[kko].ActivateTrigger();
    }

    public MyObject target;

    public KeyManager.KeyCommand command;
    public KeyManager.KeyPressType pressType;

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork,
        MyObject _target, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        Init(_isDisposableTrigger, _isDiposableAction, _isWork);

        target = _target;
        command = _command;
        pressType = _pressType;
    }

    public override void RefreshTriggerAttribute()
    {
        CmdTypeObject prevKey;
        if(VEasyCalculator.TryGetKey<CmdTypeObject, TriggerForKeyInput>
            (unitTriggerBindingDic, this, out prevKey) == false)
        {
            return;
        }

        CmdType ct = new CmdType(command, pressType);
        CmdTypeObject cto = new CmdTypeObject(ct, target);

        VEasyCalculator.ChangeKey<CmdTypeObject, TriggerForKeyInput>(unitTriggerBindingDic, prevKey, cto);
    }

    void Start()
    {
        if(target == null)
        {
            CustomLog.CompleteLogWarning(this.name + ": target is not set.");
            return;
        }
        if (command == KeyManager.KeyCommand.NONE)
        {
            CustomLog.CompleteLogWarning(this.name + ": command is not set.");
            return;
        }
        if (pressType == KeyManager.KeyPressType.NONE)
        {
            CustomLog.CompleteLogWarning(this.name + ": type is not set.");
            return;
        }

        CmdType ct = new CmdType(command, pressType);

        CmdTypeObject cto = new CmdTypeObject(ct, target);

        unitTriggerBindingDic.Add(cto, this);
    }
}
