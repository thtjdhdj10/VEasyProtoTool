using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerTimer : Trigger
{
    public TriggerTimer(Unit _owner, float _delay, bool _isActivateOnStart)
        : base(_owner)
    {
        delay = _delay;
        isActivateOnStart = _isActivateOnStart;

        if (isActivateOnStart == true)
            remainDelay = delay;
        else remainDelay = 0f;
    }

    public float delay;
    private float remainDelay;
    private bool isActivateOnStart;

    public override void Init()
    {
        base.Init();

        if (isActivateOnStart == true)
            remainDelay = delay;
        else remainDelay = 0f;
    }

    public void HandleFixedUpdate()
    {
        if (remainDelay >= delay)
        {
            ActivateTrigger();
            remainDelay = 0f;
        }
        else
        {
            remainDelay += Time.fixedDeltaTime;
        }
    }
}
