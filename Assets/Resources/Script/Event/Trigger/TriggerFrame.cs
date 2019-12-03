using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerFrame : Trigger
{
    public TriggerFrame(Unit _owner)
        : base(_owner) { }

    public TriggerType type;

    public int passCount;
    int counter;

    public enum TriggerType
    {
        NONE = 0,
        ALWAYS,
        SOMETIMES,
    }

    public void Init(bool _isDisposableTrigger, bool _isDiposableAction, bool _isWork,
        TriggerType _type, int _passCount)
    {
        Init(_isDisposableTrigger, _isDiposableAction, _isWork);

        type = _type;
        passCount = _passCount;
    }
    
    public void SetPassCount(float termSec)
    {
        if(termSec < 0f)
        {
            Debug.LogWarning("Trigger Activate() term set error. termSec must be greater than 0.");
            return;
        }

        passCount = (int)(1f / Time.fixedDeltaTime * termSec);
    }

    void Start()
    {
        if(type == TriggerType.NONE)
        {
            Debug.LogWarning(this.ToString() + ": type is not set.");
            return;
        }

        if(type == TriggerType.SOMETIMES &&
            passCount < 0)
        {
            Debug.LogWarning(this.ToString() + ": " + passCount + " is invalid passCount.");
            return;
        }

        counter = 0;
    }

    void FixedUpdate()
    {
        switch(type)
        {
            case TriggerType.ALWAYS:
                {
                    ActivateTrigger();
                }
                break;
            case TriggerType.SOMETIMES:
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
