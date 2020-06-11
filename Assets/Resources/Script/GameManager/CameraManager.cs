using UnityEngine;
using System.Collections;
using System;

namespace VEPT
{
    // TODO 카메라 쉐이크 기능 추가 (진폭, 감쇄, 지속시간)
    // toggle 인지 아닌지
    public class CameraManager : SingletonComponent<CameraManager>
    {
        public static float WorldWidth
        {
            get
            {
                Instance.Init();
                return Instance.worldWidth;
            }
        }
        public static float WorldWidthHalf
        {
            get
            {
                Instance.Init();
                return Instance.worldWidthHalf;
            }
        }
        public static float WorldHeight
        {
            get
            {
                Instance.Init();
                return Instance.worldHeight;
            }
        }
        public static float WorldHeightHalf
        {
            get
            {
                Instance.Init();
                return Instance.worldHeightHalf;
            }
        }

        private float worldWidthHalf;
        private float worldHeightHalf;
        private float worldWidth;
        private float worldHeight;

        public static Rect GetWorldSize()
        {
            return new Rect()
            {
                xMin = -WorldWidthHalf,
                xMax = WorldWidthHalf,
                yMin = -WorldHeightHalf,
                yMax = WorldHeightHalf,
            };
        }

        protected override void Init()
        {
            float widthRatio = (float)Screen.width / Screen.height;

            worldHeightHalf = Camera.main.orthographicSize;
            worldWidthHalf = worldHeightHalf * widthRatio;

            worldHeight = worldHeightHalf * 2f;
            worldWidth = worldWidthHalf * 2f;
        }
    }
}