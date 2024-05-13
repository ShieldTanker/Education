using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    //영역 안에 다른 물체가 감지 될 경우
    private void OnTriggerEnter(Collider other)
    {   
        // 부딫힌 물체 비활성화
        other.gameObject.SetActive(false);


        //만약 부딪힌 물체가 Bullet 혹은 Enemy 라면
        if (other.gameObject.tag == "Bullet")
        {
            //playerFire클래스 불러오기(PlayerFire 의 오브젝트풀에 반환 해줘야 하기때문)
            PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();

            //리스트에 총알 삽입
            playerFire.smallBulletObjectPool.Add(other.gameObject);
        
        }
        else if (other.gameObject.tag == "BigBullet")
        {
            PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();
            //리스트에 총알 삽입
            playerFire.bigBulletObjectPool.Add(other.gameObject);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyManager enManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();


            Enemy enemy = other.GetComponent<Enemy>();
            //리스트에 에너미 삽입
            enManager.enemyPool[enemy.enemyIdx].Add(other.gameObject);
        }

        else if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

            other.gameObject.SetActive(false);
            em.enemyPool[3].Add(gameObject);
        }
        else
        {
            Debug.Log(other);
            Destroy(other.gameObject);
        }
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*

     public class DestroyZone : MonoBehaviour
{
    // 영역 안에 다른 물체가 감지될 경우
    private void OnTriggerEnter(Collider other)
    {
        // 만약 부딪힌 물체가 Bullet 이라면
        if (other.gameObject.name.Contains("Bullet") || other.gameObject.name.Contains("Enemy"))
        {
            // 부딪힌 물체를 비활성화
            other.gameObject.SetActive(false);
        }
        else  //부딪힌 물체가 Bullet, Enemy 가 아닐경우
        {
            // 그 물체를 없애고 싶다.
            Destroy(other.gameObject);
        }
    }
}

 */

}
