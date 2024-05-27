using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
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

    //���ʹ� ���� ����
    EnemyState m_State;

    // �÷��̾� Ʈ������
    Transform player;

    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    // �÷��̾� �߰� ����
    public float findDistance = 8f;
    // ���� ���� ����
    public float attackDistance = 2f;
    // ���� ������ �ð�
    float attackDelay = 2f;
    // ���ʹ� ���ݷ�
    public int attackPower = 3;
    // ���� �ð�
    float currentTime = 0f;

    // �̵� �ӵ�
    public float moveSpeed = 5;

    // �ʱ� ��ġ ����
    Vector3 originPos;

    // �̵� ���� ��ġ
    public float moveDistance = 20f;

    // ���ʹ� �� ü��
    public int hp = 15;

    // ���ʹ��� �ִ� ü��
    int maxHp = 15;

    private void Start()
    {
        // ������ ���ʹ� ���¸� ��� ���·�
        m_State = EnemyState.Idle;

        /* �÷��̾� �� Ʈ��Ʈ�� ������Ʈ �޾ƿ���
           (������Ʈ�� ���� �߰��� ���� �Ǳ⿡ �̸� �������� �� �� ����)  */
        player = GameObject.Find("Player").transform;

        // ĳ���� ������Ʈ �ҷ�����
        cc = GetComponent<CharacterController>();

        // �ڽ��� �ʱ� ��ġ�� ����
        originPos = transform.position;
    }

    private void Update()
    {
        // ���� ���� �� üũ�� ���¿� ���� �޼ҵ� ȣ��
        CheckState();
    }

    void CheckState()
    {
        // ���� ���� �� ���� �޼ҵ� ȣ��
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
                // Damaged();
                break;
            case EnemyState.Die:
                // Die();
                break;

            default:
                break;
        }
    }

    void Idle()
    {
        // ����, �÷��̾� �� �� �Ÿ��� �׼� ���� ����� Move �� ��ȯ
        if (Vector3.Distance(transform.position, player.position) <= findDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Idle -> Move");
        }
    }

    void Move()
    {
        // ����, ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ �Ѿ�ٸ�
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // ���� ���¸� ����(Return) ���� ��ȯ
            m_State = EnemyState.Return;
            print("���� ��ȯ : Move -> Return");
        }
        // ����, �÷��̾� �� �� �Ÿ��� ���� ���� �� �̶�� �÷��̾ ���� �̵�
        else if (Vector3.Distance(transform.position, player.position) >= attackDistance)
        {
            // �̵� ���� ����
            Vector3 dir = (player.position - transform.position).normalized;

            // ĳ���� ��Ʈ�ѷ� �� �̿��� �̵��ϱ�
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        // �׷��� ������
        else
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ : Move -> Attack");

            //���� �ð��� ���� ������ �ð���ŭ �̸� ������� ����
            currentTime = attackDelay;
        }
    }
    
    void Attack()
    {
        // ����, �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̾ ����
        if (Vector3.Distance(transform.position, player.position)< attackDistance)
        {
            // ������ �ð����� �÷��̾ ����
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0f;
            }
        }
        // �׷��� �ʴٸ�, ���� ���¸� �̵����� ��ȯ(���߰�)
        else
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Attack -> Move");
            currentTime = 0f;
        }
    }

    void Return()
    {
        // ����, �ʱ� ��ġ���� �Ÿ��� 0.1f �̻��̶�� �ʱ� ��ġ ������ �̵�
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        // �׷��� �ʴٸ�, �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ������¸� ���� ��ȯ
        else
        {
            transform.position = originPos;

            // hp �� �ٽ� ȸ��
            hp = maxHp;

            m_State = EnemyState.Idle;
            print("���� ��ȯ : Return -> Idle");
        }
    }

    void Damaged()
    {
        // �ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ ����
        StartCoroutine(DamagedProcess());
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamagedProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(0.5f);

        // ���� ���¸� �̵� ���·� ��ȯ
        m_State = EnemyState.Move;
        print("���� ��ȯ : Damaged -> Move");
    }

    // ������ ���� �Լ�
    public void HitEnemy(int hitPower)
    {
        // ����, �̹� �ǰ� ���� �̰ų� ��� ���� �Ǵ� ���� ���¶��
        // �ƹ� ó�� �����ʰ� �Լ� ����
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die ||
            m_State == EnemyState.Return)
        {
            return;
        }

        // �÷��̾��� ���ݷ� ��ŭ ���ʹ��� ü���� ���ҽ�Ŵ
        hp -= hitPower;

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ : AnyState -> Damaged");
            Damaged();
        }
        // �׷��� �ʴٸ� ���� ���·� ��ȯ
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ : AnyState -> Die");
            //Die();
        }
    }
}
