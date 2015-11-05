using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LayerSetting))]
public class LayerSettingEditor : Editor
{
    bool layerListFold = true;

    public override void OnInspectorGUI()
    {
        //        base.OnInspectorGUI();

        LayerSetting ls = target as LayerSetting;

        Dictionary<int, string> layerNameNumber = new Dictionary<int, string>();

        for (int i = 0; i < 32; i++)
        {
            string name = LayerMask.LayerToName(i);

            if (name.Length > 0)
            {
                layerNameNumber[i] = name;
            }
        }

        //

        EditorGUILayout.Space();

        ls.editRealtime = EditorGUILayout.Toggle("Edit Layer Realtime", ls.editRealtime);

        if (layerListFold = EditorGUILayout.Foldout(layerListFold, "Layers"))
        {
            for (int i = 0; i < 32; ++i)
            {
                if (layerNameNumber.ContainsKey(i) == true)
                {
                    ls.layers[i] = EditorGUILayout.Toggle(layerNameNumber[i], ls.layers[i]);
                }
            }
        }


    }



}
