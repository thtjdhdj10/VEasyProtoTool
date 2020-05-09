using UnityEngine;
using System.Collections.Generic;

public class ShootableProjectile : Shootable
{
    public Bullet projectile;

    protected override void Shoot()
    {
        Bullet bullet = Instantiate(projectile);
        bullet.owner = owner as Unit;
        bullet.transform.position = transform.position;
        bullet._moveDirection = owner._targetDirection;
        bullet._targetDirection = owner._targetDirection;
    }
}
