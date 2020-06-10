using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class MyObject : MonoBehaviour
    {
        public virtual void Init() { }

        public new void Destroy(Object obj)
        {
            GameObject go = obj as GameObject;
            PooledObject po = go.GetComponent<PooledObject>();
            if (po == null)
            {
                Debug.LogWarning(obj.name + " is not pooled object");
                Object.Destroy(obj);
            }
            else
            {
                VEasyPoolerManager.ReleaseObjectRequest(obj, po.originName);
            }
        }
    }
}