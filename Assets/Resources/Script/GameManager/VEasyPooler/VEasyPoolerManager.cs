using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class VEasyPoolerManager : SingletonComponent<VEasyPoolerManager>
    {
        [SerializeField]
        private bool categorizePooledObject = true;
        public static bool CategorizePooledObject
            { get => Instance.categorizePooledObject; }

        // TODO 카테고라이즈 안할것들
        // public List<EResourceName> nonCategorizingObjects = new List<EResourceName>();

        private static Dictionary<string, VEasyPooler> poolerDic =
            new Dictionary<string, VEasyPooler>();

        //public bool useDebugFlow = true;
        //public bool getOnResetTransform = true;

        //// use this, than can changing object`s parents
        //// please NOT change whan runtime
        //public bool visualizeObjectList = false;

        //public int countOfAll = 0;

        //[System.Serializable]
        //private struct NameCount
        //{
        //    public string name;
        //    public int count;
        //}

        //[SerializeField]
        //private List<NameCount> prePoolingList = new List<NameCount>();
        //public List<string> poolingFromHierarchy = new List<string>();
        //public List<string> excludeLogTags = new List<string>();
        //public List<string> excludeLogNames = new List<string>();
        //public List<string> includePrefabPath = new List<string>();

        public enum TargetObject
        {
            ACTIVE_ONLY = 0,
            INACTIVE_ONLY,
            BOTH_OBJECT,
        }

        protected override void Awake()
        {
            base.Awake();
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

        //

        private VEasyPooler GetObjectPooler(EResourceName type)
        {
            string name = type.ToString();

            if (poolerDic.TryGetValue(name, out VEasyPooler ret))
                return ret;

            var pooler = new VEasyPooler(type);
            poolerDic.Add(name, pooler);

            return pooler;
        }

        private VEasyPooler GetObjectPooler(string name)
        {
            if (poolerDic.TryGetValue(name, out VEasyPooler ret))
                return ret;

            var pooler = new VEasyPooler(name);
            poolerDic.Add(name, pooler);

            return pooler;
        }

        //public delegate void ProcessingFunction(GameObject obj);

        //public static void ProcessFunctionToObjects(ProcessingFunction func, string name, TargetObject to)
        //{
        //    if (IsValidArgs(name) == false)
        //        return;

        //    List<GameObject> list = poolerDic[name].objectList;

        //    int count = 0;
        //    int startIndex = 0;

        //    if (to == TargetObject.ACTIVE_ONLY)
        //    {
        //        count = poolerDic[name].ActiveCount;
        //        startIndex = poolerDic[name].InActiveCount;
        //    }
        //    else if (to == TargetObject.INACTIVE_ONLY)
        //        count = poolerDic[name].InActiveCount;
        //    else
        //        count = poolerDic[name].ActiveCount + poolerDic[name].InActiveCount;

        //    for (int i = startIndex; i < startIndex + count; ++i)
        //    {
        //        func(list[i]);
        //    }
        //}

        //// add

        //public static void PoolingObjectFromHierarchyRequest(string name)
        //{
        //    if (IsValidArgs(name) == false)
        //        return;

        //    GameObject[] gameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        //    List<GameObject> objectList = new List<GameObject>();

        //    for (int i = 0; i < gameObjects.Length; ++i)
        //    {
        //        // include same name, name (1), name (2) ...
        //        if (gameObjects[i].name == name ||
        //            gameObjects[i].name.Contains(name + " (") &&
        //            gameObjects[i].name.Contains(" " + name) == false)
        //        {
        //            if (gameObjects[i].GetComponent<ObjectState>() == null)
        //                objectList.Add(gameObjects[i]);
        //        }
        //    }

        //    if (objectList.Count == 0)
        //        return;

        //    poolerDic[name].AddObjectFromHierarchyRequest(objectList);
        //}

        //// create

        //public static void CreateInactiveObjectRequset(string name)
        //{
        //    CreateInactiveObjectRequset(name, 1);
        //}

        //public static void CreateInactiveObjectRequset(string name, int count)
        //{
        //    if (IsValidArgs(name, count) == false)
        //        return;

        //    poolerDic[name].CreateInactiveObjectRequset(count);
        //}

        //// get

        //public static int GetObjectCountRequest(string name, bool active)
        //{
        //    if (IsValidArgs(name) == false)
        //        return 0;

        //    return poolerDic[name].GetObjectCountRequest(active);
        //}

        ////

        //public static List<GameObject> RefObjectListAtLayer(int layerMask)
        //{
        //    List<GameObject> retList = new List<GameObject>();

        //    foreach (var key in poolerDic.Keys)
        //    {
        //        GameObject obj = poolerDic[key].GetModelObject();
        //        var layerSetting = obj.GetComponent<LayerSetting>();
        //        if (layerSetting == null)
        //            continue;

        //        Debug.Log(obj.name + " " + layerMask + " " + layerSetting.layerMask);

        //        if (System.Convert.ToBoolean(layerSetting.layerMask & layerMask) == true)
        //        {
        //            retList.AddRange(poolerDic[key].objectList);
        //        }
        //    }

        //    return retList;
        //}

        //// get object

        ////public static GameObject GetObjectRequest(string name)
        ////{
        ////    List<GameObject> list = GetObjectListRequest(name, 1);

        ////    if (list == null) return null;
        ////    return list[0];
        ////}

        //public static GameObject GetObjectRequest(string name, bool active)
        //{
        //    List<GameObject> list = GetObjectListRequest(name, 1, active);

        //    if (list == null) return null;
        //    return list[0];
        //}

        //// get object list

        //public static List<GameObject> GetObjectListRequest(string name, int count)
        //{
        //    return GetObjectListRequest(name, count, true);
        //}

        //public static List<GameObject> GetObjectListRequest(string name, int count, bool active)
        //{
        //    if (IsValidArgs(name, count) == false)
        //        return null;

        //    return poolerDic[name].GetObjectRequest(count, active);
        //}

        //// get modified

        //public static GameObject GetModifiedObjectRequest(string name, Vector3 pos)
        //{
        //    Vector3 rot = new Vector3();
        //    Vector3 scale = new Vector3();

        //    GameObject model = poolerDic[name].GetModelObject();
        //    if (model != null)
        //    {
        //        rot = model.transform.eulerAngles;
        //        scale = model.transform.localScale;
        //    }

        //    return GetModifiedObjectRequest(name, pos, rot, scale);
        //}

        //public static GameObject GetModifiedObjectRequest(string name, Vector3 pos, Vector3 rot, Vector3 scale)
        //{
        //    if (IsValidArgs(name) == false)
        //        return null;

        //    List<GameObject> list = poolerDic[name].GetObjectRequest(1, true, pos, rot, scale);

        //    if (list == null) return null;
        //    return list[0];
        //}

        //// release

        //public static void ReleaseObjectRequest(GameObject obj)
        //{
        //    List<GameObject> list = new List<GameObject>(1);
        //    list.Add(obj);

        //    ReleaseObjectRequest(list);
        //}

        //public static void ReleaseObjectRequest(List<GameObject> obj)
        //{
        //    if (obj == null)
        //    {
        //        Debug.LogWarning("release request fail");
        //        return;
        //    }

        //    if (obj.Count == 0)
        //    {
        //        Debug.LogWarning("release request fail");
        //        Debug.LogWarning("wrong release request");
        //        Debug.LogWarning("this list.Count is zero");
        //        return;
        //    }

        //    ObjectState state = obj[0].GetComponent<ObjectState>();
        //    if (state == null)
        //    {
        //        Debug.LogWarning("release request fail");
        //        Debug.LogError(obj[0].name + " have not ObjectState script");
        //        return;
        //    }

        //    string name = state.originalName;
        //    if (IsValidArgs(name) == false)
        //    {
        //        Debug.LogWarning("release request fail");
        //        Debug.LogError("\"" + name + "\" is unvalid key");
        //        return;
        //    }

        //    if (obj.Count == 1)
        //        poolerDic[name].ReleaseObjectRequest(obj[0]);
        //    else
        //        poolerDic[name].ReleaseObjectRequest(obj);
        //}

        //// release with clear

        //public static void ReleaseObjectWithClearRequest(GameObject obj)
        //{
        //    List<GameObject> list = new List<GameObject>(1);
        //    list.Add(obj);

        //    ReleaseObjectWithClearRequest(list);
        //}

        //public static void ReleaseObjectWithClearRequest(List<GameObject> obj)
        //{
        //    ReleaseObjectRequest(obj);
        //    obj.Clear();
        //}

        ////

        //public static bool IsExclude(string name, string tag)
        //{
        //    if (IsExcludeName(name) == true ||
        //        IsExcludeTag(tag) == true)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool IsExcludeName(string name)
        //{
        //    for (int i = 0; i < manager.excludeLogNames.Count; ++i)
        //    {
        //        if (manager.excludeLogNames[i] == name)
        //            return true;
        //    }

        //    return false;
        //}

        //public static bool IsExcludeTag(string tag)
        //{
        //    for (int i = 0; i < manager.excludeLogTags.Count; ++i)
        //    {
        //        if (manager.excludeLogTags[i] == tag)
        //            return true;
        //    }

        //    return false;
        //}

        //private static bool IsValidArgs(string name, int count)
        //{
        //    if (IsValidArgs(name) == false ||
        //        IsValidArgs(count) == false)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private static bool IsValidArgs(string name, float lifeTime)
        //{
        //    if (IsValidArgs(name) == false ||
        //        IsValidArgs(lifeTime) == false)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private static bool IsValidArgs(string name, int count, float lifeTime)
        //{
        //    if (IsValidArgs(name) == false ||
        //        IsValidArgs(count) == false ||
        //        IsValidArgs(lifeTime) == false)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private static bool IsValidArgs(string name)
        //{
        //    if (name == null)
        //        return false;

        //    if (poolerDic.ContainsKey(name) == false)
        //    {
        //        GameObject poolObj = new GameObject();
        //        poolObj.name = "pool " + name;

        //        VEasyPooler poolScript = poolObj.AddComponent<VEasyPooler>();
        //        poolerDic.Add(name, poolScript);

        //        poolerDic[name].SetModelObject(name);

        //        if (poolerDic[name].originName == null)
        //        {
        //            poolerDic.Remove(name);
        //            Destroy(poolObj);
        //            return false;
        //        }

        //        poolObj.transform.parent = manager.transform;
        //    }

        //    return true;
        //}

        //private static bool IsValidArgs(int count)
        //{
        //    if (count < 0)
        //    {
        //        Debug.LogWarning(count + " this objectCount is too little");
        //        return false;
        //    }
        //    return true;
        //}

        //private static bool IsValidArgs(float lifeTime)
        //{
        //    if (lifeTime < 0.0f)
        //    {
        //        Debug.LogWarning(lifeTime + " this lifeTime is too little");
        //        return false;
        //    }
        //    return true;
        //}
    }
}