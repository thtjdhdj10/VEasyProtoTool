using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    // 게임 상태 출력
    public class GameStatusDisplay : MonoBehaviour
    {
        public enum EStatus
        {
            FPS,
            UNIT_COUNT,
        }

        public List<EStatus> displayStatusList = new List<EStatus>();
        public TextAnchor fontAlignment = TextAnchor.UpperLeft;
        public Color fontColor = new Color(1f, 0f, 0f, 1f);
        public float printDelay;
        public int fontSize;

        private List<Rect> rectList = new List<Rect>();
        private List<string> textList = new List<string>();
        private GUIStyle _style = new GUIStyle();
        private float _remainDelay;

        private void Awake()
        {
            int w = Screen.width, h = Screen.height;

            if (fontSize == 0) fontSize = h * 3 / 100;

            _style.alignment = fontAlignment;
            _style.fontSize = fontSize;
            _style.normal.textColor = fontColor;

            Rect rect = new Rect
            {
                size = new Vector2(w, fontSize)
            };

            for (int i = 0; i < displayStatusList.Count; ++i)
            {
                rectList.Add(rect);
                textList.Add("");
                rect.y += fontSize;
            }
        }

        private void Update()
        {
            DelayedUpdate();
        }

        private void DelayedUpdate()
        {
            if (_remainDelay > 0f)
            {
                _remainDelay -= Time.deltaTime;
            }
            else
            {
                TextUpdate();
                _remainDelay = printDelay;
            }
        }

        private void TextUpdate()
        {
            for (int i = 0; i < displayStatusList.Count; ++i)
            {
                switch (displayStatusList[i])
                {
                    case EStatus.FPS:
                        {
                            float msec = Time.deltaTime * 1000.0f;
                            float fps = 1.0f / Time.deltaTime;
                            textList[i] = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                        }
                        break;
                    case EStatus.UNIT_COUNT:
                        {
                            textList[i] = "UNITS: " + Unit.unitList.Count.ToString();
                        }
                        break;
                }
            }
        }

        private void OnGUI()
        {
            for (int i = 0; i < displayStatusList.Count; ++i)
                GUI.Label(rectList[i], textList[i], _style);
        }
    }
}