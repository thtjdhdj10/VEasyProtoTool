using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : SingletonComponent<GUIManager>
{
    public static Canvas MainCanvas
    {
        get
        {
            Instance.Init();
            return Instance.mainCanvas;
        }
    }

    private Canvas mainCanvas;

    protected override void Init()
    {
        if (mainCanvas != null)
            return;

        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var go = new GameObject("MainCanvas");
            canvas = go.AddComponent<Canvas>();
        }
        mainCanvas = canvas;
    }
}
