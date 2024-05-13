using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�÷��̾ �̵��ϴ� �ӷ�
    public float pSpeed;

    public float ghostTime;

    public BoxCollider playerCol;

    void Update()
    {

        if (ghostTime > 0)
        {
            if (playerCol.enabled)
                playerCol.enabled = false;
            
            ghostTime -= Time.deltaTime;

            if (ghostTime <= 0)
                playerCol.enabled = true;
        }


        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Vector3 dir = new Vector3(h, v, 0);

        // �ڱ� �ڽ��� �������� �̵�
        // �� ��ũ��Ʈ�� ����ϴ� ������Ʈ�� �߽����� �����ǥ�� �̵�
        transform.Translate(dir * pSpeed * Time.deltaTime);

        if (transform.position.x >= 3.5f)
        {
            transform.position = new Vector3(3.4f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -3.5f)
        {
            transform.position = new Vector3(-3.4f, transform.position.y, transform.position.z);
        }

        if (transform.position.y >= 5)
        {
            transform.position = new Vector3(transform.position.x, 4.9f, transform.position.z);
        }
        else if(transform.position.y <= -5)
        {
            transform.position = new Vector3(transform.position.x, -4.9f, transform.position.z);
        }
    }
}