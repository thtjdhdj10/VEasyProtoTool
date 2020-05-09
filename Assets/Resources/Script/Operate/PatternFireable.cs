using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PatternFireable : Operable
{
    public string phase;

    public float delay = 3f;
    public float elapseDelay = 0f;

    private Dictionary<string, List<Pattern>> _phasePatternsDic = new Dictionary<string, List<Pattern>>();
    private List<Pattern> _currentPatternList = new List<Pattern>();
    private Pattern _currentPattern;

    public void AddPattern(string phase, Pattern pattern)
    {
        if(_phasePatternsDic.TryGetValue(phase, out List<Pattern> patterns))
        {
            patterns.Add(pattern);
        }
        else
        {
            _phasePatternsDic.Add(phase, new List<Pattern>() { pattern });
        }
    }

    private void FixedUpdate()
    {
        if (state.State)
        {
            if(_phasePatternsDic.TryGetValue(phase, out List<Pattern> patterns))
            {
                _currentPatternList = patterns;
            }

            if (_currentPattern != null)
            {
                if (_currentPattern.isPatternRunning == true)
                    return;
            }

            if (elapseDelay < delay)
            {
                elapseDelay += Time.fixedDeltaTime;
            }
            else
            {
                elapseDelay = 0f;

                _currentPattern = SelectNextPattern();

                if (_currentPattern != null)
                {
                    _currentPattern.Activate();
                }
            }
        }
    }

    public virtual Pattern SelectNextPattern()
    {
        if (_currentPatternList == null) return null;

        int prioritySum = 0;

        _currentPatternList.ForEach(pattern => prioritySum += pattern.currentPriority);

        if (prioritySum == 0)
        {
            RefillPattern();
            _currentPatternList.ForEach(pattern => prioritySum += pattern.currentPriority);
        }

        int r = Random.Range(0, prioritySum);
        int targetRange = 0;

        for (int i = 0; i < _currentPatternList.Count; ++i)
        {
            targetRange += _currentPatternList[i].currentPriority;
            if (r < targetRange)
            {
                _currentPatternList[i].currentPriority = 0;
                return _currentPatternList[i];
            }
        }

        return null;
    }

    public void RefillPattern()
    {
        _currentPatternList.ForEach(p => p.currentPriority = p.priority);
    }
}
