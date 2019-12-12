using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Paralyze : BulletStraight
{
    public float duration;

    protected override void Awake()
    {
        base.Awake();

        foreach(var trg in triggerList)
        {
            if(trg is TriggerCollision)
            {
                new ActionActiveTargetOperable<Movable>(trg, Multistat.StateType.STURN, true);
                new ActionActiveTargetOperable<Movable>(trg, Multistat.StateType.STURN, false)
                { delay = duration };
                break;
            }
        }
    }



}
