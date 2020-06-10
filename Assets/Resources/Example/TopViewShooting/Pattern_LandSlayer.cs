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

        public Ptn_Slayer_1(Unit _owner) : base(_owner)
        {
            postDelay = 3f;

            move = _owner.GetOperable<Movable>();

            PtnFireTarget_AngleRandom pattern1 = new PtnFireTarget_AngleRandom(_owner);

            GameObject go = ResourcesManager.LoadResource<GameObject>(
                EResourceName.Bullet_Slayer_1);

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

            PtnFire[] pattern2 = new PtnFire[2];
            for (int i = 0; i < 2; ++i)
            {
                pattern2[i] = new PtnFire(_owner);
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

        private const int count = 100;
        private const float duration = 5f;
        private const float term = duration / count;
        private PtnFireCircle[] patterns = new PtnFireCircle[count];
        private Movable move;

        public Ptn_Slayer_2(Unit _owner) : base(_owner)
        {
            postDelay = 3f;

            move = _owner.GetOperable<Movable>();

            GameObject go = ResourcesManager.LoadResource<GameObject>(
                EResourceName.Bullet_Slayer_2);

            for (int i = 0; i < count; ++i)
            {
                patterns[i] = new PtnFireCircle(_owner);

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