using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Paralyze : BulletStraight
{
    public float duration;

    protected override void SetDefaultBulletSetting()
    {
        _force = owner._force;
        System.Type targetType;
        if (_force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TriggerCollision trgCol = new TriggerCollision(this, GetOperable<Collidable>(), targetType);
        new ActionActiveTargetOperable<Movable>(trgCol, Multistat.StateType.STURN, true);
        new ActionActiveTargetOperable<Movable>(trgCol, Multistat.StateType.STURN, false)
        { delay = duration };
        new ActionDealDamage(trgCol, damage);
        new ActionDestroyActor(trgCol, this);
    }


}
