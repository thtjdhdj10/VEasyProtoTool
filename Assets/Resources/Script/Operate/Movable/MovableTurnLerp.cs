using UnityEngine;
using System.Collections.Generic;

public class MovableTurnLerp : Movable
{
    public float turnFactor;

    // 목적과의 방향차이에 비례해서 선회
    protected override void MoveFrame()
    {
        float moveDistance = speed * Time.fixedDeltaTime;

        float dirToPlayer = VEasyCalculator.GetDirection(owner.transform.position, _targetPos);

        owner.moveDirection = VEasyCalculator.GetLerpDirection(
            owner.moveDirection, dirToPlayer, turnFactor * Time.fixedDeltaTime);

        Vector2 moveVector = VEasyCalculator.GetRotatedPosition(owner.moveDirection, moveDistance);

        Vector2 v2Pos = owner.transform.position;
        owner.transform.position = v2Pos + moveVector;
    }
}