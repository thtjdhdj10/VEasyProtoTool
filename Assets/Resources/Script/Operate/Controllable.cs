using UnityEngine;
using System.Collections.Generic;

public class Controllable : Operable
{
    public delegate void KeyInputDel(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType);
    public KeyInputDel keyInputDel = new KeyInputDel(EmptyMethod);
    public static void EmptyMethod(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType) { }

    public virtual void KeyInput(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType)
    {
        if (state == false) return;
        keyInputDel(command, pressType);
    }
}
