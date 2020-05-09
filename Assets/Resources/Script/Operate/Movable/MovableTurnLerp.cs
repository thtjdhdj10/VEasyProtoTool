using UnityEngine;
using System.Collections.Generic;

public class MovableTurnLerp : Movable
{
    public float turnFactor;

    // 목적과의 방향차이에 비례해서 선회
    protected override void MoveFrame()
    {
        float moveDistance = speed * Time.fixedDeltaTime;

        float dirToPlayer = VEasyCalculator.GetDirection(_owner.transform.position, targetPos);

        _owner._moveDirection = VEasyCalculator.GetLerpDirection(
            _owner._moveDirection, dirToPlayer, turnFactor * Time.fixedDeltaTime);

        Vector2 moveVector = VEasyCalculator.GetRotatedPosition(_owner._moveDirection, moveDistance);

        Vector2 v2Pos = _owner.transform.position;
        _owner.transform.position = v2Pos + moveVector;
    }
}