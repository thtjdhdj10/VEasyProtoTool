using UnityEngine;
using System.Collections;

public class KeyboardMove : MonoBehaviour {


    
    public float rotateSpeed = 1.0f; // 0 ~ 1

    // starcraft 1 의 유닛 브루드링 같은 이동을 구현하기 위한 값
    public bool useWaveringStep = false;
    public float waveringDelay = 0.0f;
    public float SpeedDecreasingTime = 0.0f;




    string[] keyStringSet = { "up", "left", "down", "right" };
    KeyCode[] keyCodeSet = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    public bool[] currentPressedKey = { false, false, false, false };
    public bool anyKeyPressed = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        anyKeyPressed = AnyKeyPressedCheck();



    }

    bool AnyKeyPressedCheck()
    {
        for(int i = 0; i < 4; ++i)
        {
            if (currentPressedKey[i] == true)
                return true;
        }

        return false;
    }
}
