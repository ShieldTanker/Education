using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;


    // �浹�� ó��
    private void OnCollisionEnter(Collision collision)
    {
        // ����Ʈ ������ ����
        GameObject eff = Instantiate(bombEffect);

        // ����Ʈ�� ��ġ�� �ڱ� �ڽŰ� ����
        eff.transform.position = transform.position;

        // �ڱ��ڽ� ����
        Destroy(gameObject);
    }
}
