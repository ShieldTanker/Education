using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    public int breakSpeed;
    public bool finish;

    private Rigidbody rb;
    private bool breakKeydown;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate�� �������� �Ҷ� ���
    // FixedUpdate�� ������Ʈ ���̰� ������ ���Ǳ���
    // FixedUpdate ���� Time.fixedDeltaTime �� ���
    // �Ʒ� AddForce �� FixedUpdate �� �������� ���� Time.fixedDeltaTime �� ���� ����
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        if (!finish)
        {
            if (breakKeydown)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                breakKeydown = false;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                rb.velocity *= 0.9f;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(movement * runSpeed);
            }
            rb.AddForce(movement * speed);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            breakKeydown = true;
        }
    }
}
