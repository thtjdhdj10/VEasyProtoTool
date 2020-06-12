using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class EffectAnimDestroy : Effect
    {
        public float duration = 1.5f;
        public float animSpeed = 1f;

        public SpriteRenderer sprite;
        public Animator anim;

        public override void Init()
        {
            base.Init();

            // TODO editor에서 getcomponent하게
            if (sprite == null) sprite = GetComponent<SpriteRenderer>();
            if (anim == null) anim = GetComponent<Animator>();

            anim.speed = animSpeed;

            StartCoroutine(EffectProcess());
        }

        private IEnumerator EffectProcess()
        {
            yield return new WaitForSeconds(duration);

            Destroy(gameObject);
        }
    }

}