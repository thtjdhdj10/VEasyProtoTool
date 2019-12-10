using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Multistat
{
    public bool State
    {
        get
        {
            return state;
        }
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

    public enum StateType
    {
        NONE = 0,
        ACTIVATING_PATTERN,
        STURN,
        KNOCKBACK,
        CLICK,
    }

    private Dictionary<StateType, bool> stateDic = new Dictionary<StateType, bool>();

    public static bool operator ==(Multistat stateA, bool stateB)
    {
        return stateA.state == stateB;
    }

    public static bool operator !=(Multistat stateA, bool stateB)
    {
        return stateA.state != stateB;
    }

    public static bool operator ==(bool stateA, Multistat stateB)
    {
        return stateA == stateB.state;
    }

    public static bool operator !=(bool stateA, Multistat stateB)
    {
        return stateA != stateB.state;
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
                            return;
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
                            return;
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
                            return;
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
                            return;
                        }
                    }

                    state = false;
                }
                break;
        }
    }

    public void SetStateForce(bool _state)
    {
        stateDic.Clear();
        state = _state;
    }

    public void SetState(StateType type, bool _state)
    {
        if (stateDic.ContainsKey(type))
        {
            if(stateDic[type] != _state)
            {
                stateDic[type] = _state;
                UpdateState();
            }
        }
        else
        {
            stateDic.Add(type, _state);
            UpdateState();
        }
    }

    public bool GetState(StateType type, ref bool _state) // return is success
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
               EqualityComparer<Dictionary<StateType, bool>>.Default.Equals(stateDic, @bool.stateDic);
    }

    public override int GetHashCode()
    {
        var hashCode = -325116050;
        hashCode = hashCode * -1521134295 + state.GetHashCode();
        hashCode = hashCode * -1521134295 + conditionForTrue.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<StateType, bool>>.Default.GetHashCode(stateDic);
        return hashCode;
    }
}
