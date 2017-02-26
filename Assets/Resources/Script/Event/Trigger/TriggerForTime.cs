using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerForTime : Trigger
{
    public float time;

    public Type type;

    public enum Type
    {
        NONE = 0,
        STATIC_TIMER,
        DYNAMIC_TIMER,
    }

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork,
        Type _type, float _time)
    {
        Init(_isDisposableTrigger, _isDiposableAction, _isWork);

        type = _type;
        time = _time;
    }

    void Start()
    {
        if (type == Type.NONE)
        {
            CustomLog.CompleteLogWarning(this.name + ": type is not set.");
            return;
        }

        if(time <= 0f)
        {
            CustomLog.CompleteLogWarning(this.name + ": " + time + "f is invalid time set.");
            return;
        }

        switch(type)
        {
            case Type.STATIC_TIMER:
                {
                    StartCoroutine(ActivateStaticTime());
                }
                break;
            case Type.DYNAMIC_TIMER:
                {
                    StartCoroutine(ActivateDynamicTime());
                }
                break;
        }
    }

    IEnumerator ActivateDynamicTime()
    {
        float passedTime = 0;
        while (passedTime < time)
        {
            passedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        ActivateTrigger();

        yield break;
    }

    IEnumerator ActivateStaticTime()
    {
        yield return new WaitForSeconds(time);

        ActivateTrigger();

        yield break;
    }
}
