using UnityEngine;
using System.Collections;

namespace VEPT
{
    // TODO 카메라 쉐이크 기능 추가 (진폭, 감쇄, 지속시간)
    // toggle 인지 아닌지
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager manager;

        public static float worldWidthHalf;
        public static float worldHeightHalf;

        void Awake()
        {
            manager = this;
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