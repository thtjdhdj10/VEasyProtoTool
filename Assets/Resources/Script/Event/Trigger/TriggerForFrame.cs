using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerForFrame : Trigger
{
    public Type type;

    public int passCount;
    int counter;

    public enum Type
    {
        NONE = 0,
        ALWAYS,
        SOMETIMES,
    }

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork,
        Type _type, int _passCount)
    {
        Init(_isDisposableTrigger, _isDiposableAction, _isWork);

        type = _type;
        passCount = _passCount;
    }
    
    public void SetPassCount(float termSec)
    {
        if(termSec < 0f)
        {
            CustomLog.CompleteLogWarning(
                "Trigger Activate() term set error. termSec must be greater than 0.");
            return;
        }

        passCount = (int)(1f / Time.fixedDeltaTime * termSec);
    }

    void Start()
    {
        if(type == Type.NONE)
        {
            CustomLog.CompleteLogWarning(this.name + ": type is not set.");
            return;
        }

        if(type == Type.SOMETIMES &&
            passCount < 0)
        {
            CustomLog.CompleteLogWarning(this.name + ": " + passCount + " is invalid passCount.");
            return;
        }

        counter = 0;
    }

    void FixedUpdate()
    {
        switch(type)
        {
            case Type.ALWAYS:
                {
                    ActivateTrigger();
                }
                break;
            case Type.SOMETIMES:
                {
                    if(counter < passCount)
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                        ActivateTrigger();
                    }
                }
                break;
        }
    }
}
