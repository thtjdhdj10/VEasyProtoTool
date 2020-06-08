using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class EffectParticleDestroy : Effect
    {
        public float duration = 0f;

        public ParticleSystem particle;

        private void Awake()
        {
            duration = particle.main.startLifetime.constantMax;

            StartCoroutine(ParticleProcess());
        }

        private IEnumerator ParticleProcess()
        {
            yield return new WaitForSeconds(duration);

            Destroy(gameObject);
        }
    }
}