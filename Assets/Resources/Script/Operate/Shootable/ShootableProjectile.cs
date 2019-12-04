using UnityEngine;
using System.Collections.Generic;

public class ShootableProjectile : Shootable
{
    public Bullet projectile;
    protected override void Shoot()
    {
        Bullet bulletScript = Instantiate(projectile);
        Movable bulletMove = bulletScript.GetOperable<Movable>();

        bulletScript.transform.position = transform.position;
        if (fireToTarget &&
            target != null)
        {
            bulletMove.target = target;
        }
        else
        {
            bulletMove.direction = fireDirection;
        }
    }
}
