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
        force = owner.force;
        System.Type targetType;
        if (force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TrgCollision trgCol = new TrgCollision(this, GetOperable<Collidable>(), targetType);
        new ActDealDamage(trgCol, damage);
        new ActDestroyActor(trgCol, this);
    }
}
