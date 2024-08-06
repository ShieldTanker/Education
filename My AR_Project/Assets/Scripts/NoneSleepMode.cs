using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneSleepMode : MonoBehaviour
{
    private void Start()
    {
        // 앱 실행 중에는 절전 모드로 전환되지 않도록 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
