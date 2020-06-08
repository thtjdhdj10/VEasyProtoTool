using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace VEPT
{
    [CustomEditor(typeof(ShootableProjectile))]
    [CanEditMultipleObjects]
    public class ShootableProjectileEditor : ShootableEditor
    {
        SerializedProperty projectileProp;

        protected override void OnEnable()
        {
            base.OnEnable();

            projectileProp = serializedObject.FindProperty("projectile");
        }

        protected override void ContentsUpdate()
        {
            base.ContentsUpdate();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(projectileProp);
        }
    }
}