﻿using UnityEngine;
using System.Collections.Generic;

public class MovableStraight : Movable
{
    public void Init(float spd, float dir)
    {
        speed = spd;
        owner.direction = dir;
    }

    // 직선 등속 이동
    protected override void MoveFrame()
    {
        float moveDistance = speed * Time.fixedDeltaTime;

        Vector2 moveVector = VEasyCalculator.GetRotatedPosition(owner.direction, moveDistance);
        Vector2 v2Pos = owner.transform.position;
        owner.transform.position = v2Pos + moveVector;
    }
}