using UnityEngine;
using System.Collections.Generic;

public class LayerSetting : MonoBehaviour {

    public int layerMask = 0;

    const int MaxLayerCount = 32;

    public bool editRealtime = false;

    public bool[] layers = new bool[MaxLayerCount];

    void Awake()
    {

    }

    void Start()
    {
        RewriteLayer();
    }

    void Update()
    {
        if(editRealtime == true)
        {
            RewriteLayer();
        }
    }

    private void RewriteLayer()
    {
        int sumMask = 0;

        for (int i = 0; i < MaxLayerCount; ++i)
        {
            if (layers[i] == true)
            {
                sumMask = sumMask | LayerManager.NumberToMask(i);
            }
        }

        layerMask = sumMask;
    }

}
