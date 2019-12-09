using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcesManager : MonoBehaviour
{
    // resources load 로 프리팹 다 불러와서 dictionary에 넣음
    public static Dictionary<PrefabName, GameObject> prefabDic
        = new Dictionary<PrefabName, GameObject>();

    public List<GameObject> loadedPrefabList = new List<GameObject>();

    public const string RESOURCES = "Resources";
    public const string PREFABS = "Prefabs";

    public static GameObject LoadGameObject(PrefabName name)
    {
        if (prefabDic.ContainsKey(name))
            return prefabDic[name];
        return null;
    }

    // 파일이름인식해서 prefabName의 타입이랑 일치하는거 있으면 prefabDic에 Add
    public enum PrefabName
    {
        NONE = 0,
        
        Enemy_Wing,
        EnemyBoss_Slayer,
        
        Bullet_Laser,
        Bullet_Straight_1,
        Bullet_Straight_2,
        Bullet_Slayer_1,
        Bullet_Slayer_2,

        Effect_LaserRoot,
        Effect_LaserBody,
    }

    public enum Option
    {
        NONE = 0,
        FORECE_PLAYER,
        FORECE_ENEMY,
    }

    private void Awake()
    {
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        loadedPrefabList.Clear();

        string fullPath = Application.dataPath + "/" +
            RESOURCES + "/" +
            PREFABS + "/";

        // Prefabs Directory 이하의 Directory 들을 가져옴
        string[] targetDirectoryWithPath = System.IO.Directory.GetDirectories(fullPath);

        for (int i = 0; i < targetDirectoryWithPath.Length; ++i)
        {
            // directory 들 이하의 prefab file 들을 가져옴
            string[] resourceNameWithPath = System.IO.Directory.GetFiles(targetDirectoryWithPath[i], "*.prefab");

            for (int j = 0; j < resourceNameWithPath.Length; ++j)
            {
                // resource.load 를 위한 이름
                string resourceName = GetResourceName(resourceNameWithPath[j]);

                PrefabName prefabName = (PrefabName)System.Enum.Parse(typeof(PrefabName), resourceName);
                if (prefabName != PrefabName.NONE)
                {
                    string resourceLoadName = GetLoadingName(resourceNameWithPath[j]);
                    GameObject go = Resources.Load<GameObject>(resourceLoadName);
                    prefabDic.Add(prefabName, go);
                    loadedPrefabList.Add(go);
                }
            }
        }
    }

    private string GetResourceName(string fullName)
    {
        string[] resourceName = fullName.Split('/', '\\');
        return resourceName[resourceName.Length - 1].Split('.')[0];
    }

    private string GetLoadingName(string fullName)
    {
        string[] resourceName = fullName.Split('/', '\\');

        string loadingName = "";

        // Resouces 하위의 상대경로 + 파일이름( 확장자명 제외 )

        bool b = false;
        for (int i = 0; i < resourceName.Length; ++i)
        {
            if (resourceName[i] == RESOURCES)
            {
                b = true;
                continue;
            }
            if (b)
            {
                loadingName += resourceName[i] + "/";
            }
        }
        return loadingName.Split('.')[0];
    }
}
