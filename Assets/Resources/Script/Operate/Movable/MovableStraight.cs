using UnityEngine;
using System.Collections.Generic;

public class MovableStraight : Movable
{
    // 직선 등속 이동
    protected override void MoveFrame()
    {
        float moveDistance = speed * Time.fixedDeltaTime;

        Vector2 moveVector = VEasyCalculator.GetRotatedPosition(_owner._moveDirection, moveDistance);
        Vector2 v2Pos = _owner.transform.position;
        _owner.transform.position = v2Pos + moveVector;
    }
}