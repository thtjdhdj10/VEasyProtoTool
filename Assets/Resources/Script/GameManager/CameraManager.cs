using UnityEngine;
using System.Collections;

// TODO 카메라 쉐이크 기능 추가 (진폭, 감쇄, 지속시간)
public class CameraManager : MonoBehaviour
{
    public static CameraManager manager;

    void Awake()
    {
        manager = this;
    }
}
