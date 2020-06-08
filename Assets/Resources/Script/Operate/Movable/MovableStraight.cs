using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class MovableStraight : Movable
    {
        // 직선 등속 이동
        protected override void MoveFrame()
        {
            float moveDistance = speed * Time.fixedDeltaTime;

            Vector2 moveVector = VEasyCalc.GetRotatedPosition(owner.moveDir, moveDistance);
            Vector2 v2Pos = owner.transform.position;
            owner.transform.position = v2Pos + moveVector;
        }
    }
}