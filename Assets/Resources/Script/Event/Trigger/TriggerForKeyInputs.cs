using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TriggerForKeyInput 와 달리 정해진 Object 에의 모든 키 입력에서 Activate() 호출
// Action 내에서 명령어를 선별해서 사용.
// 하나의 Action 에 복수의 명령어가 입력될 수 있을 때 사용할 것.

public class TriggerForKeyInputs : Trigger
{
    static Dictionary<MyObject, TriggerForKeyInputs> unitTriggerBindingDic
        = new Dictionary<MyObject, TriggerForKeyInputs>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(
        MyObject obj, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        if (unitTriggerBindingDic.ContainsKey(obj) == true)
        {
            unitTriggerBindingDic[obj].command = _command;
            unitTriggerBindingDic[obj].pressType = _pressType;
            unitTriggerBindingDic[obj].ActivateTrigger();
        }
    }

    public MyObject target;

    public KeyManager.KeyCommand command;
    public KeyManager.KeyPressType pressType;

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork,
        MyObject _target)
    {
        Init(_isDisposableTrigger, _isDiposableAction, _isWork);

        target = _target;
    }

    public override void RefreshTriggerAttribute()
    {
        MyObject prevKey;
        if (VEasyCalculator.TryGetKey<MyObject, TriggerForKeyInputs>
            (unitTriggerBindingDic, this, out prevKey) == false)
        {
            return;
        }

        VEasyCalculator.ChangeKey<MyObject, TriggerForKeyInputs>(unitTriggerBindingDic, prevKey, target);
    }

    void Start()
    {
        if (target == null)
        {
            CustomLog.CompleteLogWarning(this.name + ": target is not set.");
            return;
        }

        unitTriggerBindingDic.Add(target, this);
    }
}
