using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyIdx;

    public float enemySpeed;

    //��������� ���� Start �� Update ���� ���
    Vector3 dir;

    //���� ����Ʈ ������
    public GameObject explosionFactory;

    public int enemyScore;


    //������Ʈ�� Ȱ��ȭ �Ǿ�����
    private void OnEnable()
    {
        //0���� 9���� ���߿� �ϳ��� �������� �����ͼ�
        int randValue = Random.Range(0, 10);

        //���� 3���� ������ �÷��̾� ����
        if (randValue < 3)
        {
            //�÷��̾ ã�Ƽ� target ���� ���Ѵ�
            GameObject target = GameObject.Find("Player");

            //���ⱸ�ϱ�
            dir = target.transform.position - transform.position;

            //������ ũ�⸦ 1�� ���Ѵ�
            dir.Normalize();
        }
        //�׷��� ������ �Ʒ��������� ���Ѵ�
        else
            dir = Vector3.down;
    }


    private void Update()
    {
        //�̵�
        transform.Translate(dir * enemySpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ScoreManager �� Score �Ӽ�(������Ƽ)���� ���� 1����
        ScoreManager.Instance.Score += enemyScore;

        //���� ������ ���� ���� ȿ�� ȣ��
        GameObject explosion = Instantiate(explosionFactory);

        //���� ����Ʈ ��ġ
        explosion.transform.position = transform.position;
        
        //�΋H�� ��ü ��Ȱ��ȭ
        collision.gameObject.SetActive(false);

        //���� �΋H�� ��ü �� �̸��� Bullet �� ���
        if (collision.gameObject.tag == "Bullet")
        {

            //PlayerFire Ŭ���� �ҷ�����(PlayerFire �� ������ƮǮ�� ��ȯ ����� �ϱ⶧��)
            PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();

            //����Ʈ�� �Ѿ� ����
            playerFire.smallBulletObjectPool.Add(collision.gameObject);
        }

        else if (collision.gameObject.tag == "BigBullet")
        {

            //PlayerFire Ŭ���� �ҷ�����(PlayerFire �� ������ƮǮ�� ��ȯ ����� �ϱ⶧��)
            PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();

            //����Ʈ�� �Ѿ� ����
            playerFire.bigBulletObjectPool.Add(collision.gameObject);
        }

        else
            //�΋H�� ��ü ����
            Destroy(collision.gameObject);

        gameObject.SetActive(false);

        //���ʹ� �Ŵ��� Ŭ���� ȣ��
        EnemyManager enManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        //�ֳʹ� �Ŵ��� �� ���ʹ� Ǯ�� ���� ������Ʈ(enemy)�߰�
        enManager.enemyPool[enemyIdx].Add(gameObject);


    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
    ������ƮǮ �迭���

   public class Enemy : MonoBehaviour
{
    // enemy �̵� �ӵ�
    public float speed;

    // ������ ��������� ���� Start�� Update���� ���
    Vector3 dir;

    // ���� ����Ʈ ������
    public GameObject explosionFactory;

    private void OnEnable()
    {

        // 0���� 9���� ���߿� �ϳ��� �������� �����ͼ�
        int randValue = Random.Range(0, 10);

        // ���� 3���� ������ �÷��̾� ���� ����
        if (randValue < 3)
        {
            // �÷��̾ ã�Ƽ� target���� ���Ѵ�.
            GameObject target = GameObject.Find("Player");

            // ������ ���ϱ�.
            dir = target.transform.position - transform.position;

            // ������ ũ�⸦ 1�� �Ѵ�.
            dir.Normalize();
        }
        else
        {
            // �׷��� ������ �Ʒ��������� ���Ѵ�.
            dir = Vector3.down;
        }
    }

    private void Update()
    {
        // �̵�
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ScoreManager�� Score �Ӽ�(������Ƽ)���� ���� 1����
        ScoreManager.Instance.Score++;

        // ���� ȿ�� ���忡�� ���� ȿ���� �ϳ� �����.
        GameObject explosion = Instantiate(explosionFactory);

        // ���� ȿ�� ������Ʈ�� ��ġ ����
        explosion.transform.position = transform.position;



        //���� �΋H�� ��ü�� Bullet �̶��, Contain �� ���ڿ��� Bullet �̶�� �̸��� �߰��� ������
        if (collision.gameObject.name.Contains("Bullet"))
        {
            // �ε��� ��ü�� ��Ȱ��ȭ
            collision.gameObject.SetActive(false);
        }
        else
        {
            // �׷��� ������ ����
            Destroy(collision.gameObject);
        }

        // Destroy�� ���ִ� ��� ��Ȱ��ȭ�ؿ� Ǯ�� �ڿ��� �ݳ�
        gameObject.SetActive(false);
    }
}
*/

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


/*
�Ӽ� �ϱ��� �ڵ�

ScoreManager �� Get/Set �Լ��� ���� 1 ����
int currentScore = ScoreManager.Instance.GetScore();
ScoreManager.Instance.SetScore(currentScore + 1);
*/

/*

 //������ƮǮ ��� ���ҽ�
 //���װ�
 Destroy(collision.gameObject);
 //������
 Destroy(gameObject);

*/