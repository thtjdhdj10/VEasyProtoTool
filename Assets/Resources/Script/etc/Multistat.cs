using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// StateType을 키값으로 하는 복수의 상태값을 관리한다.
// State 는 복수의 상태값과 ConditionForTrue 값에 따라 갱신된다.
// 유닛이 하나 이상의 상태이상을 가지고 있으면 State를 False로 하는 식으로 활용할 수 있다.
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

    public delegate void UpdateDelegate(bool _state);
    public UpdateDelegate updateDelegate = new UpdateDelegate(UpdateStateMethod);
    public static void UpdateStateMethod(bool _state) { }

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
        bool preState = state;
        switch (conditionForTrue)
        {
            case ConditionForTrue.ALL_TRUE:
                {
                    state = stateDic.Values.All(s => s);
                }
                break;
            case ConditionForTrue.ONE_OR_MORE_TRUE:
                {
                    state = stateDic.Values.Any(s => s);
                }
                break;
            case ConditionForTrue.ALL_FALSE:
                {
                    state = stateDic.Values.All(s => !s);
                }
                break;
            case ConditionForTrue.ONE_OR_MORE_FALSE:
                {
                    state = stateDic.Values.Any(s => !s);
                }
                break;
        }

        if(state != preState) updateDelegate(state);
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
        if(stateDic.TryGetValue(type,out bool s))
        {
            _state = s;
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
