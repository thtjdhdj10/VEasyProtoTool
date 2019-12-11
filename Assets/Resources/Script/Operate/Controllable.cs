using UnityEngine;
using System.Collections.Generic;

public class Controllable : Operable
{
    public delegate void KeyInputDelegate(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType);
    public KeyInputDelegate keyInputDelegate = new KeyInputDelegate(keyInputCallback);
    public static void keyInputCallback(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType) { }

    public virtual void KeyInput(KeyManager.KeyCommand command, KeyManager.KeyPressType pressType)
    {
        if (state == false) return;
        keyInputDelegate(command, pressType);
    }
}
