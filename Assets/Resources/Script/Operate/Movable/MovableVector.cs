using UnityEngine;
using System.Collections.Generic;

public class MovableVector : Movable
{
    public bool[] moveDir = new bool[4];

    public override void Init()
    {
        base.Init();
        moveDir = new bool[4];
    }

    protected override void MoveFrame()
    {
        int dirCount = 0;

        for (int d = 0; d < 4; ++d)
        {
            if (moveDir[d] == true)
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
        if (moveDir[(int)GameManager.Direction.LEFT] == true)
        {
            delta.x -= moveDelta;
        }
        else if (moveDir[(int)GameManager.Direction.RIGHT] == true)
        {
            delta.x += moveDelta;
        }
        if (moveDir[(int)GameManager.Direction.UP] == true)
        {
            delta.y += moveDelta;
        }
        else if (moveDir[(int)GameManager.Direction.DOWN] == true)
        {
            delta.y -= moveDelta;
        }

        Vector2 v2Pos = owner.transform.position;
        owner.transform.position = v2Pos + delta;
    }
}
