using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //플레이어가 이동하는 속력
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

        // 자기 자신을 기준으로 이동
        // 이 스크립트를 사용하는 오브젝트를 중심으로 상대좌표로 이동
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