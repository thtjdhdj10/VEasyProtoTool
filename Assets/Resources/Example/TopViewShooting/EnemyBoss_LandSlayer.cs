using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class EnemyBoss_LandSlayer : EnemyBoss
    {
        public string phase;

        private const string P_DEFAULT = "Default";
        private const string P_RUSH = "Rush";

        // TODO
        public override void Init()
        {
            base.Init();

            phase = P_DEFAULT;

            var anim = GetComponent<Animator>();
            anim.speed = 0f;

            var patternFire = GetOperable<PatternFireable>();
            patternFire.phase = phase;
            patternFire.AddPattern(phase, new Ptn_Slayer_1(this));
            patternFire.AddPattern(phase, new Ptn_Slayer_2(this));
        }
    }
}