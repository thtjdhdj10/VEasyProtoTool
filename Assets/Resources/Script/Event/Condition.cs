﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Condition
{
    public abstract bool CheckCondition();

    public virtual void Init()
    {

    }

}

public class ConditionImmuneDamage : Condition
{
    public override bool CheckCondition()
    {


        return false;
    }

}