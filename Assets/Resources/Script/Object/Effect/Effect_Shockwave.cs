using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Shockwave : Effect
{
    public Material material;
    private float duration;

    private void Awake()
    {
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
