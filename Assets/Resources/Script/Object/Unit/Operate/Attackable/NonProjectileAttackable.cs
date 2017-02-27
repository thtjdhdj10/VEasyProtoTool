using UnityEngine;
using System.Collections.Generic;

public class NonProjectileAttackable : Attackable
{



    protected override void Shoot()
    {
        if(target == null)
        {
            return;
        }

        target.currentHp -= damage;

    }
}
