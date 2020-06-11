using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public float printDelay;
        private float _remainDelay;

        private void Awake()
        {
            var rect = new Vector2(200f, 50f);
            var pos = new Vector2(
                -CameraManager.WorldWidthHalf, CameraManager.WorldHeightHalf);

            var worldRect = VEasyCalc.GuiToWorldPos(rect);

            pos.x += worldRect.x * 0.5f;
            pos.y -= worldRect.y * 0.5f;

            TextDisplayer.AssignNewText(
                gameObject, "", new Color(1, 1, 1), pos, rect);
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
            string str = "";
            for (int i = 0; i < displayStatusList.Count; ++i)
            {
                switch (displayStatusList[i])
                {
                    case EStatus.FPS:
                        {
                            float msec = Time.deltaTime * 1000.0f;
                            float fps = 1.0f / Time.deltaTime;
                            str += string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                        }
                        break;
                    case EStatus.UNIT_COUNT:
                        {
                            str += "UNITS: " + Unit.unitList.Count.ToString();
                        }
                        break;
                }
                str += "\n";
            }

            TextDisplayer.UpdateText(gameObject, str);
        }
    }
}