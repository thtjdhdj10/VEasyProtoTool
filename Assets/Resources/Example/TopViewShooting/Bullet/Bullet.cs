using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class Bullet : Actor
    {
        public Unit owner;
        public int damage;

        public virtual void SetDefaultBulletSetting(Unit _owner)
        {
            owner = _owner;

            transform.position = owner.transform.position;
            moveDir = owner.targetDir;
            targetDir = owner.targetDir;
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