using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public static bool isQuitting = false;
    private ResourcesManager _resManager;

    private void Awake()
    {
        gm = this;

        _resManager = new ResourcesManager();

        Init();
    }

    public void Init()
    {
        float widthRatio = (float)Screen.width / Screen.height;

        Const.worldHeightHalf = Camera.main.orthographicSize;
        Const.worldWidthHalf = Const.worldHeightHalf * widthRatio;
    }

    private void FixedUpdate()
    {
        Operable.GetOperableList<Collidable>()?.ForEach(o => o.isCollisionInFrame = false);
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        GameObject shock = Instantiate(ResourcesManager<GameObject>.LoadResource(
    //            ResourcesManager<GameObject>.ResourceName.Effect_Shockwave));
    //        shock.transform.position = VEasyCalculator.ScreenToWorldPos(Input.mousePosition);
    //    }
    //}
}
