using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Shield : Effect
{
    public float startScale;
    public float targetScale;

    public float progress = 0f;

    public void Start()
    {
        StartCoroutine(RegenProcess());
    }

    private IEnumerator RegenProcess()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        Vector2 fromScale = new Vector2(startScale, startScale);
        Vector2 toScale = new Vector2(targetScale, targetScale);

        Color fromColor = new Color(1f, 1f, 1f, 0.2f);
        Color toColor = new Color(1f, 1f, 1f, 1f);

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

        sprite.color = toColor;
        transform.localScale = toScale / parentScale;
    }

    private bool isQuitting = false;
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (isQuitting == false)
        {
            GameObject go = Instantiate(ResourcesManager<GameObject>.LoadResource(
                ResourcesManager<GameObject>.ResourceName.Effect_ShieldBreak));
            go.transform.parent = transform.parent;
            go.transform.position = transform.position;
        }
    }
}
