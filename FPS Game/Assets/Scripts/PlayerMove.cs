using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    private void Update()
    {
        // 사용자의 입력을 받음
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 이동 방향을 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 메인 카메라를 기준으로 방향을 추가로 변환(Translate 말고 이방법도 있음)
        // 문제는 하늘을 바라보면 위로 떠버림
        dir = Camera.main.transform.TransformDirection(dir);

        // 이동 속도에 맞춰 이동
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
