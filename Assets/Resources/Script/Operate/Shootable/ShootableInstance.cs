using UnityEngine;
using System.Collections.Generic;

public class ShootableInstance : Shootable
{
    public int damage;

    protected override void Shoot()
    {
        if(target == null) return;
        if(target.unitStatus == null) return;

        target.unitStatus.CurrentHp -= damage;

    }
}
