using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    private void Update()
    {
        // ������� �Է��� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �̵� ������ ����
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // ���� ī�޶� �������� ������ �߰��� ��ȯ(Translate ���� �̹���� ����)
        // ������ �ϴ��� �ٶ󺸸� ���� ������
        dir = Camera.main.transform.TransformDirection(dir);

        // �̵� �ӵ��� ���� �̵�
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
