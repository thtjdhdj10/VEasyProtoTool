using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomRandGen
{
    public static float Rand(float min, float max)
    {
        return Random.Range(min, max);
    }

    public static int Rand(int min, int max)
    {
        return Random.Range(min, max);
    }

//    int d = System.Enum.GetNames(typeof(d)).Length;
    public static int Rand(float[] weight)
    {
        // 양수화
        {
            float minValue = float.MaxValue;

            for (int i = 0; i < weight.Length; ++i)
            {
                if (weight[i] < minValue)
                    minValue = weight[i];
            }

            if (minValue < 0f)
            {
                for (int i = 0; i < weight.Length; ++i)
                {
                    weight[i] -= minValue;
                }
            }
        }

        // 정규화, 0에서 1까지 증가하는 방식으로.
        float[] normalizedIncreasingWeight = new float[weight.Length];
        {
            float sumWeight = 0f;

            for (int i = 0; i < weight.Length; ++i)
            {
                sumWeight += weight[i];
            }

            for (int i = 0; i < weight.Length; ++i)
            {
                normalizedIncreasingWeight[i] = weight[i] / sumWeight;
                if (i > 0)
                    normalizedIncreasingWeight[i] += normalizedIncreasingWeight[i - 1];
            }
        }

        //

        float r = Rand(0f, 1f);

        for (int i = 0; i < normalizedIncreasingWeight.Length; ++i)
        {
            if (normalizedIncreasingWeight[i] >= r)
            {
                return i;
            }
        }

        CustomLog.CompleteLogError("Assert");

        return 0;
    }
    
    public static int Rand(int[] weight)
    {
        float[] fWeight = new float[weight.Length];

        for (int i = 0; i < weight.Length; ++i)
        {
            fWeight[i] = (float)weight[i];
        }

        return Rand(fWeight);
    }
}
