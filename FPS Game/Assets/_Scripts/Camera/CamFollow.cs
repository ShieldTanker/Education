using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // 목표가 될 트랜스폼 컴포넌트
    public Transform target;

    private void Update()
    {
        // 카메아의 위치를 목표 트랜스폼의 위치에 일치시킴
        transform.position = target.position;
    }
}
