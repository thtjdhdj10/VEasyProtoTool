using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLandSlayer : Boss
{
    protected override void Awake()
    {
        base.Awake();

        PatternFireable patternFire = GetOperable<PatternFireable>();
        //PatternFire.
        //    Pattern_Slayer_1 pattern_S1 = new Pattern_Slayer_1(this);
    }

}
