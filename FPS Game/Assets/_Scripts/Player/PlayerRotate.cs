using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    // ȸ�� �ӵ� ����
    public float rotSpeed = 200f;

    // ȸ���� ����
    float mx = 0f;

    private void Update()
    {
        // ���� ���°� '������' ���� �� ���� ������ �� �ְ� ��
        if (GameManager.GM.gState != GameManager.GameState.Run)
            return;

        // ���콺 �¿� �Է��� ����
        float mouse_X = Input.GetAxis("Mouse X");

        // ȸ�� �� ������ ���콺 �Է� ����ŭ �̸� ����
        mx += mouse_X * rotSpeed * Time.deltaTime;

        // ȸ�� �������� ��ü�� ȸ����Ŵ
        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
