using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Shootable))]
[CanEditMultipleObjects]
public class ShootableEditor : Editor
{
    SerializedProperty activeProp;
    SerializedProperty isRangelessProp;
    SerializedProperty rangeProp;
    SerializedProperty damageProp;
    SerializedProperty attackDelayProp;
    SerializedProperty remainAttackDelayProp;
    SerializedProperty loadOnDeactiveProp;
    SerializedProperty fireToTargetProp;
    SerializedProperty targetProp;
    SerializedProperty targetingEachFireProp;
    SerializedProperty fireDirectionProp;

    Shootable obj = null;

    protected virtual void OnEnable()
    {
        obj = target as Shootable;

        activeProp = serializedObject.FindProperty("active");
        isRangelessProp = serializedObject.FindProperty("isRangeless");
        rangeProp = serializedObject.FindProperty("range");
        damageProp = serializedObject.FindProperty("damage");
        attackDelayProp = serializedObject.FindProperty("attackDelay");
        remainAttackDelayProp = serializedObject.FindProperty("remainAttackDelay");
        loadOnDeactiveProp = serializedObject.FindProperty("loadOnDeactive");
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

    protected virtual void ContentsUpdate()
    {
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(activeProp);
        EditorGUILayout.PropertyField(isRangelessProp);

        if (!obj.isRangeless)
        {
            EditorGUI.indentLevel += 2;

            EditorGUILayout.PropertyField(rangeProp);

            EditorGUI.indentLevel -= 2;
        }

        EditorGUILayout.PropertyField(damageProp);
        EditorGUILayout.PropertyField(attackDelayProp);
        EditorGUILayout.PropertyField(remainAttackDelayProp);
        EditorGUILayout.PropertyField(loadOnDeactiveProp);
        EditorGUILayout.PropertyField(fireToTargetProp);

        if (obj.fireToTarget)
        {
            EditorGUI.indentLevel += 2;

            EditorGUILayout.PropertyField(targetProp);
            EditorGUILayout.PropertyField(targetingEachFireProp);

            EditorGUI.indentLevel -= 2;
        }
        else
        {
            EditorGUI.indentLevel += 2;

            EditorGUILayout.PropertyField(fireDirectionProp);

            EditorGUI.indentLevel -= 2;
        }
    }
}
