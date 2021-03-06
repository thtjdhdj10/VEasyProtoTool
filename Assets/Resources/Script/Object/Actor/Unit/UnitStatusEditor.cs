﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace VEPT
{
    [CustomEditor(typeof(UnitStatus))]
    [CanEditMultipleObjects]
    public class UnitStatusEditor : Editor
    {
        SerializedProperty scriptProp;
        SerializedProperty ownerProp;
        SerializedProperty hpProp;
        SerializedProperty currentHpProp;
        SerializedProperty enableVitalColorProp;
        SerializedProperty enableHpDisplayProp;
        SerializedProperty vitalProp;
        SerializedProperty spriteProp;

        UnitStatus obj = null;

        protected virtual void OnEnable()
        {
            obj = target as UnitStatus;

            scriptProp = serializedObject.FindProperty("m_Script");
            ownerProp = serializedObject.FindProperty("owner");
            hpProp = serializedObject.FindProperty("hp");
            currentHpProp = serializedObject.FindProperty("currentHp");
            enableVitalColorProp = serializedObject.FindProperty("enableVitalColor");
            enableHpDisplayProp = serializedObject.FindProperty("enableHpDisplay");
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
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(scriptProp);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(ownerProp);
            EditorGUILayout.PropertyField(hpProp);
            EditorGUILayout.PropertyField(currentHpProp);
            EditorGUILayout.PropertyField(enableVitalColorProp);
            EditorGUILayout.PropertyField(enableHpDisplayProp);

            if (obj.enableVitalColor)
            {
                EditorGUILayout.PropertyField(vitalProp);
                EditorGUILayout.PropertyField(spriteProp);
            }
        }
    }
}