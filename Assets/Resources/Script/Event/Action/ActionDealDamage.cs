using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDealDamage : Action
{
    public int damage;

    public ActionDealDamage(int _damage)
    {
        damage = _damage;
    }

    public override void Activate(Trigger trigger)
    {
        if (trigger is TriggerCollision == false) return;

        TriggerCollision trgCol = trigger as TriggerCollision;

        if (trgCol.target == null) return;
        if (trgCol.target.unitStatus == null) return;

        trgCol.target.unitStatus.CurrentHp -= damage;
    }

}
