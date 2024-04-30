using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        target = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // target.foward 카메라가 바라보는 방향 , 세워서 보게 설정
        transform.rotation = Quaternion.LookRotation(target.forward, target.up);
    }
}
