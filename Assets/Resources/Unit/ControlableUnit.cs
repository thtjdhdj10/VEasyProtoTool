using UnityEngine;
using System.Collections.Generic;

public class ControlableUnit : MonoBehaviour {
    
    void Awake()
    {
        var layerSetting = gameObject.GetComponent<LayerSetting>();
        if(layerSetting == null)
        {
            layerSetting = gameObject.AddComponent<LayerSetting>();
        }

        layerSetting.layerMask = layerSetting.layerMask | LayerManager.StringToMask("Controlable");
    }

    public void ReceiveCommand(KeyManager.KeyNumber command)
    {
        Debug.Log("dd");
    }



}
