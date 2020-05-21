using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UObject = UnityEngine.Object;
using NameTypePair = System.Collections.Generic.KeyValuePair<string, System.Type>;

// Resources 하위 경로에 있는 모든 폴더를 검사해서
// RESOURCE_TYPE_ARR 에 속하는 타입의 리소스를 저장하여 관리
public class ResourcesManager
{
    // 파일이름인식해서 prefabName의 타입이랑 일치하는거 있으면 prefabDic에 Add
    // 모든 prefab 다 할 필요는 없고, 필요할 때 마다 추가
    public enum EResName
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
        Effect_Shockwave,

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

    private static Dictionary<NameTypePair, UObject> _resNameObjDic =
        new Dictionary<NameTypePair, UObject>();
    private List<UObject> _loadedResDic = new List<UObject>();
    private const string RESOURCES = "Resources";
    private static Type[] RESOURCE_TYPE_ARR = {
        typeof(GameObject),
        typeof(Sprite),
        typeof(RuntimeAnimatorController),
    };

    private static string ResourceTypeToExtension(Type type)
    {
        if (type == typeof(GameObject))
            return "*.prefab";
        if (type == typeof(Sprite))
            return "*.png";
        if (type == typeof(RuntimeAnimatorController))
            return "*.controller";

        return "";
    }

    public static T LoadResource<T>(EResName resourceType) where T : UObject
    {
        return LoadResource<T>(resourceType.ToString());
    }

    public static T LoadResource<T>(string resourceName) where T : UObject
    {
        NameTypePair nameType = new NameTypePair(resourceName, typeof(GameObject));
        if (_resNameObjDic.ContainsKey(nameType))
            return _resNameObjDic[nameType] as T;

        return default;
    }

    public ResourcesManager()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        _loadedResDic.Clear();

        string dataPath = Application.dataPath;

        foreach (var resType in RESOURCE_TYPE_ARR)
        {
            string[] subDirectories = Directory.GetDirectories(
                dataPath + "/" + RESOURCES, "*", SearchOption.AllDirectories);

            List<string> resNameWithPathList = new List<string>();

            string extension = ResourceTypeToExtension(resType);

            // 하위 경로의 파일명 로딩
            subDirectories.ToList().ForEach(subPath =>
                resNameWithPathList.AddRange(Directory.GetFiles(subPath, extension)));

            resNameWithPathList.ForEach(resourcePath =>
            {
                UObject resource = Resources.Load<UObject>(GetLoadingName(resourcePath));
                _resNameObjDic.Add(new NameTypePair(
                    GetResourceName(resourcePath), resType), resource);
                _loadedResDic.Add(resource);
            });
        }
    }

    private string GetResourceName(string fullName)
    {
        string[] resourceName = fullName.Split('/', '\\');
        return resourceName[resourceName.Length - 1].Split('.')[0];
    }

    private string GetLoadingName(string fullName)
    {
        string[] resourceNameArr = fullName.Split('/', '\\');

        string loadingName = "";

        // Resouces 하위의 상대경로 + 파일이름( 확장자명 제외 )
        bool isAfterResources = false;
        foreach (var resName in resourceNameArr)
        {
            if (isAfterResources) loadingName += resName + "/";
            if (resName == RESOURCES) isAfterResources = true;
        }
        return loadingName.Split('.')[0];
    }
}
