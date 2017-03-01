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

        if(target.unitStatus == null)
        {
            return;
        }

        target.unitStatus.CurrentHp -= damage;

    }
}
