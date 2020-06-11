using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class BulletStraight : Bullet
    {
        protected override void SetDefaultBulletSetting()
        {
            force = owner.force;
            System.Type targetType;
            if (force == EForce.A) targetType = typeof(Enemy);
            else targetType = typeof(Player);

            TrgCollision trgCol = new TrgCollision(this, targetType);
            new ActDealDamage(trgCol, damage);
            new ActDestroyActor(trgCol, this);
            new ActCreateObjectDynamic(trgCol, EResourceName.Effect_BulletSpark.ToString(), transform);
        }
    }
}