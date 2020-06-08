using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace VEPT
{
    [CustomEditor(typeof(MovableTarget))]
    [CanEditMultipleObjects]
    public class MovableTargetEditor : MovableEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void ContentsUpdate()
        {
            base.ContentsUpdate();
        }
    }
}