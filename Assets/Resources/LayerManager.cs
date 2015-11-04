using UnityEngine;
using System.Collections;

public class LayerManager
{
    public enum LayerBitMask
    {
        DEFAULT = 1 << 0,
        TRANSPARENT_FX = 1 << 1,
        IGNORE_RAYCAST = 1 << 2,

        WATER = 1 << 4,
        UI = 1 << 5,

        TARGETABLE = 1 << 11,
    }

    public enum LayerNumber
    {
        DEFAULT = 0,
        TRANSPARENT_FX = 1,
        IGNORE_RAYCAST = 2,

        WATER = 4,
        UI = 5,

        TARGETABLE = 11,
    }

    // 특정한 레이어 비트 패턴을 리턴하는 함수들~



}

