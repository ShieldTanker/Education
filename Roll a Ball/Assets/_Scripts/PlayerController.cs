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

    // FixedUpdate는 물리연산 할때 사용
    // FixedUpdate는 업데이트 사이간 간격이 거의균일
    // FixedUpdate 에는 Time.fixedDeltaTime 을 사용
    // 아래 AddForce 는 FixedUpdate 에 있음으로 굳이 Time.fixedDeltaTime 을 넣지 않음
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
