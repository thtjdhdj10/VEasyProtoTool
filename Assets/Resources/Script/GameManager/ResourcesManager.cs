using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager<T> where T : Object
{
    public static Dictionary<ResourceName, T> resourceDic
        = new Dictionary<ResourceName, T>();

    public List<T> loadedResourceList = new List<T>();

    public const string RESOURCES = "Resources";
    public static string MIDDLE_PATH = "";
    public static string EXTENSION = "";

    public static T LoadResource(ResourceName name)
    {
        if (resourceDic.ContainsKey(name))
            return resourceDic[name];
        return default;
    }

    // 파일이름인식해서 prefabName의 타입이랑 일치하는거 있으면 prefabDic에 Add
    // 모든 prefab 다 할 필요는 없고, 필요할 때 마다 추가
    public enum ResourceName
    {
        NONE = 0,

        // prefab
        
        Bullet_Laser,
        Bullet_Slayer_1,
        Bullet_Slayer_2,
        Bullet_Straight_1,
        Bullet_Straight_2,

        Effect_Bullet,
        Effect_LaserBody,
        Effect_LaserRoot,
        Effect_Shield,
        Effect_ShieldBreak,

        Enemy_Wing,
        EnemyBoss_Slayer,

        Player,

        // sprite

        Player_Damaged_strip5,

        // controller

        Effect_Laser_Column_Controller,
        Effect_Laser_Root__Controller,

        EnemyBoss_Slayer__Controller,

        Player_Damaged_Controller,
    }

    public enum Option
    {
        NONE = 0,
        FORECE_PLAYER,
        FORECE_ENEMY,
    }

    public ResourcesManager()
    {
        // prefab
        if(typeof(T) == typeof(GameObject))
        {
            MIDDLE_PATH = "Prefabs";
            EXTENSION = "*.prefab";
        }
        else if(typeof(T) == typeof(Sprite))
        {
            MIDDLE_PATH = "Sprite";
            EXTENSION = "*.png";
        }
        else if(typeof(T) == typeof(RuntimeAnimatorController))
        {
            MIDDLE_PATH = "Sprite/Anim";
            EXTENSION = "*.controller";
        }

        LoadResources();
    }

    private void LoadResources()
    {
        loadedResourceList.Clear();

        string fullPath = Application.dataPath + "/" +
            RESOURCES + "/" +
            MIDDLE_PATH + "/";

        List<string> resourceNameWithPathList = new List<string>(System.IO.Directory.GetFiles(fullPath, EXTENSION));

        // Directory 이하의 Directory 들을 가져옴
        string[] targetDirectoryWithPath = System.IO.Directory.GetDirectories(fullPath);
        for (int i = 0; i < targetDirectoryWithPath.Length; ++i)
        {
            resourceNameWithPathList.AddRange(new List<string>(
                System.IO.Directory.GetFiles(targetDirectoryWithPath[i], EXTENSION)));
        }

        for (int j = 0; j < resourceNameWithPathList.Count; ++j)
        {
            // resource.load 를 위한 이름
            string resourceName = GetResourceName(resourceNameWithPathList[j]);

            ResourceName resourceType = ResourceName.NONE;
            if (System.Enum.TryParse(resourceName, out resourceType))
            {
                string resourceLoadName = GetLoadingName(resourceNameWithPathList[j]);
                T r = Resources.Load<T>(resourceLoadName);
                resourceDic.Add(resourceType, r);
                loadedResourceList.Add(r);
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
