using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionCreateUnit : Action
{
    public Unit target;
    public Vector2 pos;

    public override void Activate()
    {
        VEasyPoolerManager.GetModifiedObjectRequest(target.name, pos);
    }

    public void SetUnit(Unit _target)
    {
        pos = new Vector2(0, 0);
    }

    public void SetLocatedUnit(Unit _target, Vector2 _pos)
    {
        target = _target;
        pos = _pos;
    }

    public void SetMovingUnit(Unit _target, Vector2 _pos, float direction, float speed)
    {

    }

    public void SetLerpTurningUnit(Unit _target, Vector2 _pos, float direction, float speed, float turnFactor)
    {

    }

    public void SetRegularTurningUnit(Unit _target, Vector2 _pos, float direction, float speed, float turnFactor)
    {

    }
}