using UnityEngine;
using System.Collections;

public class VEasyCalculator {

    public static bool CheckMyRect(Unit.LogicalPosition me, Unit.LogicalPosition target, float r)
    {
        if (r < 0f)
            return false;

        if(target.x > me.x + r ||
            target.x < me.x - r ||
            
            target.y > me.y + r ||
            target.y < me.y - r)
        {
            return false;
        }

        return true;
    }

    public static bool CheckMyCircle(Unit.LogicalPosition me, Unit.LogicalPosition target, float r)
    {
        if (r < 0f)
            return false;
        
        float deltaDistanceSquare = CalcDistanceSquare2D(me, target);
        
        if (deltaDistanceSquare > r*r)
        {
            return false;
        }

        return true;
    }

    public static float CalcDistanceSquare2D(Unit.LogicalPosition me, Unit.LogicalPosition target)
    {
        float deltaX = target.x - me.x;
        float deltaY = target.y - me.y;

        return deltaX * deltaX + deltaY * deltaY;
    }

    public static float CalcDistance2D(Unit.LogicalPosition me, Unit.LogicalPosition target)
    {
        float deltaX = target.x - me.x;
        float deltaY = target.y - me.y;

        float deltaSquare = deltaX * deltaX + deltaY * deltaY;

        return Mathf.Sqrt(deltaSquare);
    }
}
