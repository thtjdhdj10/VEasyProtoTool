﻿using UnityEngine;
using System.Collections.Generic;

public class ShootableProjectile : Shootable
{
    public Bullet projectile;

    protected override void Shoot()
    {
        Bullet bullet = Instantiate(projectile);
        bullet.transform.position = transform.position;
        bullet.moveDirection = owner.targetDirection;
        bullet.targetDirection = owner.targetDirection;
    }
}
