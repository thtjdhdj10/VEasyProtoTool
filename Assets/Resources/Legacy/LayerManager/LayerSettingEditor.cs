using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LayerSetting))]
[CanEditMultipleObjects]
public class LayerSettingEditor : Editor
{
    SerializedProperty layerProp;
    SerializedProperty nbProp;
    SerializedProperty layerMaskProp;

    string layerListName = "layers";
    string layerMaskName = "layerMask";

    bool layerListFold = true;

    Dictionary<int, string> indexLayerName = new Dictionary<int, string>();

    void OnEnable()
    {
        layerProp = serializedObject.FindProperty(layerListName);
        if(layerProp == null)
            Debug.LogError(layerListName + " is invalid property name");

        layerMaskProp = serializedObject.FindProperty(layerMaskName);

        int idx = 0;

        for (int i = 0; i < LayerManager.MaxLayerCount; i++)
        {
            string name = LayerMask.LayerToName(i);

            if (name.Length > 0)
            {
                indexLayerName[idx++] = name;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var coll = indexLayerName.Keys;
        layerProp.arraySize = coll.Count;

        // target 은 값을 가져오는 용도로만 사용할 것
        LayerSetting ls = target as LayerSetting;
        ls.UpdateLayerMask();

        EditorGUILayout.PropertyField(layerMaskProp);
        ShowList(layerProp);

        serializedObject.ApplyModifiedProperties();

        //Rect r = GUILayoutUtility.GetRect(0f, 16f);
        //bool showNext = EditorGUI.PropertyField(r, layerProp, true);
        //bool hasName = layerProp.NextVisible(showNext);

        //        if (layerListFold = EditorGUILayout.Foldout(layerListFold, "Layers"))

    }

    void ShowList(SerializedProperty prop)
    {
        var coll = indexLayerName.Keys;

        EditorGUILayout.Space();
        
        if(layerListFold = EditorGUILayout.Foldout(layerListFold,new GUIContent("layers")))
        {
            foreach (int key in coll)
            {
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(key),
                    new GUIContent(indexLayerName[key]));
            }
        }
        
    }

}
