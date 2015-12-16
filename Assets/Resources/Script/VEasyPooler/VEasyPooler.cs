using UnityEngine;
using System.Collections.Generic;

using Debug = UnityEngine.Debug;

public class VEasyPooler : MonoBehaviour
{
    public string originName;
    public string originTag;

//    public int inActiveCount = 0;

    int inActiveCount = 0;
    public int InActiveCount
    {
        get
        {
            return inActiveCount;
        }
        set
        {
            int delta = value - inActiveCount;
            inActiveCount = value;

            VEasyPoolerManager.manager.countOfAll += delta;
        }
    }

    int activeCount = 0;
    public int ActiveCount
    {
        get
        {
            return activeCount;
        }
        set
        {
            int delta = value - activeCount;
            activeCount = value;

            VEasyPoolerManager.manager.countOfAll += delta;
        }
    }

    private int objectNumber = 0;

    public List<GameObject> objectList = new List<GameObject>();
    // lists front are inactive objects, backward are active objects

    GameObject modelObject = null;

    void ExactlyLog(string str)
    {
        if (VEasyPoolerManager.manager.useDebugFlow == false)
            return;

        if (VEasyPoolerManager.IsExclude(originName, originTag) == true)
            return;

        Debug.Log(str);
    }

    public GameObject GetModelObject()
    {
        return modelObject;
    }

    public void SetModelObject(string name)
    {
        if (modelObject != null) return;

        originName = name;

        Object prefab = null;

        for (int i = 0; i < VEasyPoolerManager.manager.includePrefabPath.Count; ++i)
        {
            if (VEasyPoolerManager.manager.includePrefabPath[i] != "")
                prefab = Resources.Load(VEasyPoolerManager.manager.includePrefabPath[i] + "/" + originName, typeof(GameObject));
            else
                prefab = Resources.Load(originName, typeof(GameObject));

            if (prefab != null)
                break;
        }

        if (prefab == null)
        {
            Debug.LogWarning("\"" + originName + "\" is unvalid prefab name");
            originName = null;
            return;
        }

        ExactlyLog("new pool for " + originName);

        modelObject = MonoBehaviour.Instantiate(prefab) as GameObject;

        if (modelObject == null)
        {
            Debug.LogError(modelObject.name + " instantiate fail");
            originName = null;
            return;
        }

        originTag = modelObject.tag;

        ObjectState modelState = modelObject.AddComponent<ObjectState>();
        
        modelState.IsUse = false;
        modelState.originalName = originName;

        modelObject.name = originName + "(Origin)";
        modelObject.transform.parent = gameObject.transform;
    }

    // add

    public void AddObjectFromHierarchyRequest(List<GameObject> list)
    {
        ExactlyLog("add " + originName + " * " + list.Count);

        for(int i = 0; i < list.Count; ++i)
        {
            ObjectState state = list[i].GetComponent<ObjectState>();
            if (state == null)
            {
                state = list[i].AddComponent<ObjectState>();
                state.IsUse = true;
                state.indexOfPool = InActiveCount + i;
                state.originalName = originName;
            }

            list[i].name = originName + "_" + objectNumber++;
            if (VEasyPoolerManager.manager.visualizeObjectList == true)
                list[i].transform.parent = gameObject.transform;
        }

        ActiveCount += list.Count;

        objectList.AddRange(list);
    }

    // create

    public void CreateInactiveObjectRequset(int count)
    {
        ExactlyLog("create inActive " + originName + " * " + count);

        for (int i = InActiveCount; i < InActiveCount + ActiveCount; ++i)
        {
            objectList[i].name = originName + "_" + objectNumber++;

            ObjectState state = objectList[i].GetComponent<ObjectState>();
            state.indexOfPool = i + count;
        }

        List<GameObject> newObjects = new List<GameObject>(count);
        
        for(int i = 0; i < count; ++i)
        {
            GameObject obj = MonoBehaviour.Instantiate(modelObject) as GameObject;

            if(obj == null)
            {
                Debug.LogError("GameObject instantiate fail");
                return;
            }


            ObjectState state = obj.GetComponent<ObjectState>();
            state.IsUse = false;
            state.indexOfPool = InActiveCount + i;
            state.originalName = originName;

            obj.name = originName + "_" + objectNumber++;
            if (VEasyPoolerManager.manager.visualizeObjectList == true)
                obj.transform.parent = gameObject.transform;

            newObjects.Add(obj);
        }

        objectList.InsertRange(InActiveCount, newObjects);

        InActiveCount += count;
    }

    public List<GameObject> CreateActiveObjectRequset(int count, bool active)
    {
        ExactlyLog("create active " + originName + " * " + count);

        for (int i = 0; i < count; ++i)
        {
            GameObject obj = MonoBehaviour.Instantiate(modelObject) as GameObject;

            if (obj == null)
            {
                Debug.LogError("GameObject instantiate fail");
                return null;
            }


            ObjectState state = obj.GetComponent<ObjectState>();
            state.IsUse = active;
            state.indexOfPool = objectList.Count;
            state.originalName = originName;

            obj.name = originName + "_" + objectNumber++;
            if (VEasyPoolerManager.manager.visualizeObjectList == true)
                obj.transform.parent = gameObject.transform;

            objectList.Add(obj);
        }

        ActiveCount += count;

        return objectList.GetRange(objectList.Count - count, count);
    }

    // get

    public List<GameObject> GetObjectRequest(int count, bool active)
    {
        return GetObjectRequest(count, active,
            modelObject.transform.position,
            modelObject.transform.eulerAngles,
            modelObject.transform.localScale);
    }

    public List<GameObject> GetObjectRequest(int count, bool active, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        ExactlyLog("get " + originName + " * " + count);

        int needCount = count - InActiveCount;

        if (needCount > 0)
        {
            List<GameObject> retObjects = CreateActiveObjectRequset(needCount, active);
            if (retObjects == null) return null;

            int countMinusNeed = count - needCount;

            if (countMinusNeed > 0)
            {
                int startIdx = InActiveCount - countMinusNeed;

                List<GameObject> addList = GetObjectList(startIdx, countMinusNeed, active, pos, rot, scale);

                retObjects.AddRange(addList);
            }

            return retObjects;
        }
        else
        {
            int getIndex = InActiveCount - count;

            return GetObjectList(getIndex, count, active, pos, rot, scale);
        }
    }

    private List<GameObject> GetObjectList(int startIdx, int count, bool active, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        InActiveCount -= count;
        ActiveCount += count;

        for (int i = startIdx; i < startIdx + count; ++i)
        {
            ObjectState state = objectList[i].GetComponent<ObjectState>();
            state.IsUse = active;
        }

        if(VEasyPoolerManager.manager.getOnResetTransform == true)
        {
            for (int i = startIdx; i < startIdx + count; ++i)
            {
                objectList[i].transform.position = pos;
                objectList[i].transform.eulerAngles = rot;
                objectList[i].transform.localScale = scale;
            }
        }

        return objectList.GetRange(startIdx, count);
    }

    public int GetObjectCountRequest(bool active)
    {
        if (active == true)
            return ActiveCount;
        else return InActiveCount;
    }

    // get finite

    public List<GameObject> GetFiniteObjectRequest(int count, bool active, float lifeTime)
    {
        return GetFiniteObjectRequest(count, active, lifeTime,
            modelObject.transform.position,
            modelObject.transform.eulerAngles,
            modelObject.transform.localScale);
    }

    public List<GameObject> GetFiniteObjectRequest(int count, bool active, float lifeTime, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        List<GameObject> list = GetObjectRequest(count, active, pos, rot, scale);

        for(int i = 0; i < count; ++i)
        {
            ObjectState state = list[i].GetComponent<ObjectState>();
            state.SetReleaseTimer(lifeTime);
        }

        return list;
    }

    // release

    public void ReleaseObjectRequest(GameObject obj)
    {
        ExactlyLog("release " + obj.name);

        List<GameObject> list = new List<GameObject>(1);
        list.Add(obj);
        ReleaseObjectRequest(list);
    }

    public void ReleaseObjectRequest(List<GameObject> obj)
    {
        ExactlyLog("release " + originName + " * " + obj.Count);

        for (int i = 0; i < obj.Count; ++i)
        {

            ObjectState releaseObjState = obj[i].GetComponent<ObjectState>();
            if (releaseObjState == null)
            {
                Debug.LogWarning("release request fail");
                Debug.LogWarning("\"" + obj[i].name + "\" have not ObjectState script");
                continue;
            }
            
            releaseObjState.IsUse = false;

            int relIdx = releaseObjState.indexOfPool;
            if(relIdx < InActiveCount || relIdx >= objectList.Count)
            {
                Debug.LogWarning("release request fail");
                Debug.LogWarning("\"" + obj[i].name + "\" this object already released");
                //
                return;
            }

            int changeIdx = InActiveCount;
            if (changeIdx == relIdx)
            {
                InActiveCount += 1;
                ActiveCount -= 1;
                continue;
            }

            ObjectState changeObjState = objectList[changeIdx].GetComponent<ObjectState>();

            GameObject tempObj = objectList[relIdx];
            objectList[relIdx] = objectList[changeIdx];
            objectList[changeIdx] = tempObj;

            int tempInt = releaseObjState.indexOfPool;
            releaseObjState.indexOfPool = changeObjState.indexOfPool;
            changeObjState.indexOfPool = tempInt;

            InActiveCount += 1;
            ActiveCount -= 1;
        }
    }

    //
}

