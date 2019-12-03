using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerFrame : Trigger
{
    public TriggerFrame(Unit _owner, int _passCount)
        : base(_owner)
    {
        passCount = _passCount;
    }

    public int passCount = 0;
    int counter;

    public override void Init()
    {
        base.Init();

        counter = 0;
    }

    public void HandleFixedUpdate()
    {
        if (passCount == 0)
        {
            ActivateTrigger();
        }
        else
        {
            if (counter < passCount)
            {
                counter++;
            }
            else
            {
                counter = 0;
                ActivateTrigger();
            }
        }
    }
}
