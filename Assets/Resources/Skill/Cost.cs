using UnityEngine;
using System.Collections.Generic;

public class Cost : MonoBehaviour {

    public Player.Relatations costConsumer = Player.Relatations.ME;

    struct TargetCostValue
    {
        public object target;
        public float cost;
    }

    struct TargetCostStack
    {
        public object target;
        public int cost;
    }

    List<TargetCostValue> costValue = new List<TargetCostValue>();
    List<TargetCostStack> costStack = new List<TargetCostStack>();

    // 아이템 소모 추가

    public void CostConsume()
    {

    }

    public void AddConsumptionValue(ref float value, float cost)
    {
        TargetCostValue tc = new TargetCostValue();
        tc.target = value;


    }
}
