using UnityEngine;
using System.Collections.Generic;

namespace VEPT
{
    public class Controllable : Operable
    {
        public delegate void KeyInputDelegate(KeyManager.EKeyCommand command, KeyManager.EKeyPressType pressType);
        public KeyInputDelegate keyInputDel = new KeyInputDelegate(KeyInputMethod);
        public static void KeyInputMethod(KeyManager.EKeyCommand command, KeyManager.EKeyPressType pressType) { }

        public virtual void KeyInput(KeyManager.EKeyCommand command, KeyManager.EKeyPressType pressType)
        {
            if (state == false) return;

            keyInputDel(command, pressType);
        }
    }
}