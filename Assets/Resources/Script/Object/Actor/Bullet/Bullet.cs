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

    public virtual void InitTransform(Unit _owner)
    {
        owner = _owner;

        transform.position = owner.transform.position;

        if(owner.TryGetOperable(out Targetable ownerTarget))
        {
            if(TryGetOperable(out Movable move))
            {
                move.direction = ownerTarget.direction;
            }
        }
    }

    protected virtual void SetDefaultBulletSetting()
    {
        System.Type targetType;
        if (force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TriggerCollision trgCol = new TriggerCollision(this, GetOperable<Collidable>(), targetType);
        new ActionDealDamage(trgCol, damage);
        new ActionDestroyActor(trgCol, this);
    }
}
