using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Collidable))]
[CanEditMultipleObjects]
public class CollidableEditor : Editor
{
    Collidable selected = null;

    void OnEnable()
    {
        selected = target as Collidable;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();

        selected.colType = (Collidable.ColliderType)EditorGUILayout.EnumPopup(selected.colType);

        switch (selected.colType)
        {
            case Collidable.ColliderType.CIRCLE:
                selected.radius = EditorGUILayout.FloatField("Radius", selected.radius);
                break;
            case Collidable.ColliderType.RECT:
                selected.rect = EditorGUILayout.Vector2Field("Rect", selected.rect);
                break;
        }

        serializedObject.ApplyModifiedProperties();


    }
}
