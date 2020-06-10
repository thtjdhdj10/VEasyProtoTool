using UnityEngine;
using System.Collections;
using System;

namespace VEPT
{
    // TODO 카메라 쉐이크 기능 추가 (진폭, 감쇄, 지속시간)
    // toggle 인지 아닌지
    public class CameraManager : SingletonComponent<CameraManager>
    {
        public static float worldWidthHalf;
        public static float worldHeightHalf;

        protected override void Awake()
        {
            base.Awake();

            float widthRatio = (float)Screen.width / Screen.height;

            worldHeightHalf = Camera.main.orthographicSize;
            worldWidthHalf = worldHeightHalf * widthRatio;
        }

        public static Rect GetWorldSize()
        {
            return new Rect()
            {
                xMin = -worldWidthHalf,
                xMax = worldWidthHalf,
                yMin = -worldHeightHalf,
                yMax = worldHeightHalf,
            };
        }
    }
}