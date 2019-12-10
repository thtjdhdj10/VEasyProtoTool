using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternFireable : Operable
{
    public List<Pattern> patternList = new List<Pattern>();
    public Pattern currentPattern;

    public float remainDelay = 3f;

    private void FixedUpdate()
    {
        if (active.State)
        {
            if (currentPattern != null)
            {
                if (currentPattern.isPatternRunning == true)
                    return;
            }

            if (remainDelay > 0f)
                remainDelay -= Time.fixedDeltaTime;
            else
            {
                currentPattern = SelectNextPattern();

                if (currentPattern != null)
                {
                    currentPattern.Activate();
                }
            }
        }
    }

    // TODO 주석달기
    public Pattern SelectNextPattern()
    {
        int prioritySum = 0;
        foreach(var pattern in patternList)
        {
            prioritySum += pattern.currentPriority;
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
}
