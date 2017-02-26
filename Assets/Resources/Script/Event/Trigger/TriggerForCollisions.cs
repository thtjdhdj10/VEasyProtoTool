using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerForCollisions : Trigger
{
    static Dictionary<KeyValuePair<System.Type, System.Type>,
        TriggerForCollisions> unitTriggerBindingDic
        = new Dictionary<KeyValuePair<System.Type, System.Type>,
            TriggerForCollisions>();

    // Unit 에서 호출하여 Trigger 를 작동시키는 방식.
    public static void UnitEventReceive(Unit hit, Unit beHit)
    {
        KeyValuePair<System.Type, System.Type> kv =
            new KeyValuePair<System.Type, System.Type>(hit.GetType(), beHit.GetType());
        if (unitTriggerBindingDic.ContainsKey(kv) == true)
        {
            unitTriggerBindingDic[kv].ActivateTrigger();
        }
    }

    public Type type;

    public Unit hit;
    public Unit beHit;

    public enum Type
    {
        NONE = 0,
        HIT,
        BEHIT,
    }

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork,
        Type _type, Unit _hit, Unit _beHit)
    {
        Init(_isDisposableTrigger, _isDiposableAction, _isWork);

        type = _type;
        hit = _hit;
        beHit = _beHit;
    }

    public override void RefreshTriggerAttribute()
    {
        KeyValuePair<System.Type, System.Type> prevKey;
        if (VEasyCalculator.TryGetKey<KeyValuePair<System.Type, System.Type>, TriggerForCollisions>
            (unitTriggerBindingDic, this, out prevKey) == false)
        {
            return;
        }

        KeyValuePair<System.Type, System.Type> tt =
            new KeyValuePair<System.Type, System.Type>(hit.GetType(), beHit.GetType());

        VEasyCalculator.ChangeKey<KeyValuePair<System.Type, System.Type>,
            TriggerForCollisions>(unitTriggerBindingDic, prevKey, tt);
    }

    void Start()
    {
        if (type == Type.NONE)
        {
            CustomLog.CompleteLogWarning(this.name + ": type is not set.");
        }

        if (hit != null)
        {
            KeyValuePair<System.Type, System.Type> kv =
                new KeyValuePair<System.Type, System.Type>(hit.GetType(), beHit.GetType());
            unitTriggerBindingDic.Add(kv, this);
        }
        else
        {
            CustomLog.CompleteLogWarning(this.name + ": target unit is not set.");
        }
    }
}
