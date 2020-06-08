using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace VEPT
{
    public class MovableVector : Movable
    {
        [System.NonSerialized]
        public bool[] moveVector = new bool[4];

        public override void Init()
        {
            for (int d = 0; d < 4; ++d)
                moveVector[d] = false;
        }

        protected override void HandleUpdateState(bool _state)
        {
            Init();
        }

        protected override void MoveFrame()
        {
            int dirCount = 0;

            for (int d = 0; d < 4; ++d)
            {
                if (moveVector[d] == true)
                {
                    dirCount++;
                }
            }

            float moveDelta = speed * Time.deltaTime;

            float reciprocalOfRoot2 = 0.7071f;
            if (dirCount >= 2)
            {
                moveDelta = speed * Time.deltaTime * reciprocalOfRoot2;
            }

            Vector2 delta = new Vector2(0f, 0f);
            if (moveVector[(int)EDirection.LEFT])
            {
                delta.x -= moveDelta;
                owner.moveDir = 180f;
            }
            else if (moveVector[(int)EDirection.RIGHT])
            {
                delta.x += moveDelta;
                owner.moveDir = 0f;
            }
            if (moveVector[(int)EDirection.UP])
            {
                delta.y += moveDelta;
                owner.moveDir = 90f;
            }
            else if (moveVector[(int)EDirection.DOWN])
            {
                delta.y -= moveDelta;
                owner.moveDir = 270f;
            }

            Vector2 v2Pos = owner.transform.position;
            owner.transform.position = v2Pos + delta;
        }
    }
}