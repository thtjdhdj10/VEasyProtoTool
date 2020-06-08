using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UObject = UnityEngine.Object;
using NameTypePair = System.Collections.Generic.KeyValuePair<string, System.Type>;

namespace VEPT
{
    // TODO: scriptable object 등을 이용해서 awake 시점이 아닌 에디터 시점에서 리소스 불러오게 하기
    // Resources 하위 경로에 있는 모든 폴더를 검사해서
    // LoadResource() 로 적재된 리소스를 불러올 수 있다
    public class ResourcesManager
    {
        // 파일이름인식해서 prefabName의 타입이랑 일치하는거 있으면 prefabDic에 Add
        // 모든 prefab 다 할 필요는 없고, 필요할 때 마다 추가
        private static Dictionary<NameTypePair, UObject> _resNameObjDic =
            new Dictionary<NameTypePair, UObject>();

        private List<UObject> _loadedResDic = new List<UObject>();
        private const string RESOURCES = "Resources";

        public static T LoadResource<T>(EResourceName resourceType) where T : UObject
        {
            return LoadResource<T>(resourceType.ToString());
        }

        // Note: Sprite 를 UnityEngine.Object 로 캐스팅이 안됨
        public static T LoadResource<T>(string resourceName) where T : UObject
        {
            NameTypePair nameType = new NameTypePair(resourceName, typeof(T));

            try
            {
                return _resNameObjDic[nameType] as T;
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError(e);
                return default;
            }
        }

        private static string ResourceTypeToExtension(Type type)
        {
            // TODO 리소스 타입이 추가될 때 마다 수정 1
            if (type == typeof(GameObject))
                return "*.prefab";
            if (type == typeof(Sprite))
                return "*.png";
            if (type == typeof(RuntimeAnimatorController))
                return "*.controller";

            return "";
        }

        public ResourcesManager()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            _loadedResDic.Clear();

            string dataPath = Application.dataPath;

            List<string> subDirectories = Directory.GetDirectories(
                dataPath + "/" + RESOURCES, "*", SearchOption.AllDirectories).ToList();

            // TODO 리소스 타입이 추가될 때 마다 수정 2
            LoadResources<GameObject>(subDirectories);
            LoadResources<Sprite>(subDirectories);
            LoadResources<RuntimeAnimatorController>(subDirectories);
        }

        private void LoadResources<T>(List<string> subDirectories) where T : UObject
        {
            List<string> resNameWithPathList = new List<string>();

            string extension = ResourceTypeToExtension(typeof(T));

            // 하위 경로의 파일명 로딩
            subDirectories.ForEach(subPath =>
                resNameWithPathList.AddRange(Directory.GetFiles(subPath, extension)));

            resNameWithPathList.ForEach(resourcePath =>
            {
                NameTypePair nameType = new NameTypePair(
                    GetResourceName(resourcePath), typeof(T));

                T resource = Resources.Load<T>(GetLoadingName(resourcePath));

                try
                {
                    _resNameObjDic.Add(nameType, resource);
                    _loadedResDic.Add(resource);
                }
                catch(ArgumentException e)
                {
                    Debug.LogError(e);
                }
            });
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
}