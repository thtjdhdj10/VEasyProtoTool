using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class ShootableInstance : Shootable
    {
        public int damage;

        protected override void Shoot()
        {
            //if(owner.target == null) return;
            //if(owner.target.unitStatus == null) return;

            //owner.target.unitStatus.CurrentHp -= damage;

        }
    }
}