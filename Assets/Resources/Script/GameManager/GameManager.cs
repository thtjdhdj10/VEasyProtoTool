using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public static bool isQuitting = false;
    private ResourcesManager<GameObject> prefabmanager;
    private ResourcesManager<Sprite> prefabManager;
    private ResourcesManager<RuntimeAnimatorController> controllerManager;

    // 순서를 바꾸지 말 것.
    // index 로 사용할 수 있도록 NONE 을 맨 뒤로 했음
    public enum Direction
    {
        LEFT = 0,
        RIGHT,
        UP,
        DOWN,

        FRONT,
        BACK,

        NONE,
    }

    private void Awake()
    {
        gm = this;

        prefabmanager = new ResourcesManager<GameObject>();
        prefabManager = new ResourcesManager<Sprite>();
        controllerManager = new ResourcesManager<RuntimeAnimatorController>();
    }

    private void FixedUpdate()
    {
        foreach(var o in Operable.GetOperableList<Collidable>())
        {
            Collidable col = o as Collidable;
            if (col != null) col.isCollisionInFrame = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject shock = Instantiate(ResourcesManager<GameObject>.LoadResource(
                ResourcesManager<GameObject>.ResourceName.Effect_Shockwave));
            shock.transform.position = VEasyCalculator.ScreenToWorldPos(Input.mousePosition);
        }
    }

    private void Start()
    {
        //TriggerForUnits tu = gameObject.AddComponent<TriggerForUnits>();
        //tu.type = TriggerForUnits.Type.CREATE_UNIT;
        //tu.target = typeof(Unit);

        //ActionCreateObject action = new ActionCreateObject();

        //tu.actionList.Add()

        //TriggerKeyInput tk = gameObject.AddComponent<TriggerKeyInput>();
        //tk.Init(false, false, true,
        //    this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        //ActionLog al = new ActionLog();
        //al.text = "ATTACK!";
        //tk.actionList.Add(al);

        //if(Controlable.controlableList.Count > 1)
        //{
        //    TriggerKeyInputs tks = gameObject.AddComponent<TriggerKeyInputs>();
        //    tks.Init(false, false, true,
        //        Controlable.controlableList[1].owner);

        //    ActionVectorMoveUnit move = new ActionVectorMoveUnit();
        //    move.speed = 2f;
        //    tks.actionList.Add(move);
        //}

        //for(int i = 0; i < 20; ++i)
        //{
        //    int[] ii = new int[3];
        //    ii[0] = 2;
        //    ii[1] = 10;
        //    ii[2] = 4;
        //    Debug.Log(CustomRandGen.Rand(ii));
        //}


    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

}
