using UnityEngine;
using System.Collections;
using System.Linq;

public class KeyboardMove : MonoBehaviour
{
    public float rotateSpeed = 1.0f; // 0 ~ 1

    private string[] _keyStringSet = { "up", "left", "down", "right" };
    private KeyCode[] _keyCodeSet = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    public bool[] currentPressedKey = { false, false, false, false };
    public bool anyKeyPressed = false;

    void Update()
    {
        anyKeyPressed = currentPressedKey.Any(k => k);
    }
}
