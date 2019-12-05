using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTarget : Movable
{
    public void Init(float spd, Unit _target)
    {
        speed = spd;
        target = _target;
    }

    protected override void MoveFrame()
    {
        // TODO target으로 이동
    }
}
