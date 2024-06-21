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

    // �浹���� ���� ó��
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'�� ��� ���� ������Ʈ����
        // Collider ������Ʈ�� �迭�� ����
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Collider[] cols = Physics.OverlapSphere(
            transform.position, explosionRadius, 1 << enemyLayer);
        
        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� ����
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }

        // ����Ʈ �������� ����
        GameObject eff = Instantiate(bombEffect);

        // ����Ʈ �������� ��ġ�� ����ź ������Ʈ �ڽ��� ��ġ�� ����
        eff.transform.position = transform.position;

        // �ڱ� �ڽ��� ����
        Destroy(gameObject);
    }
}
