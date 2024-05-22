using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 회전 속도 변수
    public float rotSpeed = 200f;

    float mX = 0f;
    float mY = 0f;

    private void Update()
    {
        // 마우스 입력을 받음
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        // 회전값 변수에 마우스 입력값만큼 미리 누적
        mX += mouse_X * rotSpeed * Time.deltaTime;
        mY += mouse_Y * rotSpeed * Time.deltaTime;

        // 마우스 상하 이동 회전 변수(mY) 의 값을 -90 ~ 90 도 사이로 제한
        mY = Mathf.Clamp(mY, -90, 90);

        // 마우스 입력 값을 이용해 외전 방향을 결정(X축기준 회전 : 상하, Y축 기준 회전: 좌우)
        // 회전 방향으로 오브젝트 회전
        transform.eulerAngles = new Vector3(-mY, mX, 0);
    }
}
