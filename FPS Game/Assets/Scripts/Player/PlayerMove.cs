using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 이동 속도 변수
    public float moveSpeed = 7f;

    // 캐릭터 컨트롤러 변수
    CharacterController cc;

    // 중력 변수
    float gravity = -20f;

    // 수직 속도 변수
    [SerializeField]
    float yVelocity = 0f;

    // 점프력 변수
    public float jumpPower = 10f;

    // 점프 상태 변수
    public bool isJumping = false;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

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

        // 점프 후 바닥에 닿은 상태인지 확인
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // 점프 가능하게 변경
            isJumping = false;

            // 캐릭터의 수직속도를 0으로 만듬
            yVelocity = 0f;
        }

        // 만약 Spacebar 키를 입력했으면
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // 캐릭터 수직 속도에 점프력 적용
            yVelocity = jumpPower;
            isJumping = true;
        }

        /* 이동 속도에 맞춰 이동 (캐릭터 컨트롤러로 이동함)
        transform.position += dir * moveSpeed * Time.deltaTime; */

        // 캐릭터 수직 속도에 중력 값을 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 이동속도에 맞춰 이동
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
