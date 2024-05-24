using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // �̵� �ӵ� ����
    public float moveSpeed = 7f;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    // �߷� ����
    float gravity = -20f;

    // ���� �ӵ� ����
    [SerializeField]
    float yVelocity = 0f;

    // ������ ����
    public float jumpPower = 10f;

    // ���� ���� ����
    public bool isJumping = false;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

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

        // ���� �� �ٴڿ� ���� �������� Ȯ��
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // ���� �����ϰ� ����
            isJumping = false;

            // ĳ������ �����ӵ��� 0���� ����
            yVelocity = 0f;
        }

        // ���� Spacebar Ű�� �Է�������
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // ĳ���� ���� �ӵ��� ������ ����
            yVelocity = jumpPower;
            isJumping = true;
        }

        /* �̵� �ӵ��� ���� �̵� (ĳ���� ��Ʈ�ѷ��� �̵���)
        transform.position += dir * moveSpeed * Time.deltaTime; */

        // ĳ���� ���� �ӵ��� �߷� ���� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // �̵��ӵ��� ���� �̵�
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
