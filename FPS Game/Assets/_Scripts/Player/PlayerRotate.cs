using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    // 회전 속도 변수
    public float rotSpeed = 200f;

    // 회전값 변수
    float mx = 0f;

    private void Update()
    {
        // 게임 상태가 '게임중' 상태 일 때만 조작할 수 있게 함
        if (GameManager.GM.gState != GameManager.GameState.Run)
            return;

        // 마우스 좌우 입력을 받음
        float mouse_X = Input.GetAxis("Mouse X");

        // 회전 값 변수에 마우스 입력 값만큼 미리 누적
        mx += mouse_X * rotSpeed * Time.deltaTime;

        // 회전 방향으로 물체를 회전기킴
        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
