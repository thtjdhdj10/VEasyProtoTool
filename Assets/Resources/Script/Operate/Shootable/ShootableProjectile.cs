using UnityEngine;
using System.Collections.Generic;

public class ShootableProjectile : Shootable
{
    public Bullet projectile;
    protected override void Shoot()
    {
        Bullet bullet = Instantiate(projectile);
        Movable bulletMove = bullet.GetOperable<Movable>();

        bullet.transform.position = transform.position;
        if (fireToTarget &&
            target != null)
        {
            bulletMove.target = target;
        }
        else
        {
            bullet.direction = fireDirection;
        }
    }
}
