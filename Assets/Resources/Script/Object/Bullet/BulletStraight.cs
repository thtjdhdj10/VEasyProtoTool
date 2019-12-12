using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵 나가면 삭제
public class BulletStraight : Bullet
{
    protected override void Start()
    {
        base.Start();

        System.Type targetType;
        if (force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TriggerCollision trgCol = new TriggerCollision(this, GetOperable<Collidable>(), targetType);
        new ActionDestroyUnit(trgCol, this);
        new ActionDealDamage(trgCol, damage);
        new ActionPrintLog(trgCol, "Bullet Collision!");
    }

}
