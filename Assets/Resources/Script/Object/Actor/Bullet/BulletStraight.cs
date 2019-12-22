using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵 나가면 삭제
public class BulletStraight : Bullet
{
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (GameManager.isQuitting == false)
        {

            GameObject effectPrefab = ResourcesManager<GameObject>.LoadResource(
            ResourcesManager<GameObject>.ResourceName.Effect_Bullet);

            GameObject effect = Instantiate(effectPrefab);

            effect.transform.position = transform.position;
            effect.transform.rotation = transform.rotation;
        }
    }
}
