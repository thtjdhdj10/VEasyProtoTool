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

        obj.active = EditorGUILayout.Toggle("Activate", obj.active);

        if (!(obj.isRangeless = EditorGUILayout.Toggle("Is Rangeless", obj.isRangeless)))
        {
            obj.range = EditorGUILayout.FloatField("   Range", obj.range);
        }

        obj.damage = EditorGUILayout.IntField("Damage", obj.damage);
        obj.attackDelay = EditorGUILayout.FloatField("Attack Delay", obj.attackDelay);
        obj.loadOnDeactive = EditorGUILayout.Toggle("Load on Deactive", obj.loadOnDeactive);

        if (obj.fireToTarget = EditorGUILayout.Toggle("Fire To Target", obj.fireToTarget))
        {
            obj.target = EditorGUILayout.ObjectField("  Target", obj.target, typeof(Unit), true) as Unit;
        }
        else
        {
            obj.fireDirection = EditorGUILayout.FloatField("    Fire Direction", obj.fireDirection);
        }
    }
}
