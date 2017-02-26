using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

    SkillCondition req;
    SkillCost cost;
    SkillEffect effect;

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
