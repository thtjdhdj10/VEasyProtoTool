using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class PoolerManager : SingletonComponent<PoolerManager>
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

        public static List<GameObject> GetObjectsRequest(EResourceName originalName, int count)
        {
            return Instance.GetObjectPooler(originalName).GetObjects(count);
        }

        public static List<GameObject> GetObjectsRequest(string originalName, int count)
        {
            return Instance.GetObjectPooler(originalName).GetObjects(count);
        }

        public static GameObject GetObjectRequest(EResourceName originalName)
        {
            return Instance.GetObjectPooler(originalName).GetObject();
        }

        public static GameObject GetObjectRequest(string originalName)
        {
            return Instance.GetObjectPooler(originalName).GetObject();
        }

        public static void ReleaseObjectRequest(List<Object> objs, EResourceName originalName)
        {
            Instance.GetObjectPooler(originalName).ReleaseObject(objs);
        }

        public static void ReleaseObjectRequest(List<Object> objs, string originalName)
        {
            Instance.GetObjectPooler(originalName).ReleaseObject(objs);
        }

        public static void ReleaseObjectRequest(List<GameObject> objs, EResourceName originalName)
        {
            Instance.GetObjectPooler(originalName).ReleaseObject(objs);
        }

        public static void ReleaseObjectRequest(List<GameObject> objs, string originalName)
        {
            Instance.GetObjectPooler(originalName).ReleaseObject(objs);
        }

        public static void ReleaseObjectRequest(Object obj, EResourceName originalName)
        {
            Instance.GetObjectPooler(originalName).ReleaseObject(obj);
        }

        public static void ReleaseObjectRequest(Object obj, string originalName)
        {
            Instance.GetObjectPooler(originalName).ReleaseObject(obj);
        }

        public static void ReleaseObjectRequest(GameObject obj, EResourceName originalName)
        {
            Instance.GetObjectPooler(originalName).ReleaseObject(obj);
        }

        public static void ReleaseObjectRequest(GameObject obj, string originalName)
        {
            Instance.GetObjectPooler(originalName).ReleaseObject(obj);
        }

        // 하이어라키에 배치된 오브젝트 추가하는 용도
        public static  void AssignObjectRequest(GameObject obj, EResourceName originalName)
        {
            Instance.GetObjectPooler(originalName).AssignObject(obj);
        }

        public static void AssignObjectRequest(GameObject obj, string originalName)
        {
            Instance.GetObjectPooler(originalName).AssignObject(obj);
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