using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class Bullet_Slayer_1 : Bullet
    {
        public float delay = 0.5f;

        protected override void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(Redirection());
        }

        public IEnumerator Redirection()
        {
            yield return new WaitForSeconds(delay);
            moveDir = owner.targetDir;
            targetDir = owner.targetDir;
        }

    }
}