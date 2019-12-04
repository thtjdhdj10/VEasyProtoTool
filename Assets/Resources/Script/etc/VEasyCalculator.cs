using UnityEngine;
using System.Collections.Generic;

public class VEasyCalculator
{
    public static void ChangeKey<TKey, TValue>(Dictionary<TKey, TValue> dic,
                                      TKey fromKey, TKey toKey)
    {
        TValue value = dic[fromKey];
        dic.Remove(fromKey);
        dic[toKey] = value;
    }

    public static bool TryGetKey<K, V>(Dictionary<K, V> instance, V value, out K key)
    {
        foreach (var entry in instance)
        {
            if (!entry.Value.Equals(value))
            {
                continue;
            }
            key = entry.Key;
            return true;
        }
        key = default(K);
        return false;
    }

    //

    public static float Square(float f)
    {
        return f * f;
    }

    public static Vector2 GetPosition(Unit a)
    {
        return new Vector2(a.transform.position.x, a.transform.position.y);
    }

    public static Vector2 GetDeltaPosition(Unit from, Unit to)
    {
        return GetPosition(to) - GetPosition(from);
    }

    public static float GetSqrDistance(Unit a, Unit b)
    {
        return GetDeltaPosition(a, b).SqrMagnitude();
    }

    public static bool CheckOutside(Collidable a)
    {
        Vector2 pos = a.transform.position;
        Rect rect = CameraManager.manager.GetLogicalRect();

        switch (a.colType)
        {
            case Collidable.ColliderType.CIRCLE:
                {
                    if (pos.x + a.colCircle < rect.xMin ||
                        pos.x - a.colCircle > rect.xMax ||
                        pos.y + a.colCircle < rect.yMin ||
                        pos.y - a.colCircle > rect.yMax)
                    {
                        return true;
                    }
                }
                break;
            case Collidable.ColliderType.RECT:
                {
                    if (pos.x + a.colRect.x < rect.xMin ||
                        pos.x - a.colRect.x > rect.xMax ||
                        pos.y + a.colRect.y < rect.yMin ||
                        pos.y - a.colRect.y > rect.yMax)
                    {
                        return true;
                    }
                }
                break;
        }

        return false;
    }

    public static GameManager.Direction CheckTerritory(Collidable a)
    {
        Vector2 pos = a.transform.position;
        Rect rect = CameraManager.manager.GetLogicalRect();

        switch (a.colType)
        {
            case Collidable.ColliderType.CIRCLE:
                {
                    if (pos.x - a.colCircle < rect.xMin)
                    {
                        return GameManager.Direction.LEFT;
                    }

                    if (pos.x + a.colCircle > rect.xMax)
                    {
                        return GameManager.Direction.RIGHT;
                    }

                    if (pos.y - a.colCircle < rect.yMin)
                    {
                        return GameManager.Direction.DOWN;
                    }

                    if (pos.y + a.colCircle > rect.yMax)
                    {
                        return GameManager.Direction.UP;
                    }
                }
                break;
            case Collidable.ColliderType.RECT:
                {
                    if (pos.x - a.colRect.x < rect.xMin)
                    {
                        return GameManager.Direction.LEFT;
                    }

                    if (pos.x + a.colRect.x > rect.xMax)
                    {
                        return GameManager.Direction.RIGHT;
                    }

                    if (pos.y - a.colRect.y < rect.yMin)
                    {
                        return GameManager.Direction.DOWN;
                    }

                    if (pos.y + a.colRect.y > rect.yMax)
                    {
                        return GameManager.Direction.UP;
                    }
                    break;
                }
        }

        return GameManager.Direction.NONE;
    }

    public static Vector2 ScreenToWorldPos(Vector2 screenPos)
    {
        return Camera.main.ScreenToWorldPoint(screenPos);
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

    public static bool IntersectCheck(Collidable a, Collidable b)
    {
        if (a.colType == Collidable.ColliderType.CIRCLE &&
            b.colType == Collidable.ColliderType.CIRCLE)
        {
            return IntersectCircle(a.transform.position, b.transform.position, a.colCircle + b.colCircle);
        }
        //else if(a.colType == Unit.ColliderType.RECT &&
        //    b.colType == Unit.ColliderType.RECT)
        //{
        //    return IntersectRect(a.transform.position, b.transform.position, a.colRect, b.colRect);
        //}
        else if (a.colType == Collidable.ColliderType.CIRCLE &&
            b.colType == Collidable.ColliderType.RECT)
        {
            return IntersectCircleRect(a.transform.position, b.transform.position, a.colCircle, b.colRect, -b.transform.eulerAngles.z);
        }
        else if (a.colType == Collidable.ColliderType.RECT &&
            b.colType == Collidable.ColliderType.CIRCLE)
        {
            return IntersectCircleRect(b.transform.position, a.transform.position, b.colCircle, a.colRect, -a.transform.eulerAngles.z);
        }

        return false;
    }

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

    static bool IntersectCircleRect(Vector2 pos1, Vector2 pos2, float scale1, Vector2 scale2, float angle)
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

    public static float GetDirection(Unit from, Unit to)
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

    public static void GetNormalizedDirection(ref float degrees)
    {
        if (degrees < 0f)
            degrees += 360f;
        else if (degrees >= 360f)
            degrees -= 360f;
    }

    public static Vector2 GetRotatedPosition(float degrees, Vector2 pos)
    {
        float prevDegrees = VEasyCalculator.GetDirection(new Vector2(0f, 0f), pos);

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
