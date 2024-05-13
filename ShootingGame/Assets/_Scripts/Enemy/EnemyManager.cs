using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // ������ƮǮ ũ��
    public int poolSize;

    //���ʹ� ������ƮǮ ����Ʈ
    public List<GameObject>[] enemyPool;


    //������(������ ���� �ҷ��� ������Ʈ)
    public GameObject[] enemyPrefab;



    //�ּҽð�
    public float minTime;

    //�ִ�ð�
    public float maxTime;

    //���� �ð�
    public float createTime;
    
    // ���� �ð�
    float currentTime;

    public float minXpoint;
    public float maxXpoint;

    int enemyCnt;
    //������ƮǮ ����Ʈ ���
    private void Start()
    {
        enemyPool = new List<GameObject>[4];
        enemyCnt = enemyPool.Length;

        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i] = InitEnemyObjPool(enemyPrefab[i]);
        }
    }

    List<GameObject> InitEnemyObjPool(GameObject enemyPrefab)
    {
        List<GameObject> enemyObjectPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);

            enemyObjectPool.Add(enemy);

            enemy.SetActive(false);
        }
        return enemyObjectPool;
    }

    private void Update()
    {
        //�ð��� �帣�ٰ�
        currentTime += Time.deltaTime;
        
        int idx = Random.Range(0, enemyCnt);

        if (enemyPool[idx].Count > 0)
        {

            //���� ����ð� �� �����ð��� �Ǹ�
            if (currentTime >= createTime)
            {
                GameObject enemy = enemyPool[idx][0];

                //���� ����
                float xPoint = Random.Range(minXpoint, maxXpoint);

                enemy.transform.position = new Vector3(
                                                        xPoint,
                                                        transform.localPosition.y,
                                                        transform.localPosition.z);

                enemy.SetActive(true);
                enemyPool[idx].Remove(enemy);


                currentTime = 0;
                createTime = Random.Range(minTime, maxTime);
            }
        }
    }

    
}

    /*
       public void InsertList(List<GameObject> enemyPool, GameObject[] enemyPrefab, int poolSize)
    {

        for (int i = 0; i < poolSize; i++)
        {
            int index = Random.Range(0,enemyPrefab.Length);
            
            GameObject enemy = Instantiate(enemyPrefab[index]);

            enemyPool.Add(enemy);

            enemy.SetActive(false);
        }
    }
    */


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
   public class EnemyManager : MonoBehaviour
{
    // ������ƮǮ ũ��
    public int poolSize;

    // ������ƮǮ �迭
    GameObject[] enemyObjectPool;

    // SpawnPoint��
    public Transform[] spawnPoints;

    // �� ����
    public GameObject enemyFactory;

    // �����ð�
    public float createTime;

    // ����ð�
    float currentTime;

    // �ּҽð�
    float minTime = 1;

    // �ִ�ð�
    float maxTime = 3;


    private void Start()
    {
        // �¾ �� �� �����ð��� �����ϰ�
        createTime = Random.Range(minTime, maxTime);

        // ������ƮǮ�� ���ʹ̵��� ���� �� �ִ� ũ��� ����� �ش�.
        enemyObjectPool = new GameObject[poolSize];

        // ������ƮǮ�� ���� ���ʹ� ���� ��ŭ �ݺ��Ͽ�
        for (int i = 0; i < poolSize; i++)
        {
            // ���ʹ̰��忡�� ���ʹ̸� ����
            GameObject enemy = Instantiate(enemyFactory);

            // ���ʹ̸� ������ƮǮ�� �ִ´�.
            enemyObjectPool[i] = enemy;

            // ��Ȱ��ȭ
            enemy.SetActive(false);
        }
    }

    private void Update()
    {
        // �ð��� �帣�ٰ�
        currentTime += Time.deltaTime;
        // ���� ����ð��� �����ð��� �Ǹ�
        if (currentTime > createTime)
        {
            // ���ʹ�Ǯ �ȿ� �ִ� ���ʹ̵� �߿���
            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = enemyObjectPool[i];

                // ���� ���ʹ̰� ��Ȱ��ȭ �Ǿ��ٸ�
                if (enemy.activeSelf == false)
                {
                    // �������� �ε��� ����
                    int index = Random.Range(0, spawnPoints.Length);

                    // ���ʹ� ��ġ ��Ű��
                    enemy.transform.position = spawnPoints[index].position;

                    // ���ʹ̸� Ȱ��ȭ
                    enemy.SetActive(true);

                    // ���ʹ� Ȱ��ȭ �߱� ������ �˻� �ߴ�
                    break;
                }
            }

            // ���� �ð��� 0���� �ʱ�ȭ
            currentTime = 0;
            // ���� ������ �� �� �����ð��� �ٽ� �����Ѵ�.
            createTime = Random.Range(minTime, maxTime);
        }
    }
}
*/


///////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*

    ������ƮǮ �Ⱦ���

//�����忡�� ���� �����ؼ�
GameObject enemy = Instantiate(enemyFactory);

//����ġ�� ���´�
enemy.transform.position = transform.position;

//����ð��� 0���� �ʱ�ȭ
currentTime = 0;

//���� ������ �� �� �����ð��� �ٽ� �����Ѵ�.
createTime = Random.Range(minTime, maxTime);

 */