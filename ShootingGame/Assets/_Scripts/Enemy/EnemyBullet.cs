using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // 이동속도
    public float bSpeed;

    public GameObject explosionPrefab;

    private void Update()
    {
        // 1. 방향을 구한다.
        Vector3 dir = Vector3.up;

        //2.이동하고 싶다.
        transform.Translate(dir * bSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //폭발 프리팹 에서 폭발 효과 호출
            GameObject explosion = Instantiate(explosionPrefab);

            //폭발 이펙트 위치
            explosion.transform.position = transform.position;

            Destroy(collision.gameObject);

            Destroy(gameObject);
        }
    }
}
