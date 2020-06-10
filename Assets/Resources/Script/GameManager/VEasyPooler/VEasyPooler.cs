using UnityEngine;
using System.Collections.Generic;
using System;

namespace VEPT
{
    public class VEasyPooler
    {
        private GameObject prefab;
        private string prefabName;
        private EResourceName prefabType;

        // [0]~[actived - 1]: actived objects
        // [actived]~[^1]: inactived objects
        private List<GameObject> objectList = new List<GameObject>();

        private int actived;
        private int inactived;
        private int StartIdxActived { get => 0; }
        private int LastIdxActived { get => actived - 1; }
        private int StartIdxInactived { get => actived; }
        private int LastIdxInactived { get => All - 1; }
        public int All { get => actived + inactived; }

        public VEasyPooler(string name)
        {
            // prefab 생성
            prefabName = name;

            prefab = ResourcesManager.LoadResource<GameObject>(name);
        }

        public VEasyPooler(EResourceName name)
        {
            prefabType = name;
            prefabName = name.ToString();

            prefab = ResourcesManager.LoadResource<GameObject>(name);
        }

        public GameObject GetObject()
        {
            try
            {
                if (inactived == 0)
                    objectList.Add(AddInactiveObject());

                GameObject obj = objectList[StartIdxInactived];

                Activate(obj);

                return obj;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public List<GameObject> GetObject(int count)
        {
            try
            {
                if (inactived < count)
                {
                    int required = inactived - count;
                    for (int i = 0; i < required; ++i)
                    {
                        objectList.Add(AddInactiveObject());
                    }
                }

                int startIndexOfActivating = StartIdxInactived;

                for (int i = startIndexOfActivating; i < startIndexOfActivating + count; ++i)
                {
                    Activate(objectList[i]);
                }

                return objectList.GetRange(startIndexOfActivating, count);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public bool ReleaseObject(GameObject obj)
        {
            int idx = objectList.IndexOf(obj);

            if (idx != -1 && obj.activeSelf == false)
            {
                obj.SetActive(false);
                SwapSafty(idx, LastIdxActived);

                --actived;
                ++inactived;

                return true;
            }

            return false;
        }

        // false: 하나 이상이 release 실패함
        public bool ReleaseObject(List<GameObject> objs)
        {
            for(int i = 0; i < objs.Count;++i)
            {
                int idx = objectList.IndexOf(objs[i]);
                if (idx != -1 && objs[i].activeSelf == false)
                {
                    objectList[i].SetActive(false);
                    SwapSafty(idx, LastIdxActived);

                    --actived;
                    ++inactived;
                }
                else return false;
            }

            return true;
        }

        public void AssignObject(GameObject obj)
        {
            AddPooledObjectComponent(obj);

            if (obj.activeSelf)
            {
                objectList.Add(obj);
                ++actived;

                SwapSafty(LastIdxInactived, LastIdxActived);
            }
            else
            {
                objectList.Add(obj);
                ++inactived;
            }
        }

        //

        public void PrintObjectList()
        {
            Debug.Log("Pooled Object Name: " + prefabName);
            Debug.Log("Pooled Object Count: " + objectList.Count);
            string state = "Pooled Object State: ";
            objectList.ForEach(o => state += o.activeSelf ? 'T' : 'F');
            Debug.Log(state);
        }

        //

        private void InitObject(GameObject obj)
        {
            // TODO
        }

        private GameObject AddInactiveObject()
        {
            try
            {
                GameObject obj = GameObject.Instantiate(prefab) as GameObject;
                obj.SetActive(false);
                objectList.Add(obj);
                ++inactived;

                AddPooledObjectComponent(obj);

                return obj;
            }
            catch(NullReferenceException e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        private void AddPooledObjectComponent(GameObject obj)
        {
            if (VEasyPoolerManager.Instance.usePoolingObjectComponent &&
                obj.GetComponent<PooledObject>() == null)
            {
                var po = obj.AddComponent<PooledObject>();
                po.index = objectList.Count - 1;
                po.originalPrefab = prefab;
            }
        }

        private void Activate(GameObject obj)
        {
            try
            {
                obj.SetActive(true);
                InitObject(obj);
                --inactived;
                ++actived;
            }
            catch (NullReferenceException e)
            {
                Debug.LogError(e);
            }
        }

        private void SwapSafty(int idxA, int idxB)
        {
            if (idxA < 0 || idxB < 0 ||
                idxA > LastIdxInactived || idxB > LastIdxInactived)
                return;

            GameObject t = objectList[idxA];
            objectList[idxA] = objectList[idxB];
            objectList[idxB] = t;
        }
    }
}