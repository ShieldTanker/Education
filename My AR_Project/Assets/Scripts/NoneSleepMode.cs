using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneSleepMode : MonoBehaviour
{
    private void Start()
    {
        // �� ���� �߿��� ���� ���� ��ȯ���� �ʵ��� ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
