using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class Const
    {
        // 순서를 바꾸지 말 것.
        // index 로 사용할 수 있도록 NONE 을 맨 뒤로 했음
        public enum EDirection
        {
            LEFT = 0,
            RIGHT,
            UP,
            DOWN,

            FRONT,
            BACK,

            NONE,
        }

        public static float worldWidthHalf;
        public static float worldHeightHalf;

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

        public static bool TryCast<T>(object o, out T result) where T : class
        {
            if (o is T)
            {
                result = o as T;
                return true;
            }

            result = null;
            return false;
        }

        public static void ChangeKey<TKey, TValue>(
            Dictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }
    }
}