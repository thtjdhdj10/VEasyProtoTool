using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Paralyze : BulleStraight
{
    public float duration;

    protected override void Awake()
    {
        base.Awake();

        foreach(var trg in triggerList)
        {
            if(trg is TriggerCollision)
            {
                ActionActiveTargetOperable<Movable> actParalyze
                    = new ActionActiveTargetOperable<Movable>(trg, Multistat.type.STURN, true);
                ActionActiveTargetOperable<Movable> actRestoreParalyze
                    = new ActionActiveTargetOperable<Movable>(trg, Multistat.type.STURN, false, duration);
                break;
            }
        }
    }



}
