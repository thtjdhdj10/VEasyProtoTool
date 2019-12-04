using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Collidable))]
[CanEditMultipleObjects]
public class CollidableEditor : Editor
{
    SerializedProperty colTypeProp;
    SerializedProperty radiusProp;
    SerializedProperty rectProp;

    Collidable selected = null;

    void OnEnable()
    {
        selected = target as Collidable;

        colTypeProp = serializedObject.FindProperty("colType");
        radiusProp = serializedObject.FindProperty("radius");
        rectProp = serializedObject.FindProperty("rect");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(colTypeProp);

        switch (selected.colType) // TODO type, 크기에 따라 에디터 화면에 충돌체 크기 보이게 하기
        {
            case Collidable.ColliderType.CIRCLE:
                EditorGUILayout.PropertyField(radiusProp);
                break;
            case Collidable.ColliderType.RECT:
                EditorGUILayout.PropertyField(rectProp);
                break;
        }

        serializedObject.ApplyModifiedProperties();


    }
}
