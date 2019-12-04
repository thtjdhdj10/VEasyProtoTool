using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ShootableProjectile))]
[CanEditMultipleObjects]
public class ShootableProjectileEditor : ShootableEditor
{
    SerializedProperty projectileProp;

    ShootableProjectile subObj = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        subObj = target as ShootableProjectile;

        projectileProp = serializedObject.FindProperty("projectile");
    }

    protected override void ContentsUpdate()
    {
        base.ContentsUpdate();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(projectileProp);
    }
}