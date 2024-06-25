using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class DestroyEffect : NetworkBehaviour
{
    // 제거될 시간 변수
    public float destroyTime = 1.5f;

    // 경과 시간 측정용 변수
    float currentTime = 0;

    // 수신 하는게 있으면 FixedUpdateNetwork 사용 해야 하지만
    // 수신 안 하고 시간만 재면 안써도 됨
    private void Update()
    {
        // 만일 경과 시간이 제거될 시간을 초과하면 자기 자신을 제거
        if (currentTime > destroyTime)
        {
            // Object : 퓨전에서 처리되고있는 오브젝트 타입
            Runner.Despawn(Object);
        }
        // 경과 시간을 누적
        currentTime += Time.deltaTime;
    }
}
