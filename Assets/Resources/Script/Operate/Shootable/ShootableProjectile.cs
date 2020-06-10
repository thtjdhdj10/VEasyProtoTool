using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class ShootableProjectile : Shootable
    {
        public Bullet projectile;

        protected override void Shoot()
        {
            GameObject go = VEasyPoolerManager.GetObjectRequest(projectile.name);
            Bullet bullet = go.GetComponent<Bullet>();

            bullet.owner = owner as Unit;
            bullet.transform.position = transform.position;
            bullet.moveDir = owner.targetDir;
            bullet.targetDir = owner.targetDir;
        }
    }
}