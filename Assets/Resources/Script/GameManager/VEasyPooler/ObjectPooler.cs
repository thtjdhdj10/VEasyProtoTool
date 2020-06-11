using UnityEngine;
using System.Collections.Generic;
using System;

using UObject = UnityEngine.Object;

namespace VEPT
{
    public class ObjectPooler
    {
        private GameObject rootObject;
        
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

        public ObjectPooler(string name)
        {
            if (PoolerManager.CategorizePooledObject)
                rootObject = new GameObject(name);

            prefabName = name;

            prefab = ResourcesManager.LoadResource<GameObject>(name);
        }

        public ObjectPooler(EResourceName type)
            :this(type.ToString())
        {
            prefabType = type;
        }

        #region public method

        public List<GameObject> GetObjects(int count)
        {
            List<GameObject> ret = new List<GameObject>();

            for (int i = 0; i < count; ++i) ret.Add(GetObject());

            return ret;
        }

        public GameObject GetObject()
        {
            if (inactived == 0) return InstanciateOne();
            else return ActivateOne();
        }

        public List<GameObject> GetObject(int count)
        {
            try
            {
                List<GameObject> ret = new List<GameObject>();

                for (int i = 0; i < count - inactived; ++i)
                    ret.Add(InstanciateOne());

                for (int i = 0; i < Math.Min(count, inactived); ++i)
                    ret.Add(ActivateOne());

                int startIndexOfActivating = StartIdxInactived;

                return ret;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public void ReleaseObject(UObject obj)
        {
            ReleaseObject(obj as GameObject);
        }

        public void ReleaseObject(GameObject obj)
        {
            int idx = objectList.IndexOf(obj);

            if (idx != -1 && obj.activeSelf == true)
            {
                obj.SetActive(false);
                SwapSafty(idx, LastIdxActived);

                --actived;
                ++inactived;
            }
            else
            {
                Debug.LogWarning("ReleaseObjectRequest Failure: " + obj.name);
            }
        }

        public void ReleaseObject(List<UObject> objs)
        {
            objs.ForEach(o => ReleaseObject(o));
        }

        public void ReleaseObject(List<GameObject> objs)
        {
            objs.ForEach(o => ReleaseObject(o));
        }

        public void AssignObject(GameObject obj)
        {
            AddPooledObjectComponent(obj);

            Categorize(obj);

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

        #endregion

        private void InitObject(GameObject obj)
        {
            var mo = obj.GetComponent<MyObject>();
            if (mo != null) mo.Init();
        }

        private GameObject InstanciateOne()
        {
            try
            {
                GameObject obj = UObject.Instantiate(prefab) as GameObject;
                objectList.Insert(StartIdxInactived, obj);
                ++actived;

                AddPooledObjectComponent(obj);

                Categorize(obj);

                return obj;
            }
            catch(NullReferenceException e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        private void Categorize(GameObject obj)
        {
            if (PoolerManager.CategorizePooledObject)
            {
                obj.transform.parent = rootObject.transform;

                rootObject.name = string.Format("{0} ({1})", prefabName, objectList.Count);
            }
        }

        private void AddPooledObjectComponent(GameObject obj)
        {
            if (obj.GetComponent<PooledObject>() == null)
            {
                var po = obj.AddComponent<PooledObject>();
                po.index = objectList.Count - 1;
                po.origin = prefab;
                po.originName = prefabName;

                obj.name = string.Format("{0} ({1})", prefabName, po.index);
            }
        }

        private GameObject ActivateOne()
        {
            try
            {
                GameObject obj = objectList[StartIdxInactived];
                obj.SetActive(true);
                InitObject(obj);

                --inactived;
                ++actived;

                return obj;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(e);
            }
            catch (NullReferenceException e)
            {
                Debug.LogError(e);
            }

            return null;
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