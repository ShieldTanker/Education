using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerRotate : NetworkBehaviour
{
    // ȸ�� �ӵ� ����
    public float rotSpeed = 200f;

    // ȸ�� �� ����
    public float mx = 0;


    private void Update()
    {
        // ���� ���°� '���� ��' ������ ���� ������ �� �ְ� ��
        if (GameManager.gm.gStateLocal != GameManager.GameState.Run)
            return;

        // ���콺 �¿� �Է��� ����
        float mouse_X = Input.GetAxis("Mouse X");

        // ȸ�� �� ������ ���콺 �Է� ����ŭ �̸� ������Ŵ
        mx += mouse_X * rotSpeed * Time.deltaTime;
    }

    // �����ϰ� ó���ϴ� �����ӿ� �۵�
    public override void FixedUpdateNetwork()
    {
        // ���� ���°� '���� ��' ������ ���� ������ �� �ְ� ��
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;

        if (GetInput(out NetworkInputData data))
        {
            // ȸ�� �������� ��ü�� ȸ����Ŵ
            transform.eulerAngles = new Vector3(0, data.mx, 0);
        }
    }
}
