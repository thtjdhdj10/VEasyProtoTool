using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Actor
{
    public Unit owner;

    public int damage;

    protected override void Start()
    {
        base.Start();

        SetDefaultBulletSetting();
    }

    protected virtual void SetDefaultBulletSetting()
    {
        _force = owner._force;
        System.Type targetType;
        if (_force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TriggerCollision trgCol = new TriggerCollision(this, GetOperable<Collidable>(), targetType);
        new ActionDealDamage(trgCol, damage);
        new ActionDestroyActor(trgCol, this);
    }
}
