using UnityEngine;
using System.Collections;

public class Condition : MonoBehaviour {

    enum Command
    {
        NONE = 0,
        ACTIVATE = 1,

    }

    Command command;

    float coolDownTime;
    float currentCoolDown;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    public bool CheckActivatable()
    {
        if (command != Command.ACTIVATE)
            return false;

        if (currentCoolDown > 0.0f)
            return false;

        //

        return true;
    }
}
