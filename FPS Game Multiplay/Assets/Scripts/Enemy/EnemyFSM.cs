using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Fusion;
using System.Threading.Tasks;

public class EnemyFSM : NetworkBehaviour
{
    // ���ʹ� ���� ���
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die,
    }

    // ���ʹ� ���� ����
    EnemyState m_State;

    // �÷��̾� �߰� ����
    public float findDistance = 8f;

    // ���� ����� �÷��̾�
    GameObject nearPlayer = null;


    // ���� ���� ����
    public float attackDistance = 2f;

    // �̵� �ӵ�
    public float moveSpeed = 5f;

    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    // ���� �ð�
    float currentTime = 0;

    // ���� ������ �ð�
    float attackDelay = 2f;

    // ���ʹ� ���ݷ�
    public int attackPower = 3;

    // �ʱ� ��ġ ����� ����
    Vector3 originPos;
    Quaternion originRot;

    // �̵� ���� ����
    public float moveDistance = 20f;

    // ���ʹ��� ü��
    // [Networked] �� �Ӽ����� ����ؾ���
    [Networked] public int hp { get; set; } = 15;

    // ���ʹ��� �ִ� ü��
    int maxHp = 15;

    // ���ʹ� hp �����̴� ����
    public Slider hpSlider;

    // �ִϸ����� ����
    Animator anim;

    // ������̼� ������Ʈ ����
    NavMeshAgent smith;

    private void Start()
    {
        // ������ ���ʹ� ���¸� ���
        m_State = EnemyState.Idle;

        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ڽ��� �ʱ� ��ġ �����ϱ�
        originPos = transform.position;
        originRot = transform.rotation;

        // �ڽ� ������Ʈ�κ��� �ִϸ����� ���� �޾ƿ���
        anim = GetComponentInChildren<Animator>();

        // ������̼� ������Ʈ ������Ʈ �޾ƿ���
        smith = GetComponent<NavMeshAgent>();
    }

    public override void FixedUpdateNetwork()
    {
        // ���� ���¸� üũ�� �ش� ���º��� ������ ����� ����
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        // ���� hp(%)�� hp �����̴��� value�� �ݿ�
        hpSlider.value = (float)hp / maxHp;
    }

    void Idle()
    {
        // ����, �ƹ� �÷��̾�� �Ÿ��� �׼� ���� �̳���� Move ���·� ��ȯ
        foreach (var player in GameManager.gm.players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < findDistance)
            {
                m_State = EnemyState.Move;
                print("���� ��ȯ: Idle -> Move");

                // �̵� �ִϸ��̼����� ��ȯ
                anim.SetTrigger("IdleToMove");
            }
        }
    }

    void Move()
    {
        // ����, ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ �Ѿ�ٸ�
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // ���� ���¸� ����(Return)�� ��ȯ
            m_State = EnemyState.Return;
            print("���� ��ȯ : Move -> Return");

            return;
        }

        // �÷��̾� ����Ʈ�� ���� ����� �÷��̾� �˻�
        float minDistance = float.MaxValue;
        foreach (var player in GameManager.gm.players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (minDistance > distance)
            {
                minDistance = distance;
                nearPlayer = player;
            }
        }

        // ����, �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵�
        if (minDistance > attackDistance)
        {
            // ������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� ����
            smith.stoppingDistance = attackDistance;

            // ������̼��� �������� �÷��̾��� ��ġ�� ����
            smith.destination = nearPlayer.transform.position;
        }
        // �׷��� �ʴٸ�, ���� ���¸� ����(Attack)���� ��ȯ
        else
        {
            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ
            smith.isStopped = true;
            smith.ResetPath();

            m_State = EnemyState.Attack;
            print("���� ��ȯ: Move -> Attack");

            // ���� �ð��� ���� ������ �ð���ŭ �̸� ������� ����
            currentTime = attackDelay;

            // ���� ��� �ִϸ��̼� �÷���
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        // ����, �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̾ ����
        if (Vector3.Distance(transform.position, nearPlayer.transform.position) < attackDistance)
        {
            // ������ �ð����� �÷��̾ ����
            currentTime += Runner.DeltaTime;
            if (currentTime > attackDelay)
            {
                // player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;

                // ���� �ִϸ��̼� �÷���
                anim.SetTrigger("StartAttack");
            }
        }
        // �׷��� �ʴٸ�, ������¸� �̵����� ��ȯ(���߰� �ǽ�)
        else
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Attack -> Move");
            currentTime = 0;

            // �̵� �ִϸ��̼� �÷���
            anim.SetTrigger("AttackToMove");
        }
    }

    // �÷��̾��� ��ũ��Ʈ�� ������ ó�� �Լ��� ����
    public void AttackAction()
    {
        nearPlayer.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        // ����, �ʱ� ��ġ���� �Ÿ��� 0.1f �̻��̶�� �ʱ� ��ġ ������ �̵�
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            // ������̼��� �������� �ʱ� ����� ��ġ�� ����
            smith.destination = originPos;

            // ������̼����� �����ϴ� �ּ� �Ÿ��� 0���� ����
            smith.stoppingDistance = 0;
        }
        // �׷��� �ʴٸ�, �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ���� ���¸� ���� ��ȯ
        else
        {
            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ
            smith.isStopped = true;
            smith.ResetPath();

            // ��ġ ���� ȸ�� ���� �ʱ� ���·� ��ȯ
            transform.position = originPos;
            transform.rotation = originRot;

            // hp�� �ٽ� ȸ��
            hp = maxHp;

            m_State = EnemyState.Idle;
            print("���� ��ȯ : Return -> Idle");

            // ��� �ִϸ��̼����� ��ȯ�ϴ� Ʈ�������� ȣ��
            anim.SetTrigger("MoveToIdle");
        }
    }

    void Damaged()
    {
        // �ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ ����
        StartCoroutine(DamageProcess());
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(1f);

        // ���� ���¸� �̵� ���·� ��ȯ
        m_State = EnemyState.Move;
        print("���� ��ȯ : Damaged => Move");
    }
    
    // ������ ���� �Լ�
    // async : �񵿱�
    public async void HitEnemy(int hitPower)
    {
        // ����, �̹� �ǰ� �����̰ų� ��� ���� �Ǵ� ���� ���¶��
        // �ƹ��� ó���� ���� �ʰ� �Լ��� ����
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die 
            || m_State == EnemyState.Return)
        {
            return;
        }

        // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ
        smith.isStopped = true;
        smith.ResetPath();

        // ������Ʈ�� ���� ������ ������
        if (!Object.HasStateAuthority)
        {
            // ���±��� ��û
            Object.RequestStateAuthority();
            while(Object.HasStateAuthority)
            {
                // 0.1 �ʸ� ��� ��(�̸��� ����)
                await Task.Delay(100);
            }
        }

        // �÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ŵ
        hp -= hitPower;

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ : Any state -> Damaged");

            // �ǰ� �ִϸ��̼��� �÷���
            anim.SetTrigger("Damaged");

            Damaged();
        }
        // �׷��� �ʴٸ� ���� ���·� ��ȯ
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ : Any state -> Die");

            // ���� �ִϸ��̼��� �÷���
            anim.SetTrigger("Die");
            Die();
        }
    }

    // ���� ���� �Լ�
    void Die()
    {
        // �������� �ǰ� �ڷ�ƾ�� ����
        StopAllCoroutines();

        // ���� ���¸� ó���ϱ� ���� �ڷ�ƾ�� ����
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ�� ��Ȱ��ȭ
        cc.enabled = false;

        // 2�� ���� ��ٸ� �Ŀ� �ڱ� �ڽ��� ����
        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }
}
