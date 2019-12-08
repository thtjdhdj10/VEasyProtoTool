using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pattern
{
    public bool isPatternRunning;

    public float preDelay;
    public float postDelay;

    // priority 높을수록 패턴 발동확률 증가 0 = 0%
    // 패턴 발동 시 currentpriority 0으로 초기화
    public float priority = 1f;
    public float currentPriority;

    public void Activate()
    {
        GameManager.gm.StartCoroutine(PatternProcess());
    }

    private IEnumerator PatternProcess()
    {
        isPatternRunning = true;
        yield return new WaitForSeconds(preDelay);

        yield return GameManager.gm.StartCoroutine(Fire());

        yield return new WaitForSeconds(postDelay);
        isPatternRunning = false;
    }

    public abstract IEnumerator Fire();
}

public class PatternContinued : Pattern
{
    public List<Pattern> patternList = new List<Pattern>();

    public float patternTerm;

    public override IEnumerator Fire()
    {
        for (int i = 0; i < patternList.Count; ++i)
        {
            yield return GameManager.gm.StartCoroutine(patternList[i].Fire());

            if (i > patternList.Count - 1)
                yield return new WaitForSeconds(patternTerm);
        }
    }
}

public abstract class PatternFire : Pattern
{
    public Unit firePrefab;

    public Unit posRootUnit;
    public Vector2 position;
    protected Vector2 deltaPos;

    public Unit dirRootUnit;
    public float direction;
    protected float deltaDir;

    public int count = 1;
    protected int firedCount = 0;
    public float term = 0f;
    public int distnaceFromRoot = 0;

    public virtual void PreFireProcess() { }

    public override IEnumerator Fire()
    {
        if (posRootUnit != null) position = posRootUnit.transform.position;
        if (dirRootUnit != null) direction = dirRootUnit.direction;

        for (int i = 0; i < count; ++i)
        {
            PreFireProcess();

            FireProcess();

            ++firedCount;

            if (term > 0f && i < count - 1) yield return new WaitForSeconds(term);
        }
    }

    public virtual void FireProcess()
    {
        Unit unit = GameObject.Instantiate(firePrefab);
        if (unit == null) return;

        if (posRootUnit != null) position = posRootUnit.transform.position;
        unit.transform.position = position + deltaPos;

        if (dirRootUnit != null) direction = dirRootUnit.direction;
        unit.direction = direction + deltaDir;
    }
}

// update 될 수 있는 owner 이동방향으로 발사
public class PatternFireDirection : PatternFire
{
    public Unit owner;
    public Vector2 ownerPos;

    public override void PreFireProcess()
    {
        if (owner != null) direction = owner.direction;
    }
}

// update 될 수 있는 target 위치로 발사
public class PatternFireTarget : PatternFire
{
    public Unit target;
    public Vector2 targetPos;

    public override void PreFireProcess()
    {
        if (target != null) targetPos = target.transform.position;
        direction = VEasyCalculator.GetDirection(position, targetPos);
    }
}

public class PatternFireTarget_AngleRandom : PatternFireTarget
{
    public float angle = 360f;

    public override void PreFireProcess()
    {
        if (target != null) targetPos = target.transform.position;
        direction = VEasyCalculator.GetDirection(position, targetPos);

        deltaDir = angle * (Random.Range(0f, 1f) - 0.5f);
    }
}

public class PatternFireTarget_RowRandom : PatternFireTarget
{
    public float length = 3f;

    public override void PreFireProcess()
    {
        if (target != null) targetPos = target.transform.position;
        direction = VEasyCalculator.GetDirection(position, targetPos);

        float deltaDistance = length * (Random.Range(0f, 1f) - 0.5f);
        deltaPos = VEasyCalculator.GetRotatedPosition(direction, new Vector2(deltaDistance, 0f));
    }
}

public class PatternFireAngleRandom : PatternFire
{
    public float angle = 360f;

    public override void PreFireProcess()
    {
        if (count == 1) return;

        deltaDir = angle * (Random.Range(0f, 1f) - 0.5f);
    }
}

public class PatternFireRowRandom : PatternFire
{
    public float length = 3f;

    public override void PreFireProcess()
    {
        if (count == 1) return;

        float deltaDistance = length * (Random.Range(0f, 1f) - 0.5f);
        deltaPos = VEasyCalculator.GetRotatedPosition(direction, new Vector2(deltaDistance, 0f));
    }
}

// 방사
public class PatternFireAngle : PatternFire
{
    public float angle = 360f;
    public bool isClockwise = false;

    public override void PreFireProcess()
    {
        if (count == 1) return;

        float fireIndex = (float)firedCount / (count - 1) - 0.5f;
        if (isClockwise) fireIndex = -fireIndex;
        deltaDir = angle * fireIndex;
    }
}

// 일렬로 발사
public class PatternFireRow : PatternFire
{
    public float length = 3f;
    public bool isLeftToRight = true;

    public override void PreFireProcess()
    {
        if (count == 1) return;

        float fireIndex = (float)firedCount / (count - 1) - 0.5f;
        if (!isLeftToRight) fireIndex = -fireIndex;
        float deltaDistance = length * fireIndex;
        deltaPos = VEasyCalculator.GetRotatedPosition(direction, new Vector2(deltaDistance, 0f));
    }
}

//

public class Pattern_Slayer_1 : PatternContinued
{
    public Pattern_Slayer_1(Unit rootPos, Movable rootDir)
    {
        PatternFireAngleRandom pt = new PatternFireAngleRandom();
        pt.firePrefab = Resources.Load("Prefabs/Player Bullet A") as Unit;

        pt.preDelay = 1f;
        pt.postDelay = 2f;

        pt.count = 30;
        pt.term = 0.1f;
        pt.angle = 90f;

        //pt.direction = direction;
        //pt.position = rootPos;

        patternList.Add(pt);
    }
}