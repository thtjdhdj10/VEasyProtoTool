using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class Effect_Shockwave : Effect
    {
        public Material material;
        private float duration;

        public override void Init()
        {
            base.Init();

            material.SetFloat("_GeneratedTime", Time.time);
            duration = material.GetFloat("_Duration");

            StartCoroutine(EffectProcess());
        }

        private IEnumerator EffectProcess()
        {
            yield return new WaitForSeconds(duration);

            Destroy(gameObject);
        }

    }
}