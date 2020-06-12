using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class ShootableProjectile : Shootable
    {
        public Bullet projectile;

        protected override void Shoot()
        {
            GameObject go = ObjectPoolerManager.GetObjectRequest(projectile.name);
            Bullet bullet = go.GetComponent<Bullet>();
            bullet.SetDefaultBulletSetting(owner as Unit);  
        }
    }
}