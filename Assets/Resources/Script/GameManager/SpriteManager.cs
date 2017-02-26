using UnityEngine;
using System.Collections.Generic;

using TypeSpriteDictionary =
System.Collections.Generic.Dictionary<string,
System.Collections.Generic.Dictionary<string,
System.Collections.Generic.Dictionary<string, SpriteManager.SpriteAttribute>>>;

public class SpriteManager : MonoBehaviour
{

    public const bool PRINT_DEBUG = true;

    public int countLoadSprite;

    public static float spriteDefaultRotation = -90f;

    public static int spriteDefaultFramePerSec = 12;

    public static SpriteManager manager;

    void Awake()
    {
        manager = this;

        LoadSprite();
    }

    public TypeSpriteDictionary typeSpriteDic = new TypeSpriteDictionary();

    public struct SpriteAttribute
    {
        public Sprite sprite;
        public float speed;
        public int frameCount;
        public float cycle;
    }

    [SerializeField]
    private List<string> loadingSpriteNameList = new List<string>();

    [SerializeField]
    private List<string> categoryKeywordList;
    [SerializeField]
    private List<string> nameKeywordList;
    [SerializeField]
    private List<string> statusKeywordList;

    public const string RESOURCES_PATH = "Resources";
    public const string SPRITE_PATH = "Sprite";

    const string EXCLUDE_KEYWORD = "@";

    public void LoadSprite()
    {
        countLoadSprite = 0;

        loadingSpriteNameList.Clear();

        string fullPath = Application.dataPath + "/" +
            RESOURCES_PATH + "/" +
            SPRITE_PATH + "/";

        // Sprite Directory 이하의 Directory 들을 가져옴
        string[] targetDirectoryWithPath = System.IO.Directory.GetDirectories(fullPath);

        for (int i = 0; i < targetDirectoryWithPath.Length; ++i)
        {
            // directory 들 이하의 png file 들을 가져옴
            string[] spriteNameWithPath = System.IO.Directory.GetFiles(targetDirectoryWithPath[i], "*.png");

            for (int j = 0; j < spriteNameWithPath.Length; ++j)
            {
                if (spriteNameWithPath[j] == EXCLUDE_KEYWORD)
                    continue;

                // 4 파트로 나뉜 이름
                string[] strType = { "", "", "", "" };
                string[] splitName = GetSplitName(spriteNameWithPath[j]);
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

        CustomLog.CompleteLog("Load Sprite Count: " + countLoadSprite);
    }

    public static void SetSpriteCycleTime(ref SpriteAttribute sa, float time)
    {
        sa.cycle = time;
        sa.speed = (1f / (float)spriteDefaultFramePerSec) / time * (float)(sa.frameCount - 1);
    }

    // [speed]_strip[frame]
    int GetSpriteFrameCount(string[] name)
    {
        string attribute = GetSpriteAttribute(name);
        if (attribute == "")
            return 1;

        int frameCountStartIndex = attribute.LastIndexOf("_strip") + "_strip".Length;

        attribute = attribute.Substring(frameCountStartIndex);

        return System.Convert.ToInt32(attribute);
    }

    float GetSpriteSpeed(string[] name, int frameCount)
    {
        string attribute = GetSpriteAttribute(name);
        if (attribute == "")
            return 1f;

        int speedEndIndex = attribute.LastIndexOf("_strip");

        attribute = attribute.Substring(0, speedEndIndex);

        int framePerSec = System.Convert.ToInt32(attribute);

        float deltaTime = 1f / (float)framePerSec;

        float speed = (1f / (float)spriteDefaultFramePerSec) / deltaTime;

        return speed;
    }

    void CutSpriteAttribute(ref string[] name)
    {
        for (int i = name.Length - 1; i >= 0; --i)
        {
            if (name[i].Contains("_strip") == true)
            {
                name[i] = "";
            }
        }
    }

    string GetSpriteAttribute(string[] name)
    {
        for (int i = name.Length - 1; i >= 0; --i)
        {
            if (name[i].Contains("_strip") == true)
            {
                return name[i];
            }
        }

        return "";
    }


    string[] GetSplitName(string name)
    {
        string[] splitName = name.Split('/', '\\');

        string nameWithExtension = splitName[splitName.Length - 1];

        string originName = nameWithExtension.Split('.')[0];

        return originName.Split(' ');
    }

    string GetLoadingName(string name)
    {
        // / or \ 로 split
        string[] splitName = name.Split('/', '\\');

        string loadingName = "";

        // Resouces 하위의 상대경로 + 파일이름( 확장자명 제외 )

        bool b = false;
        for (int i = 0; i < splitName.Length; ++i)
        {
            if (splitName[i] == RESOURCES_PATH)
            {
                b = true;
                continue;
            }
            if (b)
            {
                loadingName += splitName[i] + "/";
            }
        }
        return loadingName.Split('.')[0];
    }

    //
}
