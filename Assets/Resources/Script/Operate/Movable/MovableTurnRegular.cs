using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class MovableTurnRegular : Movable
    {
        public float turnFactor;

        // 균일한 선회
        protected override void MoveFrame()
        {
            float moveDistance = speed * Time.fixedDeltaTime;

            float dirToPlayer = VEasyCalc.GetDirection(owner.transform.position, targetPos);

            owner.moveDir = VEasyCalc.GetTurningDirection(
                owner.moveDir, dirToPlayer, turnFactor * Time.fixedDeltaTime);

            Vector2 moveVector = VEasyCalc.GetRotatedPosition(owner.moveDir, moveDistance);

            Vector2 v2Pos = owner.transform.position;
            owner.transform.position = v2Pos + moveVector;
        }
    }
}