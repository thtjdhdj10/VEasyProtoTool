using UnityEngine;
using System.Collections;

public class GameManager : MyObject
{
    public enum Direction
    {
        NONE = 0,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        FRONT,
        BACK,
    }


    public void Awake()
    {
        //TriggerForUnits tu = gameObject.AddComponent<TriggerForUnits>();
        //tu.type = TriggerForUnits.Type.CREATE_UNIT;
        //tu.target = typeof(Unit);

        //ActionCreateObject action = new ActionCreateObject();
        
        //tu.actionList.Add()

        TriggerForKeyInput tk = gameObject.AddComponent<TriggerForKeyInput>();
        tk.Init(false, false, true,
            this, KeyManager.KeyCommand.COMMAND_ATTACK, KeyManager.KeyPressType.DOWN);
        ActionLog al = new ActionLog();
        al.text = "ATTACK!";
        tk.actionList.Add(al);

        for(int i = 0; i < 20; ++i)
        {
            int[] ii = new int[3];
            ii[0] = 2;
            ii[1] = 10;
            ii[2] = 4;
            Debug.Log(CustomRandGen.Rand(ii));
        }


    }

}
