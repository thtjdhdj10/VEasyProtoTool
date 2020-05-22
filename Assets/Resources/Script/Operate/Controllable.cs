using UnityEngine;
using System.Collections.Generic;

public class Controllable : Operable
{
    public delegate void KeyInputDel(KeyManager.EKeyCommand command, KeyManager.EKeyPressType pressType);
    public KeyInputDel keyInputDel = new KeyInputDel(EmptyMethod);
    public static void EmptyMethod(KeyManager.EKeyCommand command, KeyManager.EKeyPressType pressType) { }

    public virtual void KeyInput(KeyManager.EKeyCommand command, KeyManager.EKeyPressType pressType)
    {
        if (state == false) return;

        keyInputDel(command, pressType);
    }
}
