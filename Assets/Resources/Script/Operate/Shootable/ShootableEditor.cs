using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Shootable))]
[CanEditMultipleObjects]
public class ShootableEditor : Editor
{
    SerializedProperty scriptProp;
    SerializedProperty stateProp;
    SerializedProperty conditionProp;
    SerializedProperty attackDelayProp;
    SerializedProperty remainAttackDelayProp;
    SerializedProperty loadOnDeactiveProp;

    Shootable obj = null;

    protected virtual void OnEnable()
    {
        obj = target as Shootable;

        scriptProp = serializedObject.FindProperty("m_Script");
        stateProp = serializedObject.FindProperty("state.state");
        conditionProp = serializedObject.FindProperty("state.conditionForTrue");
        attackDelayProp = serializedObject.FindProperty("attackDelay");
        remainAttackDelayProp = serializedObject.FindProperty("remainAttackDelay");
        loadOnDeactiveProp = serializedObject.FindProperty("loadOnDeactive");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ContentsUpdate();

        serializedObject.ApplyModifiedProperties();
    }

    private bool fold = true;
    protected virtual void ContentsUpdate()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(scriptProp);
        EditorGUI.EndDisabledGroup();

        fold = EditorGUILayout.BeginFoldoutHeaderGroup(fold, "Active");
        if (fold)
        {
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(stateProp);
            EditorGUILayout.PropertyField(conditionProp);
            EditorGUI.indentLevel -= 1;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.PropertyField(attackDelayProp);
        EditorGUILayout.PropertyField(remainAttackDelayProp);
        EditorGUILayout.PropertyField(loadOnDeactiveProp);
    }
}
