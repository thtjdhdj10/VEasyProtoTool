using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵 나가면 삭제
public class BulleStraight : Bullet
{
    protected override void Awake()
    {
        base.Awake();

        System.Type targetType;
        if (force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TriggerCollision trgCol = new TriggerCollision(this, targetType);
//        new ActionDestroyUnit(trgCol, this);
        new ActionDealDamage(trgCol, damage);
    }

}
