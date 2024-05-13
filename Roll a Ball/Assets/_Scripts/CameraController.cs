using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }


    // LateUpdate 다른 오브젝트(모든 오브젝트,스크립트)
    // 업데이트 들이 다 처리되고 실행
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }

}
