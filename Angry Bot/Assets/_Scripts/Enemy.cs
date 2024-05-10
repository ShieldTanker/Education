using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private AudioSource audioSrc;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public Transform fxPoint;
    public GameObject hitFx;

    public GameObject guiPivot;
    public Slider lifeBar;
    public float maxHp;
    public float hp;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

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
        // �������� ���� �ѹ� �� ȣ��Ǿ ����
        if (enemyState == EnemyState.Die)
            return;

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

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            enemyState = EnemyState.Hurt;
            speed = 0;
            anim.SetTrigger("hurt");

            GameObject fx = Instantiate(
                hitFx, fxPoint.position, Quaternion.LookRotation(fxPoint.forward));

            hp -= damage;
            // �����̴� MaxValue �� �ٲ㵵 ������� ���⼱ ������ ó����
            lifeBar.value = hp / maxHp;

            audioSrc.clip = hitSound;
            audioSrc.Play();
        }

        if (hp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        enemyState = EnemyState.Die;
        anim.SetTrigger("die");
        speed = 0;

        guiPivot.SetActive(false);
        audioSrc.clip = deathSound;
        audioSrc.Play();

        Collider col = gameObject.GetComponent<Collider>();
        col.enabled = false;

        PlayManager pm = GameObject.Find("PlayManager").GetComponent<PlayManager>();
        pm.EnemyDie();
    }

    public void AttackOn()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.Hurt(damage);
    }
}
