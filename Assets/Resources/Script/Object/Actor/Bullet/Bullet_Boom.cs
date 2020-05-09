using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Boom : Bullet
{
    protected override void SetDefaultBulletSetting()
    {
        _force = owner._force;
        System.Type targetType;
        if (_force == Force.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        // TODO
    }


}
