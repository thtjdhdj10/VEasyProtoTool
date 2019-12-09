using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_LandSlayer : EnemyBoss
{
    protected override void Awake()
    {
        base.Awake();

        Animator anim = GetComponent<Animator>();
        anim.speed = 0f;
    }

    protected override void Start()
    {
        base.Start();

        PatternFireable patternFire = GetOperable<PatternFireable>();
        patternFire.patternList.Add(new Pattern_Slayer_1(this));
        patternFire.patternList.Add(new Pattern_Slayer_2(this));
    }

}
