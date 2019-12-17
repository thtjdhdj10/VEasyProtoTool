using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternFireable : Operable
{
    public List<Pattern> patternList = new List<Pattern>();
    public Pattern currentPattern;

    public float delay = 3f;
    public float elapseDelay = 0f;

    private void FixedUpdate()
    {
        if (state.State)
        {
            if (currentPattern != null)
            {
                if (currentPattern.isPatternRunning == true)
                    return;
            }

            if (elapseDelay < delay)
            {
                elapseDelay += Time.fixedDeltaTime;
            }
            else
            {
                elapseDelay = 0f;

                currentPattern = SelectNextPattern();

                if (currentPattern != null)
                {
                    currentPattern.Activate();
                }
            }
        }
    }

    public virtual Pattern SelectNextPattern()
    {
        int prioritySum = 0;

        foreach(var pattern in patternList)
        {
            prioritySum += pattern.currentPriority;
        }

        if (prioritySum == 0)
        {
            RefillPattern();
            foreach (var pattern in patternList)
            {
                prioritySum += pattern.currentPriority;
            }
        }

        int r = Random.Range(0, prioritySum);
        int targetRange = 0;

        for(int i = 0; i < patternList.Count; ++i)
        {
            targetRange += patternList[i].currentPriority;
            if (r < targetRange)
            {
                patternList[i].currentPriority = 0;
                return patternList[i];
            }
        }

        return null;
    }

    public void RefillPattern()
    {
        foreach(var pattern in patternList)
        {
            pattern.currentPriority = pattern.priority;
        }
    }
}
