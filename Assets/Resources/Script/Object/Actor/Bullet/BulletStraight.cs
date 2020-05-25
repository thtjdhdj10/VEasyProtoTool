using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStraight : Bullet
{
    protected override void SetDefaultBulletSetting()
    {
        force = owner.force;
        System.Type targetType;
        if (force == EForce.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        GameObject effectPrefab = ResourcesManager.LoadResource<GameObject>(
            ResourcesManager.EResName.Effect_Bullet);

        TrgCollision trgCol = new TrgCollision(this, GetOperable<Collidable>(), targetType);
        new ActDealDamage(trgCol, damage);
        new ActDestroyActor(trgCol, this);
        new ActCreateObjectDynamic(trgCol, effectPrefab, transform);
    }
}
