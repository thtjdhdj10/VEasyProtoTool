using UnityEngine;
using System.Collections.Generic;

public class GameStatusDisplay : MonoBehaviour
{
    Vector2 rectSize;

    int fontSize;

    public TextAnchor fontAlignment;

    public Color fontColor;

    Vector2 rectPositionCursor;

    TextDisplay unitCountText;
    TextDisplay fpsText;

    void Awake()
    {
        rectPositionCursor = new Vector2(0, 0);

        int w = Screen.width, h = Screen.height;

        fontSize = h * 5 / 100;

        rectSize = new Vector2(w, fontSize);

        //fontAlignment = TextAnchor.UpperRight;

        //fontColor = new Color(1f, 0f, 0f, 1f);

        fpsText = AddText("");
        unitCountText = AddText("");
    }

    void Update()
    {
        float msec = Time.deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        fpsText.text = text;

        unitCountText.text = Unit.unitList.Count.ToString();
    }
    
    // text 를 갱신하려면, 인자의 text 를 수정할 것.
    public TextDisplay AddText(string text)
    {
        TextDisplay td = gameObject.AddComponent<TextDisplay>();

        td.Init(text, rectPositionCursor, rectSize,
            fontSize, fontAlignment, fontColor);
        
        rectPositionCursor.y += fontSize;

        return td;
    }
}