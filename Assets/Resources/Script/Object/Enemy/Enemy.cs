using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected override void Awake()
    {
        base.Awake();

        TriggerCollision trgCol = new TriggerCollision(this, typeof(Player));
        ActionDealDamage actDeal = new ActionDealDamage(trgCol, 1);

    }


}
