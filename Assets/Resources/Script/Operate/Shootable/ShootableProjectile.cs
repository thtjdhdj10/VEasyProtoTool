using UnityEngine;
using System.Collections.Generic;

public class ShootableProjectile : Shootable
{
    public Bullet projectile;

    protected override void Shoot()
    {
        if (target == null ||
            projectile == null)
        {
            return;
        }

        Bullet bulletScript = Instantiate(projectile);

//        Movable m = bullet.GetComponent<Movable>();
        //if(p == null ||
        //    m == null)
        //{
        //    return;
        //}
    }
}
