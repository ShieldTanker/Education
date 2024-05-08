using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform target;
    public float speed;

    private void Update()
    {
        // 위치 기준으로 y축(0,1,0) 으로 왼쪽(-speed)으로 회전
        transform.RotateAround(target.position, Vector3.up, -speed * Time.deltaTime);
    }
}
