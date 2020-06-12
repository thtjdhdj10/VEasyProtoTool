using UnityEngine;
using System.Collections.Generic;
using System;

using UObject = UnityEngine.Object;

namespace VEPT
{
    public class ObjectPoolerManager : SingletonComponent<ObjectPoolerManager>
    {
        [SerializeField]
        private bool categorizePooledObject = true;
        public static bool CategorizePooledObject
            { get => Instance.categorizePooledObject; }

        // TODO 카테고라이즈 안할것들
        // public List<EResourceName> nonCategorizingObjects = new List<EResourceName>();

        private static Dictionary<string, ObjectPooler> poolerDic =
            new Dictionary<string, ObjectPooler>();

        #region public method

        public static List<GameObject> GetObjectsRequest(EResourceName originName, int count)
        {
            return Instance.GetObjectPooler(originName).GetObjects(count);
        }

        public static List<GameObject> GetObjectsRequest(string originName, int count)
        {
            return Instance.GetObjectPooler(originName).GetObjects(count);
        }

        public static List<GameObject> GetObjectsRequest(UObject obj, int count)
        {
            if (obj is GameObject go)
            {
                return GetObjectsRequest(go, count);
            }
            else
            {
                Debug.LogError("GetObjectsRequest() Failure: " + obj.name +
                    " is not <GameObject>");
                return null;
            }
        }

        public static List<GameObject> GetObjectsRequest(GameObject obj, int count)
        {
            try
            {
                var info = obj.GetComponent<ObjectInfo>();
                var originName = info.originName;
                return Instance.GetObjectPooler(originName).GetObjects(count);
            }
            catch (NullReferenceException)
            {
                Debug.LogWarning("GetObjectRequest() Caution: " + obj.name +
                    " have not <ObjetInfo>");

                List<GameObject> objList = new List<GameObject>();
                for (int i = 0; i < count; ++i)
                    objList.Add(Instantiate(obj));

                return objList;
            }
        }

        public static GameObject GetObjectRequest(EResourceName originName)
        {
            return Instance.GetObjectPooler(originName).GetObject();
        }

        public static GameObject GetObjectRequest(string originName)
        {
            return Instance.GetObjectPooler(originName).GetObject();
        }

        public static GameObject GetObjectRequest(UObject obj)
        {
            if (obj is GameObject go)
            {
                return GetObjectRequest(go);
            }
            else
            {
                Debug.LogError("GetObjectRequest() Failure: " + obj.name +
                    " is not <GameObject>");
                return null;
            }
        }

        public static GameObject GetObjectRequest(GameObject obj)
        {
            try
            {
                var info = obj.GetComponent<ObjectInfo>();
                var originName = info.originName;
                return Instance.GetObjectPooler(originName).GetObject();
            }
            catch(NullReferenceException)
            {
                Debug.LogWarning("GetObjectRequest() Caution: " + obj.name +
                    " have not <ObjetInfo>");

                return Instantiate(obj);
            }
        }

        public static void ReleaseObjectRequest(List<UObject> objs, EResourceName originName)
        {
            Instance.GetObjectPooler(originName).ReleaseObject(objs);
        }

        public static void ReleaseObjectRequest(List<UObject> objs, string originName)
        {
            Instance.GetObjectPooler(originName).ReleaseObject(objs);
        }

        public static void ReleaseObjectRequest(List<GameObject> objs, EResourceName originName)
        {
            Instance.GetObjectPooler(originName).ReleaseObject(objs);
        }

        public static void ReleaseObjectRequest(List<GameObject> objs, string originName)
        {
            Instance.GetObjectPooler(originName).ReleaseObject(objs);
        }

        public static void ReleaseObjectRequest(UObject obj, EResourceName originName)
        {
            Instance.GetObjectPooler(originName).ReleaseObject(obj);
        }

        public static void ReleaseObjectRequest(UObject obj, string originName)
        {
            Instance.GetObjectPooler(originName).ReleaseObject(obj);
        }

        public static void ReleaseObjectRequest(UObject obj)
        {
            if (obj is GameObject go)
            {
                ReleaseObjectRequest(go);
            }
            else
            {
                Debug.LogWarning("ReleaseObjectRequest() Caution: " + obj.name +
                    " is not <GameObject>");

                Destroy(obj);
            }
        }

        public static void ReleaseObjectRequest(GameObject obj, EResourceName originName)
        {
            Instance.GetObjectPooler(originName).ReleaseObject(obj);
        }

        public static void ReleaseObjectRequest(GameObject obj, string originName)
        {
            Instance.GetObjectPooler(originName).ReleaseObject(obj);
        }

        public static void ReleaseObjectRequest(GameObject obj)
        {
            try
            {
                var info = obj.GetComponent<ObjectInfo>();
                var originName = info.originName;
                Instance.GetObjectPooler(originName).ReleaseObject(obj);
            }
            catch (NullReferenceException)
            {
                Debug.LogWarning("ReleaseObjectRequest() Caution: " + obj.name +
                    " have not <ObjetInfo>");

                Destroy(obj);
            }
        }

        // 하이어라키에 배치된 오브젝트 추가하는 용도
        public static  void AssignObjectRequest(GameObject obj, EResourceName originName)
        {
            Instance.GetObjectPooler(originName).AssignObject(obj);
        }

        public static void AssignObjectRequest(GameObject obj, string originName)
        {
            Instance.GetObjectPooler(originName).AssignObject(obj);
        }

        #endregion

        private ObjectPooler GetObjectPooler(EResourceName type)
        {
            string name = type.ToString();

            if (poolerDic.TryGetValue(name, out ObjectPooler ret))
                return ret;

            var pooler = new ObjectPooler(type);
            poolerDic.Add(name, pooler);

            return pooler;
        }

        private ObjectPooler GetObjectPooler(string name)
        {
            if (poolerDic.TryGetValue(name, out ObjectPooler ret))
                return ret;

            var pooler = new ObjectPooler(name);
            poolerDic.Add(name, pooler);

            return pooler;
        }
    }
}