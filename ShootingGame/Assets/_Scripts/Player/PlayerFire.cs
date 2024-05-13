using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //총알 생산할 공장(프리팹)(불릿 참조 연결)
    public GameObject smallBulletFactory;
    public GameObject bigBulletFactory;

    //탄창에 넣을 총알 개수(오브젝트풀 사용)
    public int poolSize;

    //큰총알 오브젝트풀 사이즈
    public int bigBulletPoolSize;

    // 오브젝트풀 리스트
    public List<GameObject> smallBulletObjectPool;
    public List<GameObject> bigBulletObjectPool;


    //총구 (총알 생성 위치)
    public GameObject firePosition;

    
    private void Start()
    {
        //오브젝트풀 리스트
        smallBulletObjectPool = InitBulletObjectPool(smallBulletFactory);
        bigBulletObjectPool = InitBulletObjectPool(bigBulletFactory);
    }


    
    private void Update()
    {
        //유니티데이터와 PC 환경일 때 동작
        //# 으로 된 것들은 전처리기 라고함(전,후)
        //기준은 컴파일(빌드) 하기전 기준
#if UNITY_EDITOR || UNITY_STANDALONE

        //만약 사용자가 발사 버튼을 누르면
        
        if (Input.GetButtonDown("Fire1"))
            Fire(smallBulletObjectPool);

        if (Input.GetKeyDown(KeyCode.Space))
            Fire(bigBulletObjectPool);

#endif
    }

    //Inint 이니셜라이즈 (초기화) 로 많이쓰임
    List<GameObject> InitBulletObjectPool(GameObject bulletFactory)
    {
        List<GameObject> bulletObjectPool = new List<GameObject>();
       
        //탄창(오브젝트풀) 에 넣을 총알 개수 만큼 반복
        for (int i = 0; i < poolSize; i++)
        {
            // 총알공장(프리팹) 에서 총알(오브젝트)을 생성
            GameObject bullet = Instantiate(bulletFactory);

            //오브젝트풀 리스트 에 총알을 넣는다
            bulletObjectPool.Add(bullet);

            //비활성화(화면에 잠깐이라도 나오지는 않음, 비활성화 된 상태로 생성하기)
            bullet.SetActive(false);
        }
        return bulletObjectPool;
    }


    public void Fire(List<GameObject> objectPool)
    {
        if (objectPool.Count > 0)
        {
            GameObject bullet = objectPool[0];

            //총알 위치 시키기
            bullet.transform.position = firePosition.transform.position;

            //총알을 활성화 시킨다
            bullet.SetActive(true);

            //오브젝트풀 에서 총알 제거
            objectPool.Remove(bullet);
        }
    }
    public void SmallFire()
    {
        Fire(smallBulletObjectPool);
    }
}



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
    //오브젝트풀 배열

   public class PlayerFire : MonoBehaviour
{
    // 총알 생산할 공장(프리팹)
    public GameObject bulletFactory;

    //탄창에 넣을 총알 개수(오브젝트풀 사용)
    public int poolSize;

    // 오브젝트풀 배열
    GameObject[] bulletObjectPool;


    //총구 (총알 생성 위치)
    public GameObject firePosition;


    private void Start()
    {
        //탄창(오브젝트풀 배열) 을 총알(오브젝트)을 담을수 있는 크기로 만든다
        bulletObjectPool = new GameObject[poolSize];

        // 탄창에 넣을 총알 개수 만큼 반복
        for (int i = 0; i < poolSize; i++)
        {
            // 총알 공장(프리팹) 에서 총알을 생성
            GameObject bullet = Instantiate(bulletFactory);

            //총알(오브젝트를)을 탄창(오브젝트풀)에 넣는다
            bulletObjectPool[i] = bullet;
            
            //비활성화(화면에 잠깐이라도 나오지는 않음
            //비활성화 된 상태로 생성 시킴
            bullet.SetActive(false);
        }
    }

    private void Update()
    {
        // 만약 사용자가 발사 버튼을 누르면
        if (Input.GetButtonDown("Fire1"))
        {
            // 탄창 안에 있는 총알들 중에서
            for (int i = 0; i < poolSize; i++)
            {

                //오브젝트풀 i번 인덱스에 있는 오브젝트를 GameObject bullet 변수에 저장
                GameObject bullet = bulletObjectPool[i];


                // 만약 총알이 비활성화 되었다면
                if (bullet.activeSelf == false)
                {
                    // 총알을 활성화시킨다.
                    bullet.SetActive(true);

                    // 총알 위치 시키기
                    bullet.transform.position = firePosition.transform.position;

                    // 총알을 발사했기 때문에 비활성화 총알 검색 중단
                    break;
                }
            }
        }
    }
}
*/

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//  오브젝트풀 사용 안할시
            /*//총알공장에서 총알을 만든다
            // 인스턴시에이트에 프리팹 오브젝트를 바로 넣을수 있음
            GameObject bullet = Instantiate(bulletFactory);

            //총알을 총구위치로 옮긴다
            bullet.transform.position = firePosition.transform.position;*/
