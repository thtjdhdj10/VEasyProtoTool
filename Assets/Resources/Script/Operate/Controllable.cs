using UnityEngine;
using System.Collections.Generic;

public class Controllable : Operable
{
    public delegate void KeyInputDelegate(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType);
    public KeyInputDelegate keyInputDelegate = new KeyInputDelegate(KeyInputCallback);
    public static void KeyInputCallback(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType) { }

    public virtual void KeyInput(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType)
    {
        if (_state == false) return;
        keyInputDelegate(command, pressType);
    }
}
