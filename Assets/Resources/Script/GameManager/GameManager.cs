using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public static bool isQuitting = false;
    private ResourcesManager _resManager;

    private void Awake()
    {
        gm = this;

        _resManager = new ResourcesManager();
    }

    private void FixedUpdate()
    {
        Operable.GetOperableList<Collidable>()?.Select(o => o.isCollisionInFrame = false);
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
