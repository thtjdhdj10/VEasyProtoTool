using UnityEngine;
using System.Collections;

public class TextDisplay : MonoBehaviour
{
    public Vector2 rectPosition;
    public Vector2 rectSize;

    public int fontSize;

    public TextAnchor fontAlignment;

    public Color fontColor;

    public string text = "";

    GUIStyle style;
    Rect rect;

    void Awake()
    {
        rect = new Rect();
        style = new GUIStyle();
    }

    public void Init(string _text, Vector2 _rectPosition, Vector2 _rectSize)
    {
        text = _text;

        int w = Screen.width, h = Screen.height;
        int size = h * 5 / 100;

        rectPosition = _rectPosition;
        rectSize = _rectSize;

        fontAlignment = TextAnchor.UpperRight;
        fontSize = size;
        fontColor = new Color(1f, 0f, 0f, 1f);
    }

    public void Init(string _text, Vector2 _rectPosition, Vector2 _rectSize,
        int _fontSize, TextAnchor _fontAlignment, Color _fontColor)
    {
        Init(_text, _rectPosition, _rectSize);
        fontAlignment = _fontAlignment;
        fontSize = _fontSize;
        fontColor = _fontColor;
    }

    void OnGUI()
    {
        rect.x = rectPosition.x;
        rect.y = rectPosition.y;
        rect.width = rectSize.x;
        rect.height = rectSize.y;

        style.alignment = fontAlignment;
        style.fontSize = fontSize;
        style.normal.textColor = fontColor;

        string str = string.Format(text);

        GUI.Label(rect, str, style);
    }
}
