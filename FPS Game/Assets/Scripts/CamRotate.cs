using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // ȸ�� �ӵ� ����
    public float rotSpeed = 200f;

    float mX = 0f;
    float mY = 0f;

    private void Update()
    {
        // ���콺 �Է��� ����
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        // ȸ���� ������ ���콺 �Է°���ŭ �̸� ����
        mX += mouse_X * rotSpeed * Time.deltaTime;
        mY += mouse_Y * rotSpeed * Time.deltaTime;

        // ���콺 ���� �̵� ȸ�� ����(mY) �� ���� -90 ~ 90 �� ���̷� ����
        mY = Mathf.Clamp(mY, -90, 90);

        // ���콺 �Է� ���� �̿��� ���� ������ ����(X����� ȸ�� : ����, Y�� ���� ȸ��: �¿�)
        // ȸ�� �������� ������Ʈ ȸ��
        transform.eulerAngles = new Vector3(-mY, mX, 0);
    }
}
