using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternFireable_Slayer : PatternFireable
{
    private Pattern prevPattern;

    public override Pattern SelectNextPattern()
    {
        Pattern ret = null;

        if(prevPattern is Pattern_Slayer_2)
        {
            // TODO
            // 2번 패턴 이후엔 무조건 3번 or 4번
        }
        else
        {

        }

        prevPattern = ret;
        return ret;
    }
}
