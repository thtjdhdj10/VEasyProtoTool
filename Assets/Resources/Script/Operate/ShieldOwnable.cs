using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEPT
{
    public class ShieldOwnable : Operable
    {
        public bool enableShield = true;

        public int maxShieldCount = 1;
        public int ShieldCount
        {
            get
            {
                return _shieldCount;
            }
            set
            {
                _shieldCount = value;
                if (_shieldCount > 0) enableShield = true;
                else enableShield = false;
            }
        }
        [SerializeField]
        private int _shieldCount = 1;

        public float shieldRegenDelay;
        private float _regenProgress;

        private List<GameObject> _shieldList = new List<GameObject>();

        //

        protected override void Awake()
        {
            base.Awake();

            enableShield = false;

            for (int i = 0; i < ShieldCount; ++i)
            {
                GameObject shield = Instantiate(ResourcesManager.LoadResource<GameObject>(
                    EResourceName.Effect_Shield));
                shield.transform.position = transform.position;
                shield.transform.parent = transform;
                _shieldList.Add(shield);

                enableShield = true;
            }
        }

        private void FixedUpdate()
        {
            if (_shieldCount < maxShieldCount)
            {
                if (_regenProgress < shieldRegenDelay)
                {
                    _regenProgress += Time.fixedDeltaTime;
                }
                else
                {
                    _regenProgress = 0f;
                    ++_shieldCount;

                    GameObject shield = Instantiate(ResourcesManager.LoadResource<GameObject>(
                        EResourceName.Effect_Shield));
                    shield.transform.position = transform.position;
                    shield.transform.parent = transform;
                    _shieldList.Add(shield);
                }
            }
        }

        public bool ShieldBreak()
        {
            if (_shieldCount > 0)
            {
                --_shieldCount;

                GameObject shield = _shieldList[_shieldList.Count - 1];
                _shieldList.Remove(shield);
                Destroy(shield);

                return true;
            }
            else return false;
        }
    }
}