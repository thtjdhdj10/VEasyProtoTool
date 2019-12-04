using UnityEngine;
using System.Collections.Generic;

public class MovableStraight : Movable
{
    public void Init(float spd, float dir, BounceType _bounceType)
    {
        speed = spd;
        direction = dir;
        bounceType = _bounceType;
    }

    // 직선 등속 이동
    protected override void MoveFrame()
    {
        float moveDistance = speed * Time.fixedDeltaTime;

        Vector2 moveVector = VEasyCalculator.GetRotatedPosition(direction, moveDistance);
        Vector2 v2Pos = owner.transform.position;
        owner.transform.position = v2Pos + moveVector;
    }
}