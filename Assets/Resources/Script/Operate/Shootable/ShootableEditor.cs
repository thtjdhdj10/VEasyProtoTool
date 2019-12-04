using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Shootable))]
[CanEditMultipleObjects]
public class ShootableEditor : Editor
{
    Shootable obj = null;

    protected virtual void OnEnable()
    {
        obj = target as Shootable;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ContentsUpdate();

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void ContentsUpdate()
    {
        EditorGUILayout.Space();

        if (obj.isRangeless = EditorGUILayout.Toggle("Is Rangeless", obj.isRangeless))
        {
            obj.range = EditorGUILayout.FloatField("   Range", obj.range);
        }

        obj.damage = EditorGUILayout.IntField("Damage", obj.damage);
        obj.attackDelay = EditorGUILayout.FloatField("Attack Delay", obj.attackDelay);
    }
}
