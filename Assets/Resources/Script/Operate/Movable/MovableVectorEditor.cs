﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace VEPT
{
    [CustomEditor(typeof(MovableVector))]
    [CanEditMultipleObjects]
    public class MovableVectorEditor : MovableEditor
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