using UnityEngine;
using System.Collections.Generic;

public class ProjectileAttackable : Attackable
{
    public GameObject projectile;

    protected override void Shoot()
    {
        if (target == null ||
            projectile == null)
        {
            return;
        }

        GameObject bullet = VEasyPoolerManager.GetObjectRequest(projectile.name);

        Movable m = bullet.GetComponent<Movable>();
        //if(p == null ||
        //    m == null)
        //{
        //    return;
        //}
    }
}
