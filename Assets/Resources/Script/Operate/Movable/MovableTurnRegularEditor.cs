using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace VEPT
{
    [CustomEditor(typeof(MovableTurnRegular))]
    [CanEditMultipleObjects]
    public class MovableTurnRegularEditor : MovableEditor
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
}