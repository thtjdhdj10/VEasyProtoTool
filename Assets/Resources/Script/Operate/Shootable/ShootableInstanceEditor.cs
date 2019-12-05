using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ShootableInstance))]
[CanEditMultipleObjects]
public class ShootableInstanceEditor : ShootableEditor
{
    SerializedProperty damageProp;

    ShootableInstance subObj = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        subObj = target as ShootableInstance;

        damageProp = serializedObject.FindProperty("damage");
    }

    protected override void ContentsUpdate()
    {
        base.ContentsUpdate();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(damageProp);
    }
}
