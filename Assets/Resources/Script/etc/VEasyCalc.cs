using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class VEasyCalc
{
    public static float Inner(Vector2 posA, Vector2 posB) // 내적
    {
        float leftExpression = posA.x * posB.x + posA.y * posB.y;
        return leftExpression / (posA.magnitude * posB.magnitude);
    }

    public static float Square(float f)
    {
        return f * f;
    }

    public static Vector2 GetPosition(MyObject a)
    {
        return new Vector2(a.transform.position.x, a.transform.position.y);
    }

    public static Vector2 GetDeltaPosition(MyObject from, MyObject to)
    {
        return GetPosition(to) - GetPosition(from);
    }

    public static float GetSqrDistance(MyObject a, MyObject b)
    {
        return GetDeltaPosition(a, b).SqrMagnitude();
    }

    public static Const.EDirection CheckOutside2D(Collider2D col)
    {
        Vector2 pos = col.transform.position;
        Rect rect = Const.GetWorldSize();

        if (Const.TryCast(col, out CircleCollider2D circleCol))
        {
            if (pos.x + circleCol.radius < rect.xMin)
                return Const.EDirection.LEFT;
            if (pos.x - circleCol.radius > rect.xMax)
                return Const.EDirection.RIGHT;
            if (pos.y + circleCol.radius < rect.yMin)
                return Const.EDirection.DOWN;
            if (pos.y - circleCol.radius > rect.yMax)
                return Const.EDirection.UP;
        }
        else if (Const.TryCast(col, out BoxCollider2D boxCol))
        {
            if (pos.x + boxCol.size.x < rect.xMin)
                return Const.EDirection.LEFT;
            if (pos.x - boxCol.size.x > rect.xMax)
                return Const.EDirection.RIGHT;
            if (pos.y + boxCol.size.y < rect.yMin)
                return Const.EDirection.DOWN;
            if (pos.y - boxCol.size.y > rect.yMax)
                return Const.EDirection.UP;
        }

        return Const.EDirection.NONE;
    }

    public static Const.EDirection CheckTerritory2D(Collider2D col)
    {
        Vector2 pos = col.transform.position;
        Rect rect = Const.GetWorldSize();

        if (Const.TryCast(col, out CircleCollider2D circleCol))
        {
            if (pos.x - circleCol.radius < rect.xMin)
                return Const.EDirection.LEFT;
            if (pos.x + circleCol.radius > rect.xMax)
                return Const.EDirection.RIGHT;
            if (pos.y - circleCol.radius < rect.yMin)
                return Const.EDirection.DOWN;
            if (pos.y + circleCol.radius > rect.yMax)
                return Const.EDirection.UP;
        }
        else if (Const.TryCast(col, out BoxCollider2D boxCol))
        {
            if (pos.x - boxCol.size.x < rect.xMin)
                return Const.EDirection.LEFT;
            if (pos.x + boxCol.size.x > rect.xMax)
                return Const.EDirection.RIGHT;
            if (pos.y - boxCol.size.y < rect.yMin)
                return Const.EDirection.DOWN;
            if (pos.y + boxCol.size.y > rect.yMax)
                return Const.EDirection.UP;
        }

        return Const.EDirection.NONE;
    }

    public static Vector2 ScreenToWorldPos(Vector2 screen)
    {
        return Camera.main.ScreenToWorldPoint(screen);
    }

    public static Vector2 WorldToScreenPos(Vector2 world)
    {
        return Camera.main.WorldToScreenPoint(world);
    }

    public static Vector2 ScreenToGUIPos(Vector2 screen)
    {
        screen.y = -screen.y;
        return screen;
    }

    // GUI 좌표계는 스크린 좌표계와 y축이 반대임
    public static Vector2 WorldToGUIPos(Vector2 world)
    {
        world.y = -world.y;
        return Camera.main.WorldToScreenPoint(world);
    }

    // 움직이는 두 Unit 이 n frame 뒤에 충돌하는지 체크
    //public static bool FutureIntersectCheck(Unit a, Unit b, int frame)
    //{
    //    Movable[] mu = new Movable[2];

    //    Movable aMove = (Movable)a.GetOperable(typeof(Movable));
    //    Movable bMove = (Movable)b.GetOperable(typeof(Movable));

    //    // TODO: check aMove is null or not

    //    mu[0] = aMove;
    //    mu[1] = bMove;

    //    Vector2[] futurePos = new Vector2[2];
    //    futurePos[0] = a.transform.position;
    //    futurePos[1] = b.transform.position;

    //    Vector2[] deltaPos = new Vector2[2];

    //    for (int i = 0; i < 2; ++i)
    //    {
    //        if (mu[i] != null)
    //        {
    //            switch (mu[i].moveType)
    //            {
    //                case Movable.MoveType.STRAIGHT:
    //                    {
    //                        deltaPos[i] = GetRotatedPosition(mu[i].direction, mu[i].speed * Time.fixedDeltaTime * (float)frame);
    //                    }
    //                    break;

    //                // TODO
    //            }
    //        }

    //        CustomLog.ProperLog("before" + futurePos[i].x, "a", 16 * 6 * 30);
    //        CustomLog.ProperLog("before" + futurePos[i].y, "b", 16 * 6 * 30);

    //        futurePos[i] += deltaPos[i];

    //        CustomLog.ProperLog(futurePos[i].x.ToString(), "c", 16 * 6 * 30);
    //        CustomLog.ProperLog(futurePos[i].y.ToString(), "d", 16 * 6 * 30);
    //    }

    //    Vector2[] originalPos = new Vector2[2];
    //    originalPos[0] = a.transform.position;
    //    originalPos[1] = b.transform.position;

    //    a.transform.position = futurePos[0];
    //    b.transform.position = futurePos[1];

    //    bool ret = IntersectCheck(a, b);

    //    a.transform.position = originalPos[0];
    //    b.transform.position = originalPos[1];

    //    return ret;
    //}

    //public static bool IntersectCheck(Collidable a, Collidable b)
    //{
    //    if (a.colType == Collidable.ColliderType.CIRCLE &&
    //        b.colType == Collidable.ColliderType.CIRCLE)
    //    {
    //        return IntersectCircle(a.transform.position, b.transform.position, a.radius + b.radius);
    //    }
    //    //else if(a.colType == Unit.ColliderType.RECT &&
    //    //    b.colType == Unit.ColliderType.RECT)
    //    //{
    //    //    return IntersectRect(a.transform.position, b.transform.position, a.colRect, b.colRect);
    //    //}
    //    else if (a.colType == Collidable.ColliderType.CIRCLE &&
    //        b.colType == Collidable.ColliderType.RECT)
    //    {
    //        return IntersectCircleRect(a.transform.position, b.transform.position, a.radius, b.rect, -b.transform.eulerAngles.z);
    //    }
    //    else if (a.colType == Collidable.ColliderType.RECT &&
    //        b.colType == Collidable.ColliderType.CIRCLE)
    //    {
    //        return IntersectCircleRect(b.transform.position, a.transform.position, b.radius, a.rect, -a.transform.eulerAngles.z);
    //    }

    //    return false;
    //}

    //static bool IntersectRect(Vector2 pos1, Vector2 pos2, Vector2 scale1, Vector2 scale2)
    //{
    //    float xDelta = Mathf.Abs(pos2.x - pos1.x);
    //    float yDelta = Mathf.Abs(pos2.y - pos1.y);

    //    if(xDelta < scale1.x + scale2.x &&
    //        yDelta < scale1.y + scale2.y)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    static bool IntersectCircle(Vector2 pos1, Vector2 pos2, float r)
    {
        if (r < 0f)
            return false;

        float deltaDistanceSquare = Vector2.SqrMagnitude(pos2 - pos1);

        if (deltaDistanceSquare > r * r)
        {
            return false;
        }

        return true;
    }

    public static bool IntersectCircleRect(Vector2 pos1, Vector2 pos2, float scale1, Vector2 scale2, float angle)
    {
        pos1 = GetRotatedPosition(angle, pos1);
        pos2 = GetRotatedPosition(angle, pos2);

        float xDelta = Mathf.Abs(pos1.x - pos2.x);
        float yDelta = Mathf.Abs(pos1.y - pos2.y);

        if (xDelta > (scale2.x + scale1)) { return false; }
        if (yDelta > (scale2.y + scale1)) { return false; }

        if (xDelta <= scale2.x) { return true; }
        if (yDelta <= scale2.y) { return true; }

        float cornerDistance_sq = (xDelta - scale2.x) * (xDelta - scale2.x) +
                             (yDelta - scale2.y) * (yDelta - scale2.y);

        return (cornerDistance_sq <= scale1 * scale1);
    }

    public static float GetDirection(MyObject from, MyObject to)
    {
        return GetDirection(from.transform.position, to.transform.position);
    }

    public static float GetDirection(Vector2 from, Vector2 to)
    {
        Vector2 v2 = (to - from).normalized;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    public static float GetLerpRange(float min, float max, float ratio)
    {
        ratio *= (max - min);
        return ratio + min;
    }

    public static float GetReflectedDirection(float moveDir, float targetDir)
    {
        GetNormalizedDirection(ref moveDir);
        GetNormalizedDirection(ref targetDir);

        moveDir -= 180f; // -180~180
        targetDir -= 180f;
        float minDir = targetDir - 90f;
        float maxDir = targetDir + 90f;
        if(minDir < -180f) minDir += 360f;
        if(maxDir > 180f) maxDir -= 360f;

        if(minDir < moveDir && moveDir < maxDir)
            moveDir = (targetDir + 90f) * 2f - moveDir + 180f;

        GetNormalizedDirection(ref moveDir);

        return moveDir;
    }

    public static float GetDirectionDelta(ref float from, ref float to)
    {
        GetNormalizedDirection(ref from);
        GetNormalizedDirection(ref to);

        float delta = to - from;

        if (delta >= 180f)
            from += 360f;
        else if (delta < -180f)
            to += 360f;

        return to - from;
    }

    public static float GetDirectionDelta(float from, float to)
    {
        GetNormalizedDirection(ref from);
        GetNormalizedDirection(ref to);

        float delta = to - from;

        if (delta >= 180f)
            from += 360f;
        else if (delta < -180f)
            to += 360f;

        return to - from;
    }

    public static float GetTurningDirection(float from, float to, float degrees)
    {
        float completeDelta = GetDirectionDelta(ref from, ref to);

        if (completeDelta >= 0f)
        {
            return Mathf.Min(from + degrees, to);
        }
        else
        {
            return Mathf.Max(from - degrees, to);
        }
    }

    public static float GetTurningDirection(float from, float to, float degrees,
        float maxDistance, float minDistance, float currentDistance, float distanceFactor)
    {
        if (currentDistance <= minDistance)
            return GetTurningDirection(from, to, degrees);
        else if (currentDistance >= maxDistance)
            return GetTurningDirection(from, to, degrees + distanceFactor);

        float distanceRatio = (currentDistance - minDistance) / (maxDistance - minDistance);

        return GetTurningDirection(from, to, degrees + distanceFactor * distanceRatio);
    }

    public static float GetLerpDirection(float from, float to, float factor)
    {
        float completeDelta = GetDirectionDelta(ref from, ref to);

        float lerpedDegrees = to * factor + from * (1f - factor);

        if (completeDelta >= 0f)
        {
            return Mathf.Min(lerpedDegrees, to);
        }
        else
        {
            return Mathf.Max(lerpedDegrees, to);
        }
    }

    public static float GetLerpDirection(float from, float to, float factor,
        float maxDistance, float minDistance, float currentDistance, float distanceFactor)
    {
        if (currentDistance <= minDistance)
            return GetLerpDirection(from, to, factor);
        else if (currentDistance >= maxDistance)
            return GetLerpDirection(from, to, factor + distanceFactor);

        float distanceRatio = (currentDistance - minDistance) / (maxDistance - minDistance);

        return GetLerpDirection(from, to, factor + distanceFactor * distanceRatio);
    }

    public static void GetNormalizedDirection(ref float degrees) // 0~360
    {
        if (degrees < 0f)
            degrees += 360f;
        else if (degrees >= 360f)
            degrees -= 360f;
    }

    public static Vector2 GetRotatedPosition(float degrees, Vector2 pos)
    {
        float prevDegrees = VEasyCalc.GetDirection(new Vector2(0f, 0f), pos);

        float distance = Vector2.Distance(new Vector2(0f, 0f), pos);

        return GetRotatedPosition(prevDegrees + degrees, distance);
    }

    public static Vector2 GetRotatedPosition(float degrees, float distance)
    {
        float x = distance * Mathf.Cos(Mathf.Deg2Rad * degrees);
        float y = distance * Mathf.Sin(Mathf.Deg2Rad * degrees);

        return new Vector2(x, y);
    }

    // analyzingTime 동안 targetPrevPos 에서 targetPos 로 이동한 적에게,
    // from 의 위치에서 "얼마"의 속도로 이동해야 collisionTime 에 충동하는지.
    public static Vector2 EstimateCollidingMoveSpeed(Vector2 from, Vector2 targetPos, Vector2 targetPrevPos, float analyzingTime, float collisionTime)
    {
        Vector2 collidingMoveSpeedForImmovableTarget = (targetPos - from) / collisionTime;
        Vector2 targetMoveSpeed = (targetPos - targetPrevPos) / analyzingTime;

        return collidingMoveSpeedForImmovableTarget - targetMoveSpeed;
    }

    // analyzingTime 동안 targetPrevPos 에서 targetPos 로 이동한 적이,
    // collisionTime 후에 도착하는 "위치".
    public static Vector2 EstimateCollidingPosition(Vector2 targetPos, Vector2 targetPrevPos, float analyzingTime, float collisionTime)
    {
        Vector2 targetMoveSpeed = (targetPos - targetPrevPos) / analyzingTime;

        return targetPos + (targetMoveSpeed * collisionTime);
    }

    public static float EstimateCollidingDirection(float dir, float prevDir, float analyzingTime, float collisionTime)
    {
        float rotateDegrees = GetDirectionDelta(prevDir, dir);

        float rotateSpeed = rotateDegrees / analyzingTime;

        float estimatedDirection = dir + rotateSpeed * collisionTime;

        GetNormalizedDirection(ref estimatedDirection);

        return estimatedDirection;
    }

    public static float EstimateCollidingDistance(float dis, float prevDis, float analyzingTime, float collisionTime)
    {
        float moveDistance = dis - prevDis;

        float moveSpeed = moveDistance / analyzingTime;

        return dis + moveSpeed * collisionTime;
    }
}
