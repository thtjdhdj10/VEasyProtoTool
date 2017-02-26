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

    // ProperLog

    /* 기능과 용도
     * 유니티에서 로그 출력은 부하가 걸리는 작업.
     * Skip Count 를 지정하면, 그만큼 로그를 띄우지 않고 스킵.
     * Update 에서 Log 를 출력하고 싶을 때 사용하면 좋음.
     * 
     * 사용 방법
     * 두번째 인자인 key 에 식별할 수 있는 키워드를 넣어야 함.
     * ex) log 가 p1 'pos: x, y'인 경우, key를 player1 position 으로 설정.
     * 
     * key 사용 이유
     * ProperLog 를 호출하는 코드의 호출 횟수를 Count 해야 함.
     * 하지만 string 만으론 각각의 호출을 식별할 수 없음.
     * ( 로그에 변수가 들어가는 경우 때문에 )
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
