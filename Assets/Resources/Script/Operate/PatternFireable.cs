using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternFireable : Operable
{
    public string phase;

    public float delay = 3f;
    public float elapseDelay = 0f;

    private Dictionary<string, List<Pattern>> phasePatternsDic = new Dictionary<string, List<Pattern>>();
    private List<Pattern> currentPatternList = new List<Pattern>();
    private Pattern currentPattern;

    public void AddPattern(string phase, Pattern pattern)
    {
        if(phasePatternsDic.TryGetValue(phase, out List<Pattern> patterns))
        {
            patterns.Add(pattern);
        }
        else
        {
            phasePatternsDic.Add(phase, new List<Pattern>() { pattern });
        }
    }

    private void FixedUpdate()
    {
        if (state.State)
        {
            if(phasePatternsDic.TryGetValue(phase, out List<Pattern> patterns))
            {
                currentPatternList = patterns;
            }

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
        if (currentPatternList == null) return null;

        int prioritySum = 0;

        foreach(var pattern in currentPatternList)
        {
            prioritySum += pattern.currentPriority;
        }

        if (prioritySum == 0)
        {
            RefillPattern();
            foreach (var pattern in currentPatternList)
            {
                prioritySum += pattern.currentPriority;
            }
        }

        int r = Random.Range(0, prioritySum);
        int targetRange = 0;

        for(int i = 0; i < currentPatternList.Count; ++i)
        {
            targetRange += currentPatternList[i].currentPriority;
            if (r < targetRange)
            {
                currentPatternList[i].currentPriority = 0;
                return currentPatternList[i];
            }
        }

        return null;
    }

    public void RefillPattern()
    {
        foreach(var pattern in currentPatternList)
        {
            pattern.currentPriority = pattern.priority;
        }
    }
}
