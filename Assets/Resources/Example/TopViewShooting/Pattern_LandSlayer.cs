using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VEPT
{
    public abstract class Pattern_LandSlayer : Pattern
    {
        public Pattern_LandSlayer(Unit _owner) : base(_owner) { }
    }

    //

    // 보고있는 방향 +-60도로 난사
    public class Ptn_Slayer_1 : Pattern_LandSlayer
    {
        public List<Pattern> patternList = new List<Pattern>();

        private Movable move;

        private const float duration = 5f;
        private const int bulletCount1 = 80;
        private const int bulletCount2 = 30;
        private const float fireAngle = 60f;

        public Ptn_Slayer_1(Unit _owner) : base(_owner)
        {
            postDelay = 3f;

            move = _owner.GetOperable<Movable>();

            PtnFireTarget_AngleRandom pattern1 = new PtnFireTarget_AngleRandom(_owner)
            {
                owner = _owner,
                bulletPrefabName = EResourceName.Bullet_Slayer_1.ToString(),
                count = bulletCount1,
                term = duration / bulletCount1,
                angle = 110f,
                posRoot = _owner,
                dirRoot = _owner,
            };

            patternList.Add(pattern1);


            PtnFire[] pattern2 = new PtnFire[2];
            for (int i = 0; i < 2; ++i)
            {
                pattern2[i] = new PtnFire(_owner)
                {
                    bulletPrefabName = EResourceName.Bullet_Slayer_1.ToString(),
                    count = bulletCount2,
                    posRoot = _owner,
                    dirRoot = _owner,
                    term = duration / bulletCount2,
                    fireAngle = (i == 0) ? -fireAngle : fireAngle,
                };

                patternList.Add(pattern2[i]);
            }
        }

        public override IEnumerator Fire()
        {
            move.state.SetState(MultiState.EStateType.ACTIVATING_PATTERN, true);

            GameManager.Instance.StartCoroutine(patternList[0].Fire());
            GameManager.Instance.StartCoroutine(patternList[1].Fire());
            yield return GameManager.Instance.StartCoroutine(patternList[2].Fire());
        }

        private Actor.ERotateTo originRotateTo;
        public override IEnumerator PostFire()
        {
            owner.rotateTo = originRotateTo;
            move.state.SetState(MultiState.EStateType.ACTIVATING_PATTERN, false);
            yield break;
        }

        public override IEnumerator PreFire()
        {
            originRotateTo = owner.rotateTo;
            owner.rotateTo = Actor.ERotateTo.TARGET;
            yield break;
        }
    }

    // 전방향 난사
    public class Ptn_Slayer_2 : Pattern_LandSlayer
    {
        public List<Pattern> patternList = new List<Pattern>();

        private Movable move;
        private PtnFireCircle[] patterns = new PtnFireCircle[count];

        private const int count = 100;
        private const float duration = 5f;
        private const float term = duration / count;

        public Ptn_Slayer_2(Unit _owner) : base(_owner)
        {
            postDelay = 3f;

            move = _owner.GetOperable<Movable>();

            GameObject go = ResourcesManager.LoadResource<GameObject>(
                EResourceName.Bullet_Slayer_2);

            for (int i = 0; i < count; ++i)
            {
                patterns[i] = new PtnFireCircle(_owner)
                {
                    bulletPrefabName = EResourceName.Bullet_Slayer_2.ToString(),
                    count = 6,
                    term = 0f,
                    posRoot = _owner,
                    dirRoot = null,
                    direction = i * 2f
                };
            }
        }

        public override IEnumerator Fire()
        {
            move.state.SetState(MultiState.EStateType.ACTIVATING_PATTERN, true);

            for (int i = 0; i < count; ++i)
            {
                GameManager.Instance.StartCoroutine(patterns[i].Fire());
                if (i < count - 1)
                    yield return new WaitForSeconds(term);
            }
        }

        public override IEnumerator PostFire()
        {
            move.state.SetState(MultiState.EStateType.ACTIVATING_PATTERN, false);
            yield break;
        }
    }
}