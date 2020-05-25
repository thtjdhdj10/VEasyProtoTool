using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Movable))]
[CanEditMultipleObjects]
public class MovableEditor : Editor
{
    SerializedProperty scriptProp;
    SerializedProperty speedProp;

    Movable obj = null;

    protected virtual void OnEnable()
    {
        obj = target as Movable;

        scriptProp = serializedObject.FindProperty("m_Script");
        speedProp = serializedObject.FindProperty("speed");
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
        GUI.color = new Color(0.7f, 0.7f, 1f);
        EditorGUILayout.PropertyField(scriptProp);
        GUI.color = Color.white;
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(speedProp);

        obj._enableBounceRef.value = EditorGUILayout.Toggle("Enable Bounce", obj._enableBounceRef.value);

        if (obj._enableBounceRef.value)
        {
            List<Movable.EBounceTrigger> triggers = obj._bounceTriggerList;
            List<Movable.EBounceAction> actions = obj._bounceActionList;

            int count = triggers.Count;
            count = Mathf.Clamp(EditorGUILayout.IntField("Bounce Action List", count), 0, 3);

            while (count < triggers.Count)
            {
                triggers.RemoveAt(triggers.Count - 1);
                actions.RemoveAt(actions.Count - 1);
            }

            while (count > triggers.Count)
            {
                triggers.Add(Movable.EBounceTrigger.NONE);
                actions.Add(Movable.EBounceAction.NONE);
            }

            EditorGUI.indentLevel += 1;
            for (int i = 0; i < triggers.Count; ++i)
            {
                if (i == 0)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Triggers");
                    EditorGUILayout.LabelField("Actions");
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                Movable.EBounceTrigger trg = (Movable.EBounceTrigger)
                    EditorGUILayout.EnumPopup(triggers[i], GUILayout.Width(120));
                Movable.EBounceAction act = (Movable.EBounceAction)
                    EditorGUILayout.EnumPopup(actions[i], GUILayout.Width(120));
                GUILayout.EndHorizontal();

                triggers[i] = trg;
                actions[i] = act;
            }
            EditorGUI.indentLevel -= 1;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(obj);
        }
    }
}
