using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TriggerForKeyInput 와 달리 정해진 Object 에의 모든 키 입력에서 Activate() 호출
// Action 내에서 명령어를 선별해서 사용.
// 하나의 Action 에 복수의 명령어가 입력될 수 있을 때 사용할 것.

public class TriggerKeyInputs : Trigger
{
    public TriggerKeyInputs(Unit _owner)
        : base(_owner)
    {
        unitTriggerDic.Add(owner, this);
    }

    static Dictionary<MyObject, TriggerKeyInputs> unitTriggerDic
        = new Dictionary<MyObject, TriggerKeyInputs>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(
        MyObject obj, KeyManager.KeyCommand _command, KeyManager.KeyPressType _pressType)
    {
        if (unitTriggerDic.ContainsKey(obj) == true)
        {
            if(unitTriggerDic[obj].useDebuger == true)
                Debug.Log("Pressed: " + 
                    _command.ToString() + " " + _pressType.ToString());

            unitTriggerDic[obj].command = _command;
            unitTriggerDic[obj].pressType = _pressType;
            unitTriggerDic[obj].ActivateTrigger();
        }
    }

    [System.NonSerialized]
    public MyObject target;

    public bool useDebuger;

    [System.NonSerialized]
    public KeyManager.KeyCommand command;
    [System.NonSerialized]
    public KeyManager.KeyPressType pressType;

    public override void RefreshTriggerAttribute()
    {
        MyObject prevKey;
        if (VEasyCalculator.TryGetKey(unitTriggerDic, this, out prevKey) == false)
            return;

        VEasyCalculator.ChangeKey(unitTriggerDic, prevKey, owner);
    }

    void Start()
    {
    }
}
