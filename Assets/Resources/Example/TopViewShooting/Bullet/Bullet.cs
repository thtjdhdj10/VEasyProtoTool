using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
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
            if (force == EForce.A) targetType = typeof(Enemy);
            else targetType = typeof(Player);

            TrgCollision trgCol = new TrgCollision(this, targetType);
            new ActDealDamage(trgCol, damage);
            new ActDestroyActor(trgCol, this);
        }
    }
}