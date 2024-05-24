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

    // �÷��̾� �߰� ����
    public float findDistance = 8f;

    // �÷��̾� Ʈ������
    Transform player;

    // ���� ���� ����
    public float attackDistance = 2f;

    // �̵� �ӵ�
    public float moveSpeed = 5;

    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    private void Start()
    {
        // ������ ���ʹ� ���¸� ��� ���·�
        m_State = EnemyState.Idle;

        /* �÷��̾� �� Ʈ��Ʈ�� ������Ʈ �޾ƿ���
           (������Ʈ�� ���� �߰��� ���� �Ǳ⿡ �̸� �������� �� �� ����)  */
        player = GameObject.Find("Player").transform;

        // ĳ���� ������Ʈ �ҷ�����
        cc = GetComponent<CharacterController>();
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
                // Attack();
                break;
            case EnemyState.Return:
                // Return();
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
        // ����, �÷��̾� �� �� �Ÿ��� ���� ���� �� �̶�� �÷��̾ ���� �̵�
        if (Vector3.Distance(transform.position, player.position) >= attackDistance)
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
        }
    }
}
