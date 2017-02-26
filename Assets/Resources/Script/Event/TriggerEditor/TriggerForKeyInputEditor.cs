using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TriggerForKeyInput))]
[CanEditMultipleObjects]
public class TriggerForKeyInputEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        // target 은 값을 가져오는 용도로만 사용할 것
        TriggerForKeyInput trigger = target as TriggerForKeyInput;

        trigger.RefreshTriggerAttribute();
    }

}
