using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 회전 속도 변수
    public float rotSpeed = 200f;

    // 회전 값 변수
    float mx = 0;
    float my = 0;

    private void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 함
        if (GameManager.gm.gStateLocal != GameManager.GameState.Run)
            return;

        // 마우스 입력을 받음
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        // 회전 값 변수에 마우스 입력 값만큼 미리 누적
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        // 마우스 상하 이동 회전 변수(my)의 값을 -90 ~ 90도 사이로 제한
        my = Mathf.Clamp(my, -90f, 90f);

        // 물체를 회전 방향으로 회전
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
