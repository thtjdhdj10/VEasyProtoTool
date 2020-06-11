using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VEPT
{
    // 원하는 오브젝트에 붙여서 원하는 값 출력
    public class TextDisplayer
    {
        private Text text;

        private static Dictionary<GameObject, TextDisplayer> textDisplayerDic =
            new Dictionary<GameObject, TextDisplayer>();

        public static void AssignNewText(GameObject root, string str, Color color,
            Vector2 worldPos, Vector2 guiRectSize, TextAnchor alignment)
        {
            var canvas = GUIManager.MainCanvas;
            var go = new GameObject(root.name + " Text");
            var text = go.AddComponent<Text>();

            text.rectTransform.SetParent(canvas.transform);

            text.rectTransform.sizeDelta = new Vector2(guiRectSize.x, guiRectSize.y);
            text.rectTransform.localScale = new Vector3(1, 1, 1);
            text.rectTransform.anchoredPosition = VEasyCalc.WorldToGuiPos(worldPos);

            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.color = color;
            text.text = str;
            text.alignment = alignment;

            textDisplayerDic.Add(root, new TextDisplayer { text = text });
        }

        public static void RemoveText(GameObject key)
        {
            if(textDisplayerDic.ContainsKey(key))
            {
                Object.Destroy(textDisplayerDic[key].text);
                textDisplayerDic.Remove(key);
            }
        }

        public static void UpdateText(GameObject key, string str)
        {
            if(textDisplayerDic.ContainsKey(key))
            {
                textDisplayerDic[key].text.text = str;
            }
        }

        public static void UpdateText(GameObject key, string str, Vector2 worldPos)
        {
            if(textDisplayerDic.ContainsKey(key))
            {
                var text = textDisplayerDic[key].text;
                text.text = str;
                text.rectTransform.anchoredPosition = VEasyCalc.WorldToGuiPos(worldPos);
            }
        }
    }
}