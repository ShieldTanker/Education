using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    //���� �ȿ� �ٸ� ��ü�� ���� �� ���
    private void OnTriggerEnter(Collider other)
    {   
        // �΋H�� ��ü ��Ȱ��ȭ
        other.gameObject.SetActive(false);


        //���� �ε��� ��ü�� Bullet Ȥ�� Enemy ���
        if (other.gameObject.tag == "Bullet")
        {
            //playerFireŬ���� �ҷ�����(PlayerFire �� ������ƮǮ�� ��ȯ ����� �ϱ⶧��)
            PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();

            //����Ʈ�� �Ѿ� ����
            playerFire.smallBulletObjectPool.Add(other.gameObject);
        
        }
        else if (other.gameObject.tag == "BigBullet")
        {
            PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();
            //����Ʈ�� �Ѿ� ����
            playerFire.bigBulletObjectPool.Add(other.gameObject);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyManager enManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();


            Enemy enemy = other.GetComponent<Enemy>();
            //����Ʈ�� ���ʹ� ����
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
    // ���� �ȿ� �ٸ� ��ü�� ������ ���
    private void OnTriggerEnter(Collider other)
    {
        // ���� �ε��� ��ü�� Bullet �̶��
        if (other.gameObject.name.Contains("Bullet") || other.gameObject.name.Contains("Enemy"))
        {
            // �ε��� ��ü�� ��Ȱ��ȭ
            other.gameObject.SetActive(false);
        }
        else  //�ε��� ��ü�� Bullet, Enemy �� �ƴҰ��
        {
            // �� ��ü�� ���ְ� �ʹ�.
            Destroy(other.gameObject);
        }
    }
}

 */

}
