using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    float delta = -0.01f;

    private void Update()
    {
        float newXposition = transform.localPosition.x + delta;
        transform.localPosition = new Vector3(newXposition, transform.localPosition.y, transform.localPosition.z);

        if (transform.localPosition.x < -3.5f)
        {
            delta = 0.01f;
        }
        else if (transform.localPosition.x > 3.5f)
        {
            delta = -0.01f;
        }
    }

    // collison = 충돌한 오브젝트
    private void OnCollisionEnter(Collision collision)
    {   // 예시) 충돌한 오브젝트의 위치(8,5) - 이 스크립트를 쓰는 오브젝트의 위치(0,0) = (8,5) 방향으로 벡터
        Vector3 direction = collision.transform.position - transform.position;
        
        // nomalized = 벡터의 방향은 그대로 이면서 길이는 1로 만들어줌(물체의 크기 상관없이 같은 힘으로 날아감)
        // 이 코드는 길이 상관없이 벡터방향으로 1000 의 길이로 만들어줌
        direction = direction.normalized * 100;
        
        // 충돌한 오브젝트(collision) 의 게임오브젝트 에 Rigidbody를 가져와서 direction 값만큼 힘을 가한다
        // gameObject = 유니티에서 만들어둔 변수
        collision.gameObject.GetComponent<Rigidbody>().AddForce(direction);
    }

    private void Start()
    {
        TestMethod("Ball");
    }

    void TestMethod(string name)
    {
        // Vector3.Distance 는 Vector3.Distance(Vector3 a,Vector3 b) 간 의 거리를 보여줌
        float distance = Vector3.Distance(
            GameObject.Find(name).transform.position,
            transform.position);
        //Debug.Log(name + "까지 거리 : " + distance);
    }
}
