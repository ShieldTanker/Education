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
        // �÷��̾� �� �� ������Ʈ�� �Ÿ��� ���� ���� Ŭ��
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance >= findRange)
        {
            enemyState = EnemyState.Idle;
            anim.SetBool("run", false);
            speed = 0;
        }
        // �Ÿ��� �������� ������
        else
        {
            enemyState = EnemyState.Move;
            anim.SetBool("run", true);
            speed = moveSpeed;
        }
    }

    private void MoveUpdate()
    {
        // �÷��̾� �� y ������ �ϸ� �÷��̾ ���ʿ� ���� �� �������� �ٶ�
        transform.rotation = Quaternion.LookRotation(
            new Vector3(player.position.x, transform.position.y, player.position.z)
            - transform.position);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

}
