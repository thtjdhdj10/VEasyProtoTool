using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Boom : Bullet
{
    protected override void SetDefaultBulletSetting()
    {
        force = owner.force;
        System.Type targetType;
        if (force == EForce.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        // TODO
    }


}
