using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace VEPT
{
    [CustomEditor(typeof(MovableStraight))]
    [CanEditMultipleObjects]
    public class MovableStraightEditor : MovableEditor
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