using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyIdx;

    public float enemySpeed;

    //멤버변수로 만들어서 Start 와 Update 에서 사용
    Vector3 dir;

    //폭발 이펙트 프리팹
    public GameObject explosionFactory;

    public int enemyScore;


    //오브젝트가 활성화 되었을때
    private void OnEnable()
    {
        //0부터 9까지 값중에 하나를 랜덤으로 가져와서
        int randValue = Random.Range(0, 10);

        //만약 3보다 작으면 플레이어 방향
        if (randValue < 3)
        {
            //플레이어를 찾아서 target 으로 정한다
            GameObject target = GameObject.Find("Player");

            //방향구하기
            dir = target.transform.position - transform.position;

            //방향의 크기를 1로 정한다
            dir.Normalize();
        }
        //그렇지 않으면 아래방향으로 정한다
        else
            dir = Vector3.down;
    }


    private void Update()
    {
        //이동
        transform.Translate(dir * enemySpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ScoreManager 의 Score 속성(프로퍼티)으로 점수 1증가
        ScoreManager.Instance.Score += enemyScore;

        //폭발 프리팹 에서 폭발 효과 호출
        GameObject explosion = Instantiate(explosionFactory);

        //폭발 이펙트 위치
        explosion.transform.position = transform.position;
        
        //부딫힌 물체 비활성화
        collision.gameObject.SetActive(false);

        //만일 부딫힌 물체 의 이름이 Bullet 인 경우
        if (collision.gameObject.tag == "Bullet")
        {

            //PlayerFire 클래스 불러오기(PlayerFire 의 오브젝트풀에 반환 해줘야 하기때문)
            PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();

            //리스트에 총알 삽입
            playerFire.smallBulletObjectPool.Add(collision.gameObject);
        }

        else if (collision.gameObject.tag == "BigBullet")
        {

            //PlayerFire 클래스 불러오기(PlayerFire 의 오브젝트풀에 반환 해줘야 하기때문)
            PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();

            //리스트에 총알 삽입
            playerFire.bigBulletObjectPool.Add(collision.gameObject);
        }

        else
            //부딫힌 물체 제거
            Destroy(collision.gameObject);

        gameObject.SetActive(false);

        //에너미 매니저 클래스 호출
        EnemyManager enManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        //애너미 매니저 의 에너미 풀에 현재 오브젝트(enemy)추가
        enManager.enemyPool[enemyIdx].Add(gameObject);


    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
    오브젝트풀 배열사용

   public class Enemy : MonoBehaviour
{
    // enemy 이동 속도
    public float speed;

    // 방향을 멤버변수로 만들어서 Start와 Update에서 사용
    Vector3 dir;

    // 폭발 이펙트 프리팹
    public GameObject explosionFactory;

    private void OnEnable()
    {

        // 0부터 9까지 값중에 하나를 랜덤으로 가져와서
        int randValue = Random.Range(0, 10);

        // 만약 3보다 작으면 플레이어 방향 으로
        if (randValue < 3)
        {
            // 플레이어를 찾아서 target으로 정한다.
            GameObject target = GameObject.Find("Player");

            // 방향을 구하기.
            dir = target.transform.position - transform.position;

            // 방향의 크기를 1로 한다.
            dir.Normalize();
        }
        else
        {
            // 그렇지 않으면 아래방향으로 정한다.
            dir = Vector3.down;
        }
    }

    private void Update()
    {
        // 이동
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ScoreManager의 Score 속성(프로퍼티)으로 점수 1증가
        ScoreManager.Instance.Score++;

        // 폭발 효과 공장에서 폭발 효과를 하나 만든다.
        GameObject explosion = Instantiate(explosionFactory);

        // 폭발 효과 오브젝트의 위치 지정
        explosion.transform.position = transform.position;



        //만약 부딫힌 물체가 Bullet 이라면, Contain 은 문자열에 Bullet 이라는 이름이 중간에 들어갔을시
        if (collision.gameObject.name.Contains("Bullet"))
        {
            // 부딪힌 물체를 비활성화
            collision.gameObject.SetActive(false);
        }
        else
        {
            // 그렇지 않으면 제거
            Destroy(collision.gameObject);
        }

        // Destroy로 없애는 대신 비활성화해여 풀에 자원을 반납
        gameObject.SetActive(false);
    }
}
*/

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


/*
속성 하기전 코드

ScoreManager 의 Get/Set 함수로 점수 1 증가
int currentScore = ScoreManager.Instance.GetScore();
ScoreManager.Instance.SetScore(currentScore + 1);
*/

/*

 //오브젝트풀 사용 안할시
 //너죽고
 Destroy(collision.gameObject);
 //나죽자
 Destroy(gameObject);

*/