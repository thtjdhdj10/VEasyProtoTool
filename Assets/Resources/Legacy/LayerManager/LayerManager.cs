using UnityEngine;
using System.Collections.Generic;

// 개발 중단됨
public class LayerManager : MonoBehaviour
{

    static Dictionary<string, int> layerNameNumber = new Dictionary<string, int>();

    public const int MaxLayerCount = 32;

    void Awake()
    {
        for (int i = 0; i < MaxLayerCount; i++)
        {
            string name = LayerMask.LayerToName(i);

            if (name.Length > 0)
            {
                layerNameNumber[name] = i;
            }
        }
    }

    static public bool CheckLayer(int layer, string name)
    {
        if (layerNameNumber.ContainsKey(name) == false)
        {
            Debug.LogWarning(name + " is unvalid layer name");
            return false;
        }

        int number = layerNameNumber[name];

        return CheckBit(layer, number);
    }

    static public bool CheckLayerOr(int layer, params string[] names)
    {
        for (int i = 0; i < names.Length; ++i)
        {
            if (layerNameNumber.ContainsKey(names[i]) == false)
            {
                Debug.LogWarning(names[i] + " is unvalid layer name");
                continue;
            }

            int number = layerNameNumber[names[i]];

            if (CheckBit(layer, number) == true)
                return true;
        }

        return false;
    }

    static public bool CheckLayerAnd(int layer, params string[] names)
    {
        for (int i = 0; i < names.Length; ++i)
        {
            if (layerNameNumber.ContainsKey(names[i]) == false)
            {
                Debug.LogWarning(names[i] + " is unvalid layer name");
                continue;
            }

            int number = layerNameNumber[names[i]];

            if (CheckBit(layer, number) == false)
                return false;
        }

        return true;
    }

    static public void AddLayer(ref int layer, params string[] names)
    {
        for (int i = 0; i < names.Length; ++i)
        {
            int number = layerNameNumber[names[i]];

            AddBit(ref layer, number);
        }
    }

    static public void AddLayer(ref int layer, params int[] number)
    {
        for (int i = 0; i < number.Length; ++i)
        {
            AddBit(ref layer, number[i]);
        }
    }

    static public void RemoveLayer(ref int layer, params string[] names)
    {
        for (int i = 0; i < names.Length; ++i)
        {
            int number = layerNameNumber[names[i]];

            RemoveBit(ref layer, number);
        }
    }

    static public void RemoveLayer(ref int layer, params int[] number)
    {
        for (int i = 0; i < number.Length; ++i)
        {
            RemoveBit(ref layer, number[i]);
        }
    }

    //

    static public int LogicalRightShift(int pattern, int shift)
    {
        return pattern = unchecked((int)((uint)pattern >> shift));
    }

    static public bool CheckBit(int pattern, int number)
    {
        if (number < 0 || number >= 32)
            return false;

        int mask = NumberToMask(number);

        return System.Convert.ToBoolean(mask & pattern);
    }

    static public void AddBit(ref int pattern, int number)
    {
        if (number < 0 || number >= 32)
            return;

        int mask = NumberToMask(number);

        pattern = pattern | mask;
    }

    static public void RemoveBit(ref int pattern, int number)
    {
        if (number < 0 || number >= 32)
            return;

        int mask = NumberToMask(number);

        mask = -1 ^ mask;

        pattern = pattern & mask;
    }

    static public int StringToMask(string str)
    {
        if (layerNameNumber.ContainsKey(str) == false)
        {
            Debug.LogWarning(str + " is unvalid layer name");
            return -1;
        }

        return NumberToMask(layerNameNumber[str]);
    }

    static public int StringToNumber(string str)
    {
        if (layerNameNumber.ContainsKey(str) == false)
        {
            Debug.LogWarning(str + " is unvalid layer name");
            return -1;
        }

        return layerNameNumber[str];
    }

    static public int NumberToMask(int number)
    {
        if (number < 0 || number >= 32)
            return 0;

        int rightMask = -1;
        int leftMask = -1;

        rightMask = LogicalRightShift(rightMask, sizeof(int) * 8 - number - 1);
        leftMask = leftMask << number;

        return rightMask & leftMask;
    }
}