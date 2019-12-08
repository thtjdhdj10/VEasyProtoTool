using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Slayer_1 : Bullet
{
    public Unit owner;

    public float delay = 0.5f;

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(Redirection());
    }

    public IEnumerator Redirection()
    {
        yield return new WaitForSeconds(delay);
        direction = owner.direction;
    }

}
