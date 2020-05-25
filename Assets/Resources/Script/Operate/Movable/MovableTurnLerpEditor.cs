using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(MovableTurnLerp))]
[CanEditMultipleObjects]
public class MovableTurnLerpEditor : MovableEditor
{
    SerializedProperty turnFactorProp;

    protected override void OnEnable()
    {
        base.OnEnable();

        turnFactorProp = serializedObject.FindProperty("turnFactor");
    }

    protected override void ContentsUpdate()
    {
        base.ContentsUpdate();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(turnFactorProp);
    }
}
