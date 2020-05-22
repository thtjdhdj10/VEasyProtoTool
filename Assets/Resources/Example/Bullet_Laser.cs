using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Laser : Bullet
{
    public int length = 10;
    public float scale = 1f;
    public float duration = 1.5f;

    public float direction;

    public GameObject laserRoot;
    public GameObject laserBody;

    // TODO 충돌시간 당 데미지

    protected override void SetDefaultBulletSetting()
    {
        force = owner.force;
        System.Type targetType;
        if (force == EForce.A) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        BoxCollider2D col = GetOperable<Collidable>().collider as BoxCollider2D;

        col.size = new Vector2(scale * length, scale);

        col.offset = new Vector2(scale * 0.5f, 0f);

        GameObject root = Instantiate(laserRoot);
        root.transform.position = transform.position;
        root.transform.rotation = transform.rotation;
        root.GetComponent<EffectAnimDestroy>().duration = duration;

        GameObject[] bodies = new GameObject[length - 1];

        for (int i = 0; i < length; ++i)
        {
            bodies[i] = Instantiate(laserBody);
            bodies[i].transform.rotation = transform.rotation;
            Vector2 bodyPos = VEasyCalc.GetRotatedPosition(direction, (i + 1) * scale);
            bodies[i].transform.position = bodyPos;
            bodies[i].GetComponent<EffectAnimDestroy>().duration = duration;
        }
    }


}
