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

public class PtnFire : Pattern
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

    public PtnFire(Actor _owner) : base(_owner) { }

    public virtual void PreFireProcess() { }

    public override IEnumerator Fire()
    {
        if (posRoot != null) position = posRoot.transform.position;
        if (dirRoot != null) direction = dirRoot.targetDirection;

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

        if (dirRoot != null) direction = dirRoot.targetDirection;
        actor.targetDirection = direction + deltaDir;
        actor.moveDirection = direction + deltaDir;
    }
}

// update 될 수 있는 owner 방향으로 발사
public class PtnFireDirection : PtnFire
{
    public Vector2 ownerPos;

    public PtnFireDirection(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if (owner != null) direction = owner.targetDirection;
    }
}

// update 될 수 있는 target 위치로 발사
public class PtnFireTarget : PtnFire
{
    public Actor target;
    public Vector2 targetPos;

    public PtnFireTarget(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if(target == null && owner != null)
            target = owner.GetOperable<Targetable>().target;

        if (target != null) targetPos = target.transform.position;
        direction = VEasyCalculator.GetDirection(position, targetPos);
    }
}

public class PtnFireTarget_AngleRandom : PtnFireTarget
{
    public float angle = 360f;

    public PtnFireTarget_AngleRandom(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if (target == null && owner != null)
            target = owner.GetOperable<Targetable>().target;

        if (target != null) targetPos = target.transform.position;
        direction = VEasyCalculator.GetDirection(position, targetPos);

        deltaDir = angle * (Random.Range(0f, 1f) - 0.5f);
    }
}

public class PtnFireTarget_RowRandom : PtnFireTarget
{
    public float length = 3f;

    public PtnFireTarget_RowRandom(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if (target != null) targetPos = target.transform.position;
        direction = VEasyCalculator.GetDirection(position, targetPos);

        float deltaDistance = length * (Random.Range(0f, 1f) - 0.5f);
        deltaPos = VEasyCalculator.GetRotatedPosition(direction, new Vector2(deltaDistance, 0f));
    }
}

public class PtnFireAngleRandom : PtnFire
{
    public float angle = 360f;

    public PtnFireAngleRandom(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if (count == 1) return;

        deltaDir = angle * (Random.Range(0f, 1f) - 0.5f);
    }
}

public class PtnFireRowRandom : PtnFire
{
    public float length = 3f;

    public PtnFireRowRandom(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if (count == 1) return;

        float deltaDistance = length * (Random.Range(0f, 1f) - 0.5f);
        deltaPos = VEasyCalculator.GetRotatedPosition(direction, new Vector2(deltaDistance, 0f));
    }
}

public class PtnFireCircle : PtnFire
{
    public bool isClockwise = false;

    public PtnFireCircle(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if (count == 1) return;

        float fireIndex = (float)firedCount / count;
        if (isClockwise) fireIndex = -fireIndex;
        deltaDir = 360f * fireIndex;
    }
}

// 샷건처럼 방사
public class PtnFireAngle : PtnFire
{
    public float angle = 120f;
    public bool isClockwise = false;

    public PtnFireAngle(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if (count == 1) return;

        float fireIndex = (float)firedCount / (count - 1) - 0.5f;
        if (isClockwise) fireIndex = -fireIndex;
        deltaDir = angle * fireIndex;
    }
}

// 일렬로 발사
public class PtnFireRow : PtnFire
{
    public float length = 3f;
    public bool isLeftToRight = true;

    public PtnFireRow(Actor _owner) : base(_owner) { }

    public override void PreFireProcess()
    {
        if (count == 1) return;

        float fireIndex = (float)firedCount / (count - 1) - 0.5f;
        if (!isLeftToRight) fireIndex = -fireIndex;
        float deltaDistance = length * fireIndex;
        deltaPos = VEasyCalculator.GetRotatedPosition(direction, new Vector2(deltaDistance, 0f));
    }
}