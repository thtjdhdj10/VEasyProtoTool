using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Collidable))]
[CanEditMultipleObjects]
public class CollidableEditor : Editor
{
    SerializedProperty collidableProp;

    string colTypeName = "colType";

    void OnEnable()
    {
        collidableProp = serializedObject.FindProperty(colTypeName);
        if (collidableProp == null)
            Debug.LogError(colTypeName + " is invalid property name");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();



    }
}
