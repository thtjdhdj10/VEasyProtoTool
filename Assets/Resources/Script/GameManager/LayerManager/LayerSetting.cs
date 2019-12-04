using UnityEngine;
using System.Collections.Generic;

using System.Runtime.Serialization;


public class LayerSetting : MonoBehaviour {

    public int layerMask = 0;

//    public bool editRealtime;
    
        // layers 에 접근해서 특정한 레이어를 설정하는 경우
        // 추가해야 되는 컴포넌트가 있음. ex) Targetable

//    private List<bool> layers = new List<bool>();

        // 이걸 어느 시점에 만들어줄지가 문제
    public Dictionary<int, int> indexToLayerNumber = new Dictionary<int, int>();

    public bool[] layers = new bool[LayerManager.MaxLayerCount];

    public KeyValuePair<int, bool>[] layerNumberNState = new KeyValuePair<int, bool>[LayerManager.MaxLayerCount];
    
    void Start()
    {
        UpdateLayerMask();
//        RewriteLayer();
    }

    void Update()
    {
        //if(editRealtime == true)
        //{
        //    RewriteLayer();
        //}
    }

    public void AddLayer(string str)
    {
        int targetLayerNumber = LayerManager.StringToNumber(str);
        
        foreach(var item in indexToLayerNumber)
        {
            if(item.Value == targetLayerNumber)
            {
                layers[item.Key] = true;
                UpdateLayerMask();
                return;
            }
        }
    }

    public void UpdateLayerMask()
    {
        var coll = indexToLayerNumber.Keys;

        foreach(var key in coll)
        {
            int number = indexToLayerNumber[key];

            if(layers[key] == true)
            {
                layerMask = layerMask | LayerManager.NumberToMask(number);
            }
            else
            {
                int mask = -1;
                mask = mask ^ LayerManager.NumberToMask(number);
                layerMask = layerMask & mask;
            }
        }
    }

    // private void UpdateLayerMask(){}

    //private void RewriteLayer()
    //{
    //    int sumMask = 0;

    //    for (int i = 0; i < layers.Count; ++i)
    //    {
    //        if (layers[i] == true)
    //        {
    //            int layerNumber = indexToLayerNumber[i];
    //            sumMask = sumMask | LayerManager.NumberToMask(layerNumber);
    //        }
    //    }

    //    layerMask = sumMask;
    //}

}
