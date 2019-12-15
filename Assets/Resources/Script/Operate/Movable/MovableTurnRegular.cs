using UnityEngine;
using System.Collections.Generic;

public class MovableTurnRegular : Movable
{
    public float turnFactor;

    // 균일한 선회
    protected override void MoveFrame()
    {
        float moveDistance = speed * Time.fixedDeltaTime;

        float dirToPlayer = VEasyCalculator.GetDirection(owner.transform.position, targetPos);

        owner.moveDirection = VEasyCalculator.GetTurningDirection(
            owner.moveDirection, dirToPlayer, turnFactor * Time.fixedDeltaTime);

        Vector2 moveVector = VEasyCalculator.GetRotatedPosition(owner.moveDirection, moveDistance);

        Vector2 v2Pos = owner.transform.position;
        owner.transform.position = v2Pos + moveVector;
    }
}
