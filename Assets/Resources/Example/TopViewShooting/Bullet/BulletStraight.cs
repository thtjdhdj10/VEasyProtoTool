﻿using System.Collections;
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

            GameObject effectPrefab = ResourcesManager.LoadResource<GameObject>(
                EResourceName.Effect_Bullet);

            TrgCollision trgCol = new TrgCollision(this, targetType);
            new ActDealDamage(trgCol, damage);
            new ActDestroyActor(trgCol, this);
            new ActCreateObjectDynamic(trgCol, effectPrefab, transform);
        }
    }
}