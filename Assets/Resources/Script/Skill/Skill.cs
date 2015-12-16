using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

    Condition req;
    Cost cost;
    Effect effect;

	// Use this for initialization
	void Start () {

        ActivateSkill();

    }
	
	// Update is called once per frame
	void Update () {

        ActivateSkill();

    }
    
    public void ActivateSkill()
    {
        if (req.CheckActivatable() == true)
        {

        }
    }
}
