using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // �̵��ӵ�
    public float bSpeed;

    public GameObject explosionPrefab;

    private void Update()
    {
        // 1. ������ ���Ѵ�.
        Vector3 dir = Vector3.up;

        //2.�̵��ϰ� �ʹ�.
        transform.Translate(dir * bSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //���� ������ ���� ���� ȿ�� ȣ��
            GameObject explosion = Instantiate(explosionPrefab);

            //���� ����Ʈ ��ġ
            explosion.transform.position = transform.position;

            Destroy(collision.gameObject);

            Destroy(gameObject);
        }
    }
}
