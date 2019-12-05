using UnityEngine;
using System.Collections.Generic;

public class Controlable : Operable
{
    public virtual void ReceiveCommand(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType)
    {
        if (active == false) return;
        TriggerKeyInput.UnitEventReceive(owner, command, pressType);
        TriggerKeyInputs.UnitEventReceive(owner, command, pressType);
    }
}
