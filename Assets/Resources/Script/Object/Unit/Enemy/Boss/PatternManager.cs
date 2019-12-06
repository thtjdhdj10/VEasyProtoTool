using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    // TODO 패턴 종류별로 다 구현
    // 패턴함수 딜리게이트로 만들어서 patternfireable에서 갖다쓰기
    // patternfireable 에선 패턴 쿨타임이나 딜레이같은거 관리
    // 

    public void AssignPattern()
    {

    }

    public void ActivatePattern(Pattern pattern)
    {
        GameManager.gm.StartCoroutine(pattern.Fire());
    }

    public delegate void PatternA(PatternOption option);

    private void Awake()
    {
        PatternA pa;
        pa = new PatternA();

        
    }

    public void asdf(int a)
    {
        
    }


}

public abstract class Pattern
{
    public abstract IEnumerator Fire();
}

public class PatternRepeat : Pattern
{
    public Pattern pattern;

    public float term;

    public override IEnumerator Fire()
    {
        throw new System.NotImplementedException();
    }
}

public abstract class PatternFire : Pattern
{
    public Bullet bulletPrefab;

    public Unit rootUnit;
    public Vector2 rootPos;

    public int count = 1;
    protected int firedCount = 0;
    public float direction;
    public float speed;
    public float term = 0f;
    public int distnaceFromRoot = 0;

    public abstract void PreFireProcess();
    public abstract void PostFireProcess();

    public override IEnumerator Fire()
    {
        PreFireProcess();

        for (int i = 0; i < count; ++i)
        {
            FireProcess();

            PostFireProcess();

            ++firedCount;

            if (term > 0f) yield return new WaitForSeconds(term);
        }
    }

    public virtual void FireProcess()
    {
        Bullet bullet = GameObject.Instantiate(bulletPrefab);

        if (rootUnit != null) rootPos = rootUnit.transform.position;
        rootPos = currentPos;
        bullet.transform.position = currentPos;

        Movable bulletMove = bullet.GetOperable<Movable>();
        if (bulletMove != null)
        {
            bulletMove.direction = currentDir;
            bulletMove.speed = speed;
        }
    }
}

// update 될 수 있는 owner 이동방향으로 발사
public class PatternFireDirection : PatternFire
{
    public Unit owner;
    public Vector2 ownerPos;

    public override void PreFireProcess()
    {


    }

    public override void PostFireProcess()
    {
        Bullet bullet = GameObject.Instantiate(bulletPrefab);

        Movable bulletMove = bullet.GetOperable<Movable>();
        if (bulletMove != null)
        {
            bulletMove.direction = direction;
            bulletMove.speed = speed;
        }
    }
}

// update 될 수 있는 target 위치로 발사
public class PatternFireTarget : PatternFire
{
    public Unit target;
    public Vector2 targetPos;

    public override void PreFireProcess()
    {
        
    }

    public override void PostFireProcess()
    {

    }
}

// 방사
public class PatternFireEmission : PatternFire
{
    public float angle = 360f;
    public bool isClockwise = false;

    private float deltaAngle;

    public override void PreFireProcess()
    {

    }

    public override void PostFireProcess()
    {
        float indexDir = (float)firedCount / (count - 1);
        indexDir -= 0.5f;
        deltaAngle = angle * indexDir;
    }
}

// 일렬로 발사
public class PatternFireRow : PatternFire
{
    public float length = 3f;
    public bool isLeftToRight = true;

    private Vector2 deltaPos;

    public override void PreFireProcess()
    {
        throw new System.NotImplementedException();
    }

    public override void PostFireProcess()
    {
        currentPos += deltaPos;
    }

    public override void FireProcess()
    {
        Bullet bullet = GameObject.Instantiate(bulletPrefab);

        if (rootUnit != null) rootPos = rootUnit.transform.position;
        bullet.transform.position = currentPos;

        Movable bulletMove = bullet.GetOperable<Movable>();
        if (bulletMove != null)
        {
            bulletMove.direction = currentDir;
            bulletMove.speed = speed;
        }
    }
}