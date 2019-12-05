using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(UnitStatus))]
[CanEditMultipleObjects]
public class UnitStatusEditor : Editor
{
    SerializedProperty ownerProp;
    SerializedProperty hpProp;
    SerializedProperty currentHpProp;
    SerializedProperty enableVitalColorProp;
    SerializedProperty vitalProp;
    SerializedProperty spriteProp;

    UnitStatus obj = null;

    protected virtual void OnEnable()
    {
        obj = target as UnitStatus;

        ownerProp = serializedObject.FindProperty("owner");
        hpProp = serializedObject.FindProperty("hp");
        currentHpProp = serializedObject.FindProperty("currentHp");
        enableVitalColorProp = serializedObject.FindProperty("enableVitalColor");
        vitalProp = serializedObject.FindProperty("vital");
        spriteProp = serializedObject.FindProperty("sprite");
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

        EditorGUILayout.PropertyField(ownerProp);
        EditorGUILayout.PropertyField(hpProp);
        EditorGUILayout.PropertyField(currentHpProp);
        EditorGUILayout.PropertyField(enableVitalColorProp);

        if(obj.enableVitalColor)
        {
            EditorGUILayout.PropertyField(vitalProp);
            EditorGUILayout.PropertyField(spriteProp);
        }
    }


}
