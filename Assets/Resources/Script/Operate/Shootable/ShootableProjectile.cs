using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class ShootableProjectile : Shootable
    {
        public Bullet projectile;

        protected override void Shoot()
        {
            Bullet bullet = Instantiate(projectile);
            bullet.owner = owner as Unit;
            bullet.transform.position = transform.position;
            bullet.moveDir = owner.targetDir;
            bullet.targetDir = owner.targetDir;
        }
    }
}