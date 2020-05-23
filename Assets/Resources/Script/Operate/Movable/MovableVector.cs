using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MovableVector : Movable
{
    [System.NonSerialized]
    public bool[] moveDir = new bool[4];

    public override void Init()
    {
        for (int d = 0; d < 4; ++d)
            moveDir[d] = false;
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
        if (moveDir[(int)Const.EDirection.LEFT])
        {
            delta.x -= moveDelta;
            owner.moveDir = 180f;
        }
        else if (moveDir[(int)Const.EDirection.RIGHT])
        {
            delta.x += moveDelta;
            owner.moveDir = 0f;
        }
        if (moveDir[(int)Const.EDirection.UP])
        {
            delta.y += moveDelta;
            owner.moveDir = 90f;
        }
        else if (moveDir[(int)Const.EDirection.DOWN])
        {
            delta.y -= moveDelta;
            owner.moveDir = 270f;
        }

        Vector2 v2Pos = owner.transform.position;
        owner.transform.position = v2Pos + delta;
    }
}
