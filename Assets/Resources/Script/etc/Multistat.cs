using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Multistat
{
    public bool State
    {
        get;
    }
    [SerializeField]
    private bool state = true;

    public ConditionForTrue conditionForTrue = ConditionForTrue.ALL_FALSE;

    public enum ConditionForTrue
    {
        ALL_TRUE,
        ALL_FALSE,
        ONE_OR_MORE_TRUE,
        ONE_OR_MORE_FALSE,
    }

    public enum type
    {
        NONE = 0,
        ACTIVATING_PATTERN,
        STURN,
        KNOCKBACK,
        NON_CLICK,
    }

    private Dictionary<type, bool> stateDic = new Dictionary<type, bool>();

    public static bool operator ==(Multistat stateA, bool stateB)
    {
        return stateA.state == stateB;
    }

    public static bool operator !=(Multistat stateA, bool stateB)
    {
        return stateA.state == stateB;
    }

    public static bool operator ==(bool stateA, Multistat stateB)
    {
        return stateA == stateB.state;
    }

    public static bool operator !=(bool stateA, Multistat stateB)
    {
        return stateA == stateB.state;
    }

    public void UpdateState()
    {
        switch (conditionForTrue)
        {
            case ConditionForTrue.ALL_TRUE:
                {
                    foreach(var v in stateDic.Values)
                    {
                        if (v == false)
                        {
                            state = false;
                            break;
                        }
                    }

                    state = true;
                }
                break;
            case ConditionForTrue.ONE_OR_MORE_TRUE:
                {
                    foreach(var v in stateDic.Values)
                    {
                        if(v == true)
                        {
                            state = true;
                            break;
                        }
                    }

                    state = false;
                }
                break;
            case ConditionForTrue.ALL_FALSE:
                {
                    foreach(var v in stateDic.Values)
                    {
                        if(v == true)
                        {
                            state = false;
                            break;
                        }
                    }

                    state = true;
                }
                break;
            case ConditionForTrue.ONE_OR_MORE_FALSE:
                {
                    foreach(var v in stateDic.Values)
                    {
                        if(v == false)
                        {
                            state = true;
                            break;
                        }
                    }

                    state = false;
                }
                break;
        }
    }

    public void SetStateForce(bool _state)
    {
        foreach(var k in stateDic.Keys)
        {
            stateDic[k] = _state;
        }
    }

    public void SetState(type type, bool _state)
    {
        if (stateDic.ContainsKey(type))
        {
            if(stateDic[type] != _state)
            {
                UpdateState();
                stateDic[type] = _state;
            }
        }
        else
        {
            stateDic.Add(type, _state);
            UpdateState();
        }
    }

    public bool GetState(type type, ref bool _state) // return is success
    {
        if (stateDic.ContainsKey(type))
        {
            _state = stateDic[type];
            return true;
        }

        return false;
    }

    //

    public override bool Equals(object obj)
    {
        var @bool = obj as Multistat;
        return @bool != null &&
               state == @bool.state &&
               conditionForTrue == @bool.conditionForTrue &&
               EqualityComparer<Dictionary<type, bool>>.Default.Equals(stateDic, @bool.stateDic);
    }

    public override int GetHashCode()
    {
        var hashCode = -325116050;
        hashCode = hashCode * -1521134295 + state.GetHashCode();
        hashCode = hashCode * -1521134295 + conditionForTrue.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<type, bool>>.Default.GetHashCode(stateDic);
        return hashCode;
    }
}
