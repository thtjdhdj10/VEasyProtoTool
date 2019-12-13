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
    SerializedProperty isRangelessProp;
    SerializedProperty rangeProp;
    SerializedProperty attackDelayProp;
    SerializedProperty remainAttackDelayProp;
    SerializedProperty loadOnDeactiveProp;
    SerializedProperty followUnitTargetProp;
    SerializedProperty fireToTargetProp;
    SerializedProperty targetProp;
    SerializedProperty targetingEachFireProp;
    SerializedProperty fireDirectionProp;

    Shootable obj = null;

    protected virtual void OnEnable()
    {
        obj = target as Shootable;

        scriptProp = serializedObject.FindProperty("m_Script");
        stateProp = serializedObject.FindProperty("state.state");
        conditionProp = serializedObject.FindProperty("state.conditionForTrue");
        isRangelessProp = serializedObject.FindProperty("isRangeless");
        rangeProp = serializedObject.FindProperty("range");
        attackDelayProp = serializedObject.FindProperty("attackDelay");
        remainAttackDelayProp = serializedObject.FindProperty("remainAttackDelay");
        loadOnDeactiveProp = serializedObject.FindProperty("loadOnDeactive");
        followUnitTargetProp = serializedObject.FindProperty("followUnitTarget");
        fireToTargetProp = serializedObject.FindProperty("fireToTarget");
        targetProp = serializedObject.FindProperty("target");
        targetingEachFireProp = serializedObject.FindProperty("targetingEachFire");
        fireDirectionProp = serializedObject.FindProperty("fireDirection");
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
        EditorGUILayout.PropertyField(scriptProp);

        fold = EditorGUILayout.BeginFoldoutHeaderGroup(fold, "Active");
        if (fold)
        {
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(stateProp);
            EditorGUILayout.PropertyField(conditionProp);
            EditorGUI.indentLevel -= 1;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //EditorGUILayout.PropertyField(isRangelessProp);

        //if (!obj.isRangeless)
        //{
        //    EditorGUI.indentLevel += 1;
        //    EditorGUILayout.PropertyField(rangeProp);
        //    EditorGUI.indentLevel -= 1;
        //}

        EditorGUILayout.PropertyField(attackDelayProp);
        EditorGUILayout.PropertyField(remainAttackDelayProp);
        EditorGUILayout.PropertyField(loadOnDeactiveProp);
//        EditorGUILayout.PropertyField(fireToTargetProp);

        //if (!obj.followUnitTarget)
        //{
        //    EditorGUI.indentLevel += 1;
        //    if (obj.fireToTarget)
        //    {
        //        EditorGUI.indentLevel += 1;
        //        EditorGUILayout.PropertyField(targetProp);
        //        EditorGUILayout.PropertyField(targetingEachFireProp);
        //        EditorGUI.indentLevel -= 1;
        //    }
        //    else
        //    {
        //        EditorGUI.indentLevel += 1;
        //        EditorGUILayout.PropertyField(fireDirectionProp);
        //        EditorGUI.indentLevel -= 1;
        //    }
        //    EditorGUI.indentLevel -= 1;
        //}
    }
}
