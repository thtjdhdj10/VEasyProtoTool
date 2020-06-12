using UnityEngine;
using System.Collections;

namespace VEPT
{
    public class UnitStatus : MonoBehaviour
    {
        public Unit owner;

        public bool enableVitalColor; // 남은체력에따라 컬러변경
        [SerializeField]
        private bool enableHpDisplay = false;
        public EVitalSign vital;
        public SpriteRenderer sprite;

        public int hp;
        [SerializeField]
        private int currentHp;
        public int CurrentHp
        {
            get
            {
                return currentHp;
            }
            set
            {
                currentHp = value;
                if (currentHp <= 0)
                {
                    owner.willDestroy = true;
                }

                vital = GetVitalSign();
                if (enableVitalColor)
                {
                    sprite.color = GetVitalColor();
                }
            }
        }

        public enum EVitalSign
        {
            RED, // 100%~80%
            ORANGE, // 80%~50%
            YELLOW, // 50%~20%
            WHITE, // 20%~0%
        }

        private void Awake()
        {
            // TODO 에디터에서 찾게 수정
            if (owner == null) owner = gameObject.GetComponent<Unit>();
            owner.unitStatus = this;

            if (enableVitalColor && sprite == null)
                sprite = GetComponent<SpriteRenderer>();

            if (enableHpDisplay)
            {
                var pos = (Vector2)transform.position + new Vector2(0, 0.3f);
                TextDisplayer.AssignNewText(gameObject, "", new Color(1, 0, 0)
                    , pos, new Vector2(50, 20), TextAnchor.MiddleCenter);
            }
        }

        private void Update()
        {
            if (enableHpDisplay)
            {
                var pos = (Vector2)transform.position + new Vector2(0, 0.3f);
                TextDisplayer.UpdateText(gameObject, "HP: " + CurrentHp, pos);
            }
        }

        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            TextDisplayer.RemoveText(gameObject);
        }

        public virtual void Init()
        {
            CurrentHp = hp;
            if (CurrentHp <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public Color GetVitalColor()
        {
            Color ret = sprite.color;

            switch (vital)
            {
                case EVitalSign.RED:
                    ret.r = 1f;
                    ret.g = 0f;
                    ret.b = 0f;
                    break;
                case EVitalSign.ORANGE:
                    ret.r = 1f;
                    ret.g = 0.4f;
                    ret.b = 0f;
                    break;
                case EVitalSign.YELLOW:
                    ret.r = 1f;
                    ret.g = 0.85f;
                    ret.b = 0f;
                    break;
                case EVitalSign.WHITE:
                    ret.r = 1f;
                    ret.g = 1f;
                    ret.b = 1f;
                    break;
            }

            return ret;
        }

        public EVitalSign GetVitalSign()
        {
            float remainHpPercentage = (float)currentHp / hp;
            if (remainHpPercentage > 0.8f)
                return EVitalSign.RED;
            else if (remainHpPercentage > 0.5f)
                return EVitalSign.ORANGE;
            else if (remainHpPercentage > 0.2f)
                return EVitalSign.YELLOW;
            return EVitalSign.WHITE;
        }
    }
}