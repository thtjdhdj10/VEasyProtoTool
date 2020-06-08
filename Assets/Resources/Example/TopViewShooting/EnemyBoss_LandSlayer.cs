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

        protected override void Awake()
        {
            base.Awake();

            phase = P_DEFAULT;

            Animator anim = GetComponent<Animator>();
            anim.speed = 0f;
        }

        protected override void Start()
        {
            base.Start();

            PatternFireable patternFire = GetOperable<PatternFireable>();
            patternFire.phase = phase;
            patternFire.AddPattern(phase, new Ptn_Slayer_1(this));
            patternFire.AddPattern(phase, new Ptn_Slayer_2(this));
        }

    }
}