using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for(int i = 0; i < 3; ++i)
        {
            yield return new WaitForSeconds(term);
            moveDirection += 30f;
            targetDirection += 30f;
        }
    }
}
