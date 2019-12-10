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
    public int priority = 1;
    public int currentPriority = 1;

    public void Activate()
    {
        GameManager.gm.StartCoroutine(PatternProcess());
    }

    private IEnumerator PatternProcess()
    {
        isPatternRunning = true;

        yield return GameManager.gm.StartCoroutine(PreFire());

        yield return new WaitForSeconds(preDelay);

        yield return GameManager.gm.StartCoroutine(Fire());

        yield return new WaitForSeconds(postDelay);

        yield return GameManager.gm.StartCoroutine(PostFire());

        isPatternRunning = false;
    }

    public abstract IEnumerator Fire();
    public virtual IEnumerator PreFire()
    {
        yield break;
    }
    public virtual IEnumerator PostFire()
    {
        yield break;
    }
}

public class PatternFire : Pattern
{
    public Unit firePrefab;

    public Unit posRootUnit;
    public Vector2 position;
    public Vector2 deltaPos;

    public Unit dirRootUnit;
    public float direction;
    public float deltaDir;

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

public class PatternFireCircle : PatternFire
{
    public bool isClockwise = false;

    public override void PreFireProcess()
    {
        if (count == 1) return;

        float fireIndex = (float)firedCount / count;
        if (isClockwise) fireIndex = -fireIndex;
        deltaDir = 360f * fireIndex;
    }
}

// 샷건처럼 방사
public class PatternFireAngle : PatternFire
{
    public float angle = 120f;
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

// 보고있는 방향 +-60도로 난사
public class Pattern_Slayer_1 : Pattern
{
    public List<Pattern> patternList = new List<Pattern>();

    private Movable move;

    public Pattern_Slayer_1(Unit unit)
    {
        postDelay = 5f;

        move = unit.GetOperable<Movable>();

        PatternFireAngleRandom pattern1 = new PatternFireAngleRandom();
        GameObject go = ResourcesManager.LoadGameObject(ResourcesManager.PrefabName.Bullet_Slayer_1);
        Bullet_Slayer_1 bullet = go.GetComponent<Bullet_Slayer_1>();
        bullet.owner = unit;

        pattern1.firePrefab = bullet;

        float duration = 5f;

        pattern1.count = 150;
        pattern1.term = duration / pattern1.count;
        pattern1.angle = 110f;

        pattern1.posRootUnit = unit;
        pattern1.dirRootUnit = unit;

        patternList.Add(pattern1);

        PatternFire[] pattern2 = new PatternFire[2];
        for(int i = 0; i < 2; ++i)
        {
            pattern2[i] = new PatternFire();
            pattern2[i].firePrefab = bullet;

            pattern2[i].count = 30;
            pattern2[i].term = duration / pattern2[i].count;
            if(i == 0) pattern2[i].deltaDir = -60f;
            else pattern2[i].deltaDir = 60f;

            pattern2[i].posRootUnit = unit;
            pattern2[i].dirRootUnit = unit;

            patternList.Add(pattern2[i]);
        }
    }

    public override IEnumerator Fire()
    {
        move.active.SetState(Multistat.type.ACTIVATING_PATTERN, true);

        GameManager.gm.StartCoroutine(patternList[0].Fire());
        GameManager.gm.StartCoroutine(patternList[1].Fire());
        yield return GameManager.gm.StartCoroutine(patternList[2].Fire());
    }

    public override IEnumerator PostFire()
    {
        move.active.SetState(Multistat.type.ACTIVATING_PATTERN, false);
        yield break;
    }
}

// 전방향 난사
public class Pattern_Slayer_2 : Pattern
{
    public List<Pattern> patternList = new List<Pattern>();

    private const int count = 20;
    private const float duration = 5f;
    private const float term = duration / count;
    private PatternFireCircle[] patterns = new PatternFireCircle[count];
    private Movable move;

    public Pattern_Slayer_2(Unit unit)
    {
        postDelay = 5f;

        move = unit.GetOperable<Movable>();

        GameObject go = ResourcesManager.LoadGameObject(ResourcesManager.PrefabName.Bullet_Slayer_2);

        for (int i = 0; i < count; ++i)
        {
            patterns[i] = new PatternFireCircle();

            patterns[i].firePrefab = go.GetComponent<Bullet>();

            patterns[i].count = 8;
            patterns[i].term = 0f;

            patterns[i].posRootUnit = unit;
            patterns[i].dirRootUnit = null;
            patterns[i].direction = i * 120f;
        }
    }

    public override IEnumerator Fire()
    {
        move.active.SetState(Multistat.type.ACTIVATING_PATTERN, true);

        for (int i = 0; i < count; ++i)
        {
            GameManager.gm.StartCoroutine(patterns[i].Fire());
            if (i < count - 1)
                yield return new WaitForSeconds(term);
        }
    }

    public override IEnumerator PostFire()
    {
        move.active.SetState(Multistat.type.ACTIVATING_PATTERN, false);
        yield break;
    }
}