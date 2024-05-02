using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Hurt,
    Die
}

public class Enemy : MonoBehaviour
{
    public EnemyState enemyState;

    public Animator anim;

    private float speed;
    public float moveSpeed;
    public float attackSpeed;

    public float findRange;
    public float damage;
    public Transform player;

    private void Update()
    {
        if (enemyState == EnemyState.Idle)
        {
            DistanceCheck();
        }
        else if (enemyState == EnemyState.Move)
        {
            MoveUpdate();
            AttackRangeCheck();
        }
    }

    private void AttackRangeCheck()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < 1.5f && enemyState != EnemyState.Attack)
        {
            speed = 0;
            enemyState = EnemyState.Attack;
            anim.SetTrigger("attack");
        }
    }

    private void DistanceCheck()
    {
        // 플레이어 와 적 오브젝트의 거리가 범위 보다 클때
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance >= findRange)
        {
            enemyState = EnemyState.Idle;
            anim.SetBool("run", false);
            speed = 0;
        }
        // 거리가 범위보다 작을때
        else
        {
            enemyState = EnemyState.Move;
            anim.SetBool("run", true);
            speed = moveSpeed;
        }
    }

    private void MoveUpdate()
    {
        // 플레이어 의 y 값으로 하면 플레이어가 위쪽에 있을 때 기울어져서 바라봄
        transform.rotation = Quaternion.LookRotation(
            new Vector3(player.position.x, transform.position.y, player.position.z)
            - transform.position);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

}
