using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

// last update: 2017 02 24

public class VEasyPoolerManager : MonoBehaviour
{
    public bool useDebugFlow = true;
    public bool getOnResetTransform = true;

    // true: can`t change object`s parents
    // don`t change at runtime
    public bool visualizeObjectList = false;
    
    public int countOfAll = 0;

    [System.Serializable]
    private struct NameCount
    {
        public string name;
        public int count;
    }

    [SerializeField]
    private List<NameCount> prePoolingList = new List<NameCount>();
    public List<string> poolingFromHierarchy = new List<string>();
    public List<string> excludeLogTags = new List<string>();
    public List<string> excludeLogNames = new List<string>();
    public List<string> includePrefabPath = new List<string>();

    static Dictionary<string, VEasyPooler> poolDic = new Dictionary<string, VEasyPooler>();

    public static VEasyPoolerManager manager;


    public enum TargetObject
    {
        ACTIVE_ONLY = 0,
        INACTIVE_ONLY,
        BOTH_OBJECT,
    }

    void Awake()
    {
        manager = this;

        for (int i = 0; i < poolingFromHierarchy.Count; ++i)
        {
            PoolingObjectFromHierarchyRequest(poolingFromHierarchy[i]);
        }

        for (int i = 0; i < prePoolingList.Count; ++i)
        {
            CreateInactiveObjectRequset(prePoolingList[i].name, prePoolingList[i].count);
        }

    }

    public delegate void ProcessingFunction(GameObject obj);

    public static void ProcessFunctionToObjects(ProcessingFunction func, string name, TargetObject to)
    {
        if (IsValidArgs(name) == false)
            return;

        List<GameObject> list = poolDic[name].objectList;

        int count = 0;
        int startIndex = 0;

        if (to == TargetObject.ACTIVE_ONLY)
        {
            count = poolDic[name].ActiveCount;
            startIndex = poolDic[name].InActiveCount;
        }
        else if (to == TargetObject.INACTIVE_ONLY)
            count = poolDic[name].InActiveCount;
        else
            count = poolDic[name].ActiveCount + poolDic[name].InActiveCount;

        for (int i = startIndex; i < startIndex + count; ++i)
        {
            func(list[i]);
        }
    }

    // add

    public static void PoolingObjectFromHierarchyRequest(string name)
    {
        if (IsValidArgs(name) == false)
            return;

        GameObject[] gameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        List<GameObject> objectList = new List<GameObject>();

        for (int i = 0;i < gameObjects.Length; ++i)
        {
            // include same name, name (1), name (2) ...
            if(gameObjects[i].name == name ||
                gameObjects[i].name.Contains(name + " (") &&
                gameObjects[i].name.Contains(" " + name) == false)
            {
                if (gameObjects[i].GetComponent<ObjectState>() == null)
                    objectList.Add(gameObjects[i]);
            }
        }

        if (objectList.Count == 0)
            return;

        poolDic[name].AddObjectFromHierarchyRequest(objectList);
    }

    // create

    public static void CreateInactiveObjectRequset(string name)
    {
        CreateInactiveObjectRequset(name, 1);
    }

    public static void CreateInactiveObjectRequset(string name, int count)
    {
        if (IsValidArgs(name, count) == false)
            return;

        poolDic[name].CreateInactiveObjectRequset(count);
    }

    // get

    public static int GetObjectCountRequest(string name, bool active)
    {
        if (IsValidArgs(name) == false)
            return 0;

        return poolDic[name].GetObjectCountRequest(active);
    }

    //

    public static List<GameObject> RefObjectListAtLayer(int layerMask)
    {
        List<GameObject> retList = new List<GameObject>();

        foreach (var key in poolDic.Keys)
        {
            GameObject obj = poolDic[key].GetModelObject();
            var layerSetting = obj.GetComponent<LayerSetting>();
            if (layerSetting == null)
                continue;

            if (System.Convert.ToBoolean(layerSetting.layerMask & layerMask) == true)
            {
                retList.AddRange(poolDic[key].objectList);
            }
        }

        return retList;
    }

    // get object

    public static GameObject GetObjectRequest(string name)
    {
        List<GameObject> list = GetObjectListRequest(name, 1);

        if (list == null) return null;
        return list[0];
    }

    public static GameObject GetObjectRequest(string name, bool active)
    {
        List<GameObject> list = GetObjectListRequest(name, 1, active);

        if (list == null) return null;
        return list[0];
    }

    // get object list

    public static List<GameObject> GetObjectListRequest(string name, int count)
    {
        return GetObjectListRequest(name, count, true);
    }

    public static List<GameObject> GetObjectListRequest(string name, int count, bool active)
    {
        if (IsValidArgs(name, count) == false)
            return null;

        return poolDic[name].GetObjectRequest(count, active);
    }

    // get finite

    public static GameObject GetFiniteObjectRequest(string name, float lifeTime)
    {
        List<GameObject> list = GetFiniteObjectListRequest(name, 1, true, lifeTime);

        if (list == null) return null;
        return list[0];
    }

    public static GameObject GetFiniteObjectRequest(string name, bool active, float lifeTime)
    {
        List<GameObject> list = GetFiniteObjectListRequest(name, 1, active, lifeTime);

        if (list == null) return null;
        return list[0];
    }

    public static List<GameObject> GetFiniteObjectListRequest(string name, int count, float lifeTime)
    {
        return GetFiniteObjectListRequest(name, count, true, lifeTime);
    }

    public static List<GameObject> GetFiniteObjectListRequest(string name, int count, bool active, float lifeTime)
    {
        if (IsValidArgs(name, count, lifeTime) == false)
            return null;

        return poolDic[name].GetFiniteObjectRequest(count, active, lifeTime);
    }

    // get modified

    public static GameObject GetModifiedObjectRequest(string name, Vector3 pos)
    {
        Vector3 rot = new Vector3();
        Vector3 scale = new Vector3();

        GameObject model = poolDic[name].GetModelObject();
        if (model != null)
        {
            rot = model.transform.eulerAngles;
            scale = model.transform.localScale;
        }

        return GetModifiedObjectRequest(name, pos, rot, scale);
    }

    public static GameObject GetModifiedObjectRequest(string name, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        if (IsValidArgs(name) == false)
            return null;

        List<GameObject> list = poolDic[name].GetObjectRequest(1, true, pos, rot, scale);

        if (list == null) return null;
        return list[0];
    }

    // get modified finite

    public static GameObject GetModifiedFiniteObjectRequest(string name, float lifeTime, Vector3 pos )
    {
        Vector3 rot = new Vector3();
        Vector3 scale = new Vector3();

        GameObject model = poolDic[name].GetModelObject();
        if(model != null)
        {
            rot = model.transform.eulerAngles;
            scale = model.transform.localScale;
        }

        return GetModifiedFiniteObjectRequest(name, lifeTime, pos, rot, scale);
    }

    public static GameObject GetModifiedFiniteObjectRequest(string name, float lifeTime, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        if (IsValidArgs(name, lifeTime) == false)
            return null;

        List<GameObject> list = poolDic[name].GetFiniteObjectRequest(1, true, lifeTime, pos, rot, scale);

        if (list == null) return null;
        return list[0];
    }

    // release

    public static void ReleaseObjectRequest(GameObject obj)
    {
        List<GameObject> list = new List<GameObject>(1);
        list.Add(obj);

        ReleaseObjectRequest(list);
    }

    public static void ReleaseObjectRequest(List<GameObject> obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("release request fail: target is null object.");
            return;
        }

        if (obj.Count == 0)
        {
            Debug.LogWarning("release request fail: list.Count is zero.");
            return;
        }
        
        ObjectState state = obj[0].GetComponent<ObjectState>();
        if (state == null)
        {
            Debug.LogWarning("release request fail: " + obj[0].name + " don`t have ObjectState script.");
            return;
        }
        
        string name = state.originalName;
        if(IsValidArgs(name) == false)
        {
            Debug.LogWarning("release request fail: " + "\"" + name + "\" is invalid key.");
            return;
        }

        if (obj.Count == 1)
            poolDic[name].ReleaseObjectRequest(obj[0]);
        else
            poolDic[name].ReleaseObjectRequest(obj);
    }

    // release with clear

    public static void ReleaseObjectWithClearRequest(GameObject obj)
    {
        List<GameObject> list = new List<GameObject>(1);
        list.Add(obj);

        ReleaseObjectWithClearRequest(list);
    }

    public static void ReleaseObjectWithClearRequest(List<GameObject> obj)
    {
        ReleaseObjectRequest(obj);
        obj.Clear();
    }

    //

    public static bool IsExclude(string name, string tag)
    {
        if (IsExcludeName(name) == true ||
            IsExcludeTag(tag) == true)
        {
            return true;
        }
        return false;
    }

    public static bool IsExcludeName(string name)
    {
        for (int i = 0; i < manager.excludeLogNames.Count; ++i)
        {
            if (manager.excludeLogNames[i] == name)
                return true;
        }

        return false;
    }

    public static bool IsExcludeTag(string tag)
    {
        for (int i = 0; i < manager.excludeLogTags.Count; ++i)
        {
            if (manager.excludeLogTags[i] == tag)
                return true;
        }

        return false;
    }

    private static bool IsValidArgs(string name, int count)
    {
        if (IsValidArgs(name) == false ||
            IsValidArgs(count) == false)
        {
            return false;
        }
        return true;
    }

    private static bool IsValidArgs(string name, float lifeTime)
    {
        if (IsValidArgs(name) == false ||
            IsValidArgs(lifeTime) == false)
        {
            return false;
        }
        return true;
    }

    private static bool IsValidArgs(string name, int count, float lifeTime)
    {
        if (IsValidArgs(name) == false ||
            IsValidArgs(count) == false ||
            IsValidArgs(lifeTime) == false)
        {
            return false;
        }
        return true;
    }

    private static bool IsValidArgs(string name)
    {
        if (name == null)
            return false;

        if (poolDic.ContainsKey(name) == false)
        {
            GameObject poolObj = new GameObject();
            poolObj.name = "pool " + name;

            VEasyPooler poolScript = poolObj.AddComponent<VEasyPooler>();
            poolDic.Add(name, poolScript);

            poolDic[name].SetModelObject(name);

            if (poolDic[name].originName == null)
            {
                poolDic.Remove(name);
                Destroy(poolObj);
                return false;
            }

            poolObj.transform.parent = manager.transform;
        }

        return true;
    }

    private static bool IsValidArgs(int count)
    {
        if (count < 0)
        {
            Debug.LogWarning(count + " is invalid Object count.");
            return false;
        }
        return true;
    }

    private static bool IsValidArgs(float lifeTime)
    {
        if (lifeTime < 0.0f)
        {
            Debug.LogWarning(lifeTime + " is invalid Object lifetime.");
            return false;
        }
        return true;
    }
}