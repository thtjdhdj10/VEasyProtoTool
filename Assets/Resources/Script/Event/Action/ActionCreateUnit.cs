using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionCreateUnit : Action
{
    private Unit target;
    private Vector2 pos;
    private bool isMovingUnit;
    private float direction;
    private float speed;

    public ActionCreateUnit(Trigger trigger, Unit _target, Vector2 _pos)
        :base(trigger)
    {
        target = _target;
        pos = _pos;

        isMovingUnit = false;
    }

    public ActionCreateUnit(Trigger trigger, Unit _target, Vector2 _pos, float _direction, float _speed)
        :base(trigger)
    {
        target = _target;
        pos = _pos;

        isMovingUnit = true;

        direction = _direction;
        speed = _speed;
    }

    public override void Activate(Trigger trigger)
    {
        Unit unit = GameObject.Instantiate(target);
        unit.transform.position = pos;

        if (isMovingUnit)
        {
            Movable move = unit.GetOperable<Movable>();
            move.direction = direction;
            move.speed = speed;
        }
    }
}