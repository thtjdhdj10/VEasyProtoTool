using UnityEngine;
using System.Collections.Generic;

public class ControlableUnit : MonoBehaviour {
    
    void Start()
    {
        var layerSetting = gameObject.GetComponent<LayerSetting>();
        if(layerSetting == null)
        {
            layerSetting = gameObject.AddComponent<LayerSetting>();
        }
        
        layerSetting.AddLayer("Controlable");
    }

    void Update()
    {

    }

    public void ReceiveCommand(KeyManager.KeyNumber command, KeyManager.KeyPressType type)
    {
        if (type == KeyManager.KeyPressType.PRESS)
            return;

        Debug.Log(gameObject.name + "->" + command + " " + type);
    }



}
