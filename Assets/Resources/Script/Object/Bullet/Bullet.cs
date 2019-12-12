using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Unit
{
    public int damage;

    protected override void Start()
    {
        base.Start();

        SetDefaultBulletSetting();
    }

    protected virtual void SetDefaultBulletSetting()
    {
        System.Type targetType;
        if (force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TriggerCollision trgCol = new TriggerCollision(this, GetOperable<Collidable>(), targetType);
        new ActionDealDamage(trgCol, damage);
        new ActionDestroyUnit(trgCol, this);
    }
}
