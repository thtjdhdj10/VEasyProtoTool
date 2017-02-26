using UnityEngine;
using System.Collections.Generic;

public class Controlable : Operable
{
    public MyObject owner;

    public static List<Controlable> controlableList = new List<Controlable>();

    protected virtual void Awake()
    {
        owner = GetComponent<MyObject>();

        controlableList.Add(this);
    }

    protected virtual void OnDestroy()
    {
        controlableList.Remove(this);
    }

    public virtual void ReceiveCommand(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType)
    {
        TriggerForKeyInput.UnitEventReceive(owner, command, pressType);
    }

    //
    
    //void Start()
    //{
    //    var layerSetting = gameObject.GetComponent<LayerSetting>();
    //    if(layerSetting == null)
    //    {
    //        layerSetting = gameObject.AddComponent<LayerSetting>();
    //    }
        
    //    layerSetting.AddLayer("Controlable");
    //}
}
