using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomLog
{
    public enum WarningLevel
    {
        NONE,
        WARN,
        ERROR,
    }

    static Dictionary<string, int> skipCountDic = new Dictionary<string, int>();

    // 더블클릭했을 때 호출부 이동이 CustomLog 여서 디버깅이 불편함.
    // 해당 문제가 해결될 때 까지 사용하지 말 것

    // ProperLog

    /* 기능과 용도
     * 유니티에서 로그 출력은 부하가 걸리는 작업.
     * Skip Count 를 지정하면, 그만큼 로그를 띄우지 않고 스킵.
     * Update 에서 Log 를 출력하고 싶을 때 사용하면 좋음.
     * 
     * 두번째 인자인 key를 기준으로 같은 로그인지 확인
     */

    [System.Diagnostics.Conditional("DEBUG")]
    public static void ProperLog(string str, string key, int skipCount)
    {
        if (skipCountDic.ContainsKey(key) == true)
        {
            skipCountDic[key]++;
            if (skipCountDic[key] >= skipCount)
            {
                CompleteLog(str + " (" + skipCount + ")");

                skipCountDic[key] = 0;
            }
        }
        else
        {
            CompleteLog(str);
            skipCountDic.Add(key, 1);
        }
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void CompleteLog(string str)
    {
        CompleteLog(str, WarningLevel.NONE);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void CompleteLogWarning(string str)
    {
        CompleteLog(str, WarningLevel.WARN);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void CompleteLogError(string str)
    {
        CompleteLog(str, WarningLevel.ERROR);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void CompleteLog(string str, bool print)
    {
        if (print == false) return;

        CompleteLog(str, WarningLevel.NONE);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void CompleteLogWarning(string str, bool print)
    {
        if (print == false) return;

        CompleteLog(str, WarningLevel.WARN);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void CompleteLogError(string str, bool print)
    {
        if (print == false) return;

        CompleteLog(str, WarningLevel.ERROR);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    static void CompleteLog(string str, WarningLevel wl)
    {
        switch (wl)
        {
            case WarningLevel.NONE:
                {
                    Debug.Log(str);
                }
                break;
            case WarningLevel.WARN:
                {
                    Debug.LogWarning(str);
                }
                break;
            case WarningLevel.ERROR:
                {
                    Debug.LogError(str);
                }
                break;
        }
    }
}
