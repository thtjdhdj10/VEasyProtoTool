using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵 나가면 삭제
public class BulleStraight : Bullet
{
    protected override void Start()
    {
        base.Start();

        System.Type targetType;
        if (force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        // player bullet이랑 충돌처리안되게
        // bullet끼리 충돌안되게
        TriggerCollision trgCol = new TriggerCollision(this, targetType);
        new ActionDestroyUnit(trgCol, this);
        new ActionDealDamage(trgCol, damage);
        new ActionPrintLog(trgCol, "Bullet Collision!");
    }

}
