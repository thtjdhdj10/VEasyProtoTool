using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcesManager : MonoBehaviour
{
    // resources load 로 프리팹 다 불러와서 dictionary에 넣음
    public Dictionary<PrefabName, GameObject> prefabDic
        = new Dictionary<PrefabName, GameObject>();

    public int loadedPrefabCount;
    public List<GameObject> loadedPrefabList = new List<GameObject>();

    public const string RESOURCES = "Resources";
    public const string PREFABS = "Prefabs";


    // 파일이름인식해서 prefabName의 타입이랑 일치하는거 있으면 prefabDic에 Add
    public enum PrefabName
    {
        NONE = 0,
        Enemy_Wing,
        EnemyBoss_Slayer,
    }

    public enum Option
    {
        NONE = 0,
        FORECE_PLAYER,
        FORECE_ENEMY,
    }

    private void Awake()
    {


        
    }

    //public static GameObject LoadGameObject<T>()
    //{

    //}

    private void LoadPrefabs()
    {
        loadedPrefabCount = 0;

        loadedPrefabList.Clear();

        string fullPath = Application.dataPath + "/" +
            RESOURCES + "/" +
            PREFABS + "/";

        // Sprite Directory 이하의 Directory 들을 가져옴
        string[] targetDirectoryWithPath = System.IO.Directory.GetDirectories(fullPath);

        for (int i = 0; i < targetDirectoryWithPath.Length; ++i)
        {
            // directory 들 이하의 png file 들을 가져옴
            string[] spriteNameWithPath = System.IO.Directory.GetFiles(targetDirectoryWithPath[i], "*.prefab");

            for (int j = 0; j < spriteNameWithPath.Length; ++j)
            {
                for (int k = 0; k < splitName.Length; ++k)
                {
                    strType[k] = splitName[k];
                }

                SpriteAttribute sa = new SpriteAttribute();

                // resource.load 를 위한 이름
                string resourceLoadName = GetLoadingName(spriteNameWithPath[j]);
                sa.sprite = Resources.Load<Sprite>(resourceLoadName);

                sa.frameCount = GetSpriteFrameCount(strType);
                sa.speed = GetSpriteSpeed(strType, sa.frameCount);
                sa.cycle = (1f / (float)spriteDefaultFramePerSec) / sa.speed * (float)(sa.frameCount - 1);

                CutSpriteAttribute(ref strType);

                //

                if (sa.sprite == null)
                {
                    CustomLog.CompleteLogWarning(
                        "Invalid Sprite: " + resourceLoadName,
                        PRINT_DEBUG);

                    continue;
                }

                loadingSpriteNameList.Add(strType[0] + " " + strType[1] + " " + strType[2]);

                string category = strType[0];
                string name = strType[1];
                string status = strType[2];

                if (typeSpriteDic.ContainsKey(category) == false)
                    typeSpriteDic.Add(category, new Dictionary<string, Dictionary<string, SpriteAttribute>>());
                if (typeSpriteDic[category].ContainsKey(name) == false)
                    typeSpriteDic[category].Add(name, new Dictionary<string, SpriteAttribute>());
                if (typeSpriteDic[category][name].ContainsKey(status) == false)
                    typeSpriteDic[category][name].Add(status, sa);
                ++countLoadSprite;
            }
        }

        Debug.Log("Load Sprite Count: " + countLoadSprite);
    }
}
