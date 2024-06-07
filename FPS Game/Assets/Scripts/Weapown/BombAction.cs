using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // ���� ����Ʈ ������ ����
    public GameObject bombEffect;

    // ����ź ������
    public int attackPower = 10;

    // ���� ȿ�� �ݰ�
    public float explosionRadius = 5f;

    // �浹�� ó��
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ȿ�� �ݰ� ������ ���̾ "Enemy" �� ��� ���� ������Ʈ ����
        // Collider ������Ʈ�� �迭�� ����
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        /* 8�� ���̾� �ϰ�� 2^8 �̱⿡ ����Ʈ �����ڷ� 8�ڸ��� �ڸ��ű�(��Ʈ�� ó��)
           00100010000 �̸� 8�� ���̾�� 4�� ���̾� �Ѵ� Ȯ�� ����

           �ΰ� �̻� ���̾� �ϰ� ������( 1 << enemyLayer | �ٸ� ���̾� )
           �̷������� �ϸ� �� */
        Collider[] cols = Physics.OverlapSphere(
            transform.position, explosionRadius, 1 << enemyLayer);

        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� ����
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }

        // ����Ʈ ������ ����
        GameObject eff = Instantiate(bombEffect);

        // ����Ʈ�� ��ġ�� �ڱ� �ڽŰ� ����
        eff.transform.position = transform.position;

        // �ڱ��ڽ� ����
        Destroy(gameObject);
    }
}
