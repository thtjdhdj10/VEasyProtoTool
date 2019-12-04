﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵 나가면 삭제
public class BulleStraight : Bullet
{
    protected override void Awake()
    {
        base.Awake();

        System.Type targetType;
        if (force == Force.PLAYER) targetType = typeof(Enemy);
        else targetType = typeof(Player);

        TriggerCollision triggerCollision = new TriggerCollision(this, targetType);
        ActionDestroyUnit actionDestroy = new ActionDestroyUnit();
        triggerCollision.actionList.Add(actionDestroy);
    }


}