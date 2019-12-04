using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ShootableProjectile))]
[CanEditMultipleObjects]
public class ShootableProjectileEditor : ShootableEditor
{
    ShootableProjectile obj = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        obj = target as ShootableProjectile;
    }

    protected override void ContentsUpdate()
    {
        base.ContentsUpdate();

        obj.projectile = EditorGUILayout.ObjectField(
            "Projectile", obj.projectile, typeof(Unit), true) as Bullet;
    }
}
