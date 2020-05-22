using System.Collections.Generic;
using UnityEngine;

// 원하는 오브젝트에 붙여서 원하는 값 출력
public class TextDisplay : MonoBehaviour
{
    const int MAX_TEXT_LENGTH = 300;

    public static Dictionary<GameObject, TextDisplay> goTextDic =
        new Dictionary<GameObject, TextDisplay>();

    public Dictionary<string, string> keywordTextDic = new Dictionary<string, string>();
    public Color fontColor = new Color(1f, 0f, 0f, 1f);
    public int fontSize;

    private Vector2 _relativePos = new Vector2(0, -30);
    private GUIStyle _style = new GUIStyle();

    private void Awake()
    {
        goTextDic.Add(gameObject, this);

        int w = Screen.width, h = Screen.height;

        if (fontSize == 0) fontSize = h * 3 / 100;

        _style.fontSize = fontSize;
        _style.normal.textColor = fontColor;
        _style.alignment = TextAnchor.MiddleCenter;
    }

    public static bool TextUpdate(GameObject go, string keyword, string text)
    {
        if (goTextDic.ContainsKey(go))
        {
            if (goTextDic[go].keywordTextDic.ContainsKey(keyword))
            {
                goTextDic[go].keywordTextDic[keyword] = text;
            }
            else
            {
                goTextDic[go].keywordTextDic.Add(keyword, text);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnGUI()
    {
        if (keywordTextDic.Count == 0) return;

        Rect rect = new Rect();

        Vector2 size = new Vector2(MAX_TEXT_LENGTH, fontSize);
        Vector2 position = VEasyCalc.WorldToGUIPos(transform.position);
        position.x = position.x - size.x / 2;
        position.y = position.y - size.y / 2;
        position += _relativePos;
        rect.size = size;
        rect.position = position;

        foreach(var text in keywordTextDic.Values)
        {
            GUI.Label(rect, text, _style);
            rect.y += fontSize;
        }
    }
}
