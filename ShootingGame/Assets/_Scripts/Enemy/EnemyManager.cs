using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 오브젝트풀 크기
    public int poolSize;

    //에너미 오브젝트풀 리스트
    public List<GameObject>[] enemyPool;


    //적공장(프리팹 에서 불러올 오브젝트)
    public GameObject[] enemyPrefab;



    //최소시간
    public float minTime;

    //최대시간
    public float maxTime;

    //일정 시간
    public float createTime;
    
    // 현재 시간
    float currentTime;

    public float minXpoint;
    public float maxXpoint;

    int enemyCnt;
    //오브젝트풀 리스트 사용
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
        //시간이 흐르다가
        currentTime += Time.deltaTime;
        
        int idx = Random.Range(0, enemyCnt);

        if (enemyPool[idx].Count > 0)
        {

            //만약 현재시간 이 일정시간이 되면
            if (currentTime >= createTime)
            {
                GameObject enemy = enemyPool[idx][0];

                //랜덤 숫자
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
    // 오브젝트풀 크기
    public int poolSize;

    // 오브젝트풀 배열
    GameObject[] enemyObjectPool;

    // SpawnPoint들
    public Transform[] spawnPoints;

    // 적 공장
    public GameObject enemyFactory;

    // 일정시간
    public float createTime;

    // 현재시간
    float currentTime;

    // 최소시간
    float minTime = 1;

    // 최대시간
    float maxTime = 3;


    private void Start()
    {
        // 태어날 때 적 생성시간을 설정하고
        createTime = Random.Range(minTime, maxTime);

        // 오브젝트풀을 에너미들을 담을 수 있는 크기로 만들어 준다.
        enemyObjectPool = new GameObject[poolSize];

        // 오브젝트풀에 넣을 에너미 개수 만큼 반복하여
        for (int i = 0; i < poolSize; i++)
        {
            // 에너미공장에서 에너미를 생성
            GameObject enemy = Instantiate(enemyFactory);

            // 에너미를 오브젝트풀에 넣는다.
            enemyObjectPool[i] = enemy;

            // 비활성화
            enemy.SetActive(false);
        }
    }

    private void Update()
    {
        // 시간이 흐르다가
        currentTime += Time.deltaTime;
        // 만약 현재시간이 일정시간이 되면
        if (currentTime > createTime)
        {
            // 에너미풀 안에 있는 에너미들 중에서
            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = enemyObjectPool[i];

                // 만약 에너미가 비활성화 되었다면
                if (enemy.activeSelf == false)
                {
                    // 랜덤으로 인덱스 선택
                    int index = Random.Range(0, spawnPoints.Length);

                    // 에너미 위치 시키기
                    enemy.transform.position = spawnPoints[index].position;

                    // 에너미를 활성화
                    enemy.SetActive(true);

                    // 에너미 활성화 했긴 때문에 검색 중단
                    break;
                }
            }

            // 현재 시간을 0으로 초기화
            currentTime = 0;
            // 적을 생성한 후 적 생성시간을 다시 설정한다.
            createTime = Random.Range(minTime, maxTime);
        }
    }
}
*/


///////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*

    오브젝트풀 안쓸때

//적공장에서 적을 생성해서
GameObject enemy = Instantiate(enemyFactory);

//내위치에 놓는다
enemy.transform.position = transform.position;

//현재시간을 0으로 초기화
currentTime = 0;

//적을 생성한 후 적 생성시간을 다시 설정한다.
createTime = Random.Range(minTime, maxTime);

 */