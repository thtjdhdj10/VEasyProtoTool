using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class Bullet_Slayer_2 : Bullet
    {
        public float term = 0.5f;

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(Redirection());
        }

        public IEnumerator Redirection()
        {
            for (int i = 0; i < 3; ++i)
            {
                yield return new WaitForSeconds(term);
                moveDir -= 60f;
                targetDir -= 60f;
            }
        }
    }
}