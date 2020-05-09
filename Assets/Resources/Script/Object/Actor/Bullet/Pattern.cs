using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pattern
{
    public Actor owner;

    public bool isPatternRunning;

    public float preDelay;
    public float postDelay;

    // priority 높을수록 패턴 발동확률 증가 0 = 0%
    // 패턴 발동 시 currentpriority 0으로 초기화
    public int priority = 1;
    public int currentPriority = 1;

    public Pattern(Actor _owner)
    {
        owner = _owner;
    }

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
    public Actor firePrefab;

    public Actor posRoot;
    public Vector2 position;
    public Vector2 deltaPos;

    public Actor dirRoot;
    public float direction;
    public float deltaDir;

    public int count = 1;
    protected int firedCount = 0;
    public float term = 0f;
    public int distnaceFromRoot = 0;

    public PatternFire(Actor _owner)
        : base(_owner) { }

    public virtual void PreFireProcess() { }

    public override IEnumerator Fire()
    {
        if (posRoot != null) position = posRoot.transform.position;
        if (dirRoot != null) direction = dirRoot._targetDirection;

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
        Actor actor = GameObject.Instantiate(firePrefab);
        if (actor == null) return;

        if(actor is Bullet)
        {
            (actor as Bullet).owner = owner as Unit;
        }
        else if(actor is Module)
        {
            (actor as Module).owner = owner as Unit;
        }

        if (posRoot != null) position = posRoot.transform.position;
        actor.transform.position = position + deltaPos;

        if (dirRoot != null) direction = dirRoot._targetDirection;
        actor._targetDirection = direction + deltaDir;
        actor._moveDirection = direction + deltaDir;
    }
}

// update 될 수 있는 owner 방향으로 발사
public class PatternFireDirection : PatternFire
{
    public Vector2 ownerPos;

    public PatternFireDirection(Actor _owner)
        : base(_owner) { }

    public override void PreFireProcess()
    {
        if (owner != null) direction = owner._targetDirection;
    }
}

// update 될 수 있는 target 위치로 발사
public class PatternFireTarget : PatternFire
{
    public Actor target;
    public Vector2 targetPos;

    public PatternFireTarget(Actor _owner)
        : base(_owner) { }

    public override void PreFireProcess()
    {
        if(target == null &&
            owner != null)
            target = owner.GetOperable<Targetable>().target;

        if (target != null) targetPos = target.transform.position;
        direction = VEasyCalculator.GetDirection(position, targetPos);
    }
}

public class PatternFireTarget_AngleRandom : PatternFireTarget
{
    public float angle = 360f;

    public PatternFireTarget_AngleRandom(Actor _owner)
        : base(_owner) { }

    public override void PreFireProcess()
    {
        if (target == null &&
            owner != null)
            target = owner.GetOperable<Targetable>().target;

        if (target != null) targetPos = target.transform.position;
        direction = VEasyCalculator.GetDirection(position, targetPos);

        deltaDir = angle * (Random.Range(0f, 1f) - 0.5f);
    }
}

public class PatternFireTarget_RowRandom : PatternFireTarget
{
    public float length = 3f;

    public PatternFireTarget_RowRandom(Actor _owner)
        : base(_owner) { }

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

    public PatternFireAngleRandom(Actor _owner)
        : base(_owner) { }

    public override void PreFireProcess()
    {
        if (count == 1) return;

        deltaDir = angle * (Random.Range(0f, 1f) - 0.5f);
    }
}

public class PatternFireRowRandom : PatternFire
{
    public float length = 3f;

    public PatternFireRowRandom(Actor _owner)
        : base(_owner) { }

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

    public PatternFireCircle(Actor _owner)
        : base(_owner) { }

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

    public PatternFireAngle(Actor _owner)
        : base(_owner) { }

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

    public PatternFireRow(Actor _owner)
        : base(_owner) { }

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

    public Pattern_Slayer_1(Unit _owner)
        :base(_owner)
    {
        postDelay = 3f;

        move = _owner.GetOperable<Movable>();

        PatternFireTarget_AngleRandom pattern1 = new PatternFireTarget_AngleRandom(_owner);

        GameObject go = ResourcesManager<GameObject>.LoadResource(
            ResourcesManager<GameObject>.ResourceName.Bullet_Slayer_1);

        Bullet_Slayer_1 bullet = go.GetComponent<Bullet_Slayer_1>();
        bullet.owner = _owner;

        pattern1.owner = _owner;

        pattern1.firePrefab = bullet;

        float duration = 5f;

        pattern1.count = 80;
        pattern1.term = duration / pattern1.count;
        pattern1.angle = 110f;

        pattern1.posRoot = _owner;
        pattern1.dirRoot = _owner;

        patternList.Add(pattern1);

        PatternFire[] pattern2 = new PatternFire[2];
        for (int i = 0; i < 2; ++i)
        {
            pattern2[i] = new PatternFire(_owner);
            pattern2[i].firePrefab = bullet;

            pattern2[i].count = 30;
            pattern2[i].term = duration / pattern2[i].count;
            if (i == 0) pattern2[i].deltaDir = -60f;
            else pattern2[i].deltaDir = 60f;

            pattern2[i].posRoot = _owner;
            pattern2[i].dirRoot = _owner;

            patternList.Add(pattern2[i]);
        }
    }

    public override IEnumerator Fire()
    {
        move.state.SetState(Multistat.StateType.ACTIVATING_PATTERN, true);

        GameManager.gm.StartCoroutine(patternList[0].Fire());
        GameManager.gm.StartCoroutine(patternList[1].Fire());
        yield return GameManager.gm.StartCoroutine(patternList[2].Fire());
    }

    private Actor.RotateTo originRotateTo;
    public override IEnumerator PostFire()
    {
        owner._rotateTo = originRotateTo;
        move.state.SetState(Multistat.StateType.ACTIVATING_PATTERN, false);
        yield break;
    }

    public override IEnumerator PreFire()
    {
        originRotateTo = owner._rotateTo;
        owner._rotateTo = Actor.RotateTo.TARGET;
        yield break;
    }
}

// 전방향 난사
public class Pattern_Slayer_2 : Pattern
{
    public List<Pattern> patternList = new List<Pattern>();

    private const int count = 100;
    private const float duration = 5f;
    private const float term = duration / count;
    private PatternFireCircle[] patterns = new PatternFireCircle[count];
    private Movable move;

    public Pattern_Slayer_2(Unit _owner)
        :base(_owner)
    {
        postDelay = 3f;

        move = _owner.GetOperable<Movable>();

        GameObject go = ResourcesManager<GameObject>.LoadResource(
            ResourcesManager<GameObject>.ResourceName.Bullet_Slayer_2);

        for (int i = 0; i < count; ++i)
        {
            patterns[i] = new PatternFireCircle(_owner);

            patterns[i].firePrefab = go.GetComponent<Bullet>();

            patterns[i].count = 6;
            patterns[i].term = 0f;

            patterns[i].posRoot = _owner;
            patterns[i].dirRoot = null;
            patterns[i].direction = i * 2f;
        }
    }

    public override IEnumerator Fire()
    {
        move.state.SetState(Multistat.StateType.ACTIVATING_PATTERN, true);

        for (int i = 0; i < count; ++i)
        {
            GameManager.gm.StartCoroutine(patterns[i].Fire());
            if (i < count - 1)
                yield return new WaitForSeconds(term);
        }
    }

    public override IEnumerator PostFire()
    {
        move.state.SetState(Multistat.StateType.ACTIVATING_PATTERN, false);
        yield break;
    }
}