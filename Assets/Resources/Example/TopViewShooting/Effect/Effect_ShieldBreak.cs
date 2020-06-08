using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class Effect_ShieldBreak : Effect
    {
        public float startScale;
        public float targetScale;

        public float progress = 0f;

        public void Start()
        {
            StartCoroutine(BreakProcess());
        }

        private IEnumerator BreakProcess()
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            Vector2 fromScale = new Vector2(startScale, startScale);
            Vector2 toScale = new Vector2(targetScale, targetScale);

            Color fromColor = new Color(1f, 1f, 1f, 1f);
            Color toColor = new Color(1f, 1f, 1f, 0.2f);

            Vector2 parentScale = new Vector2(1f, 1f);
            if (transform.parent != null)
                parentScale = transform.parent.lossyScale;

            while (progress < 1f)
            {
                sprite.color = Color.Lerp(fromColor, toColor, progress);
                transform.localScale = Vector2.Lerp(
                    fromScale / parentScale, toScale / parentScale,
                    1f - (1f - progress) * (1f - progress));

                progress += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Destroy(gameObject);
        }
    }
}