﻿using UnityEngine;
using System.Collections.Generic;

/* Unit 구조 설명
 * 
 * Unit 은 GameObject 의 Operable 들을 멤버로 가짐.
 * ( GetComponent 사용을 줄이기 위하여 )
 * 각 Operable 은 static 으로 동종의 Operable List 를 가짐.
 * ( Operable 순회 작업을 쉽게 하기 위하여 )
 */

public class Unit : MyObject
{
    [System.NonSerialized]
    public UnitStatus unitStatus;

    public bool unitActive = true;

    public Dictionary<System.Type, List<Operable>> operableListDic
        = new Dictionary<System.Type, List<Operable>>();

    public List<Trigger> triggerList = new List<Trigger>();

    public static List<Unit> unitList = new List<Unit>();

    public Force force = Force.NONE;

    public enum Force
    {
        NONE = 0,
        PLAYER,
        ENEMY,
    }

    public enum Relation
    {
        NONE = 0,
        ALLY,
        ENEMY,
        ALL,
    }

    public static Relation GetRelation(Force a, Force b)
    {
        if (a == Force.NONE ||
            b == Force.NONE)
            return Relation.NONE;

        if (a == b)
            return Relation.ALLY;
        else return Relation.ENEMY;
    }

    //

    protected virtual void Awake()
    {
        TriggerUnit.UnitEventReceive(this, TriggerUnit.TriggerType.CREATE_UNIT);

        Init();
    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDestroy()
    {
        TriggerUnit.UnitEventReceive(this, TriggerUnit.TriggerType.DESTROY_UNIT);
    }

    protected virtual void OnEnable()
    {
        unitList.Add(this);
    }

    protected virtual void OnDisable()
    {
        unitList.Remove(this);
    }

    protected virtual void FixedUpdate()
    {
        for (int i = 0; i < triggerList.Count; ++i)
        {
            if (triggerList[i] is TriggerFrame)
                (triggerList[i] as TriggerFrame).HandleFixedUpdate();
            if (triggerList[i] is TriggerTimer)
                (triggerList[i] as TriggerTimer).HandleFixedUpdate();
        }
    }

    public virtual void Init()
    {
        TriggerUnit.UnitEventReceive(this, TriggerUnit.TriggerType.INIT_UNIT);
    }

    public virtual void InitSprite()
    {

    }
    
    //

    public Operable GetOperable(System.Type type)
    {
        if (operableListDic[type].Count > 0)
            return operableListDic[type][0];
        return null;
    }
}
