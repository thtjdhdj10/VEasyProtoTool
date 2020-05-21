using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Paralyze : BulletStraight
{
    public float duration;

    protected override void SetDefaultBulletSetting()
    {
        force = owner.force;
        System.Type targetType;
        if (force == EForce.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TrgCollision trgCol = new TrgCollision(this, GetOperable<Collidable>(), targetType);
        new ActActiveTargetOperable<Movable>(trgCol, Multistat.EStateType.STURN, true);
        new ActActiveTargetOperable<Movable>(trgCol, Multistat.EStateType.STURN, false)
        { delay = duration };
        new ActDealDamage(trgCol, damage);
        new ActDestroyActor(trgCol, this);
    }


}
