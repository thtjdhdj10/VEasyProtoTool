using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternFireable : Operable
{
    public List<Pattern> patternList = new List<Pattern>();
    public Pattern nextPattern;

    public float remainDelay = 3f;

    private void FixedUpdate()
    {
        if (active)
        {
            if (remainDelay > 0f)
                remainDelay -= Time.fixedDeltaTime;
            else
            {
                if (nextPattern == null)
                    nextPattern = SelectNextPattern();


            }
        }
    }

    public void ActivatePattern(Pattern pattern)
    {

    }

    public Pattern SelectNextPattern()
    {
        return null;


    }

}
