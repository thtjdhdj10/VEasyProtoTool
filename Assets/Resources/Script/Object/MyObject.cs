using UnityEngine;
using System.Collections.Generic;

using UObject = UnityEngine.Object;

namespace VEPT
{
    public class MyObject : MonoBehaviour
    {
        // ObjectPooler 사용 시 SetActive로 생성/삭제 처리 되므로
        // Awake/OnDestroy 대신 OnEnable/OnDisable 사용
        //private void Awake() { }
        //private void OnDestroy() { }
        protected virtual void OnEnable()
        {
            Init();
        }
        protected virtual void OnDisable() { }

        public virtual void Init() { }

        public new GameObject Instantiate(UObject obj)
        {
            return ObjectPoolerManager.GetObjectRequest(obj);
        }

        public new void Destroy(UObject obj)
        {
            ObjectPoolerManager.ReleaseObjectRequest(obj);
        }
    }
}