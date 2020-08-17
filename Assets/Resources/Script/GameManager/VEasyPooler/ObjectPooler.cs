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

        // [0]~[nActived - 1]: actived objects
        // [nActived]~[^1]: deactived objects
        private List<GameObject> objectList = new List<GameObject>();

        private int nActived;
        private int nDeactived;
        private int IdxFirstActived { get => 0; }
        private int IdxLastActived { get => nActived - 1; }
        private int IdxFirstDeactived { get => nActived; }
        private int IdxLastDeactived { get => NObjects - 1; }
        public int NObjects { get => nActived + nDeactived; }

        public ObjectPooler(string name)
        {
            if (ObjectPoolerManager.CategorizePooledObject)
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
            if (nDeactived == 0) return InstanciateOne();
            else return ActivateOne();
        }

        public List<GameObject> GetObject(int count)
        {
            try
            {
                List<GameObject> ret = new List<GameObject>();

                for (int i = 0; i < count - nDeactived; ++i)
                    ret.Add(InstanciateOne());

                for (int i = 0; i < Math.Min(count, nDeactived); ++i)
                    ret.Add(ActivateOne());

                int startIndexOfActivating = IdxFirstDeactived;

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
                SwapSafty(idx, IdxLastActived);

                --nActived;
                ++nDeactived;
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
                ++nActived;

                SwapSafty(IdxLastDeactived, IdxLastActived);
            }
            else
            {
                objectList.Add(obj);
                ++nDeactived;
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

        private GameObject InstanciateOne()
        {
            try
            {
                GameObject obj = UObject.Instantiate(prefab) as GameObject;
                objectList.Insert(IdxFirstDeactived, obj);
                ++nActived;

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
            if (ObjectPoolerManager.CategorizePooledObject)
            {
                obj.transform.parent = rootObject.transform;

                rootObject.name = string.Format("{0} ({1})", prefabName, objectList.Count);
            }
        }

        private void AddPooledObjectComponent(GameObject obj)
        {
            if (obj.GetComponent<ObjectInfo>() == null)
            {
                var po = obj.AddComponent<ObjectInfo>();
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
                GameObject obj = objectList[IdxFirstDeactived];
                obj.SetActive(true);

                --nDeactived;
                ++nActived;

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
                idxA > IdxLastDeactived || idxB > IdxLastDeactived)
                return;

            GameObject t = objectList[idxA];
            objectList[idxA] = objectList[idxB];
            objectList[idxB] = t;
        }
    }
}