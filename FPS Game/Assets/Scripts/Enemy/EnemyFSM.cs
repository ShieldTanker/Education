using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // 에너미 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die,
    }

    //에너미 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    public float findDistance = 8f;

    // 플레이어 트랜스폼
    Transform player;

    // 공격 가능 범위
    public float attackDistance = 2f;

    // 이동 속도
    public float moveSpeed = 5;

    // 캐릭터 컨트롤러 컴포넌트
    CharacterController cc;

    private void Start()
    {
        // 최초의 에너미 상태를 대기 상태로
        m_State = EnemyState.Idle;

        /* 플레이어 의 트랜트폼 컴포넌트 받아오기
           (오브젝트가 게임 중간에 생성 되기에 미리 참조연결 할 수 없음)  */
        player = GameObject.Find("Player").transform;

        // 캐릭터 컴포넌트 불러오기
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 현재 상태 를 체크해 상태에 따라 메소드 호출
        CheckState();
    }

    void CheckState()
    {
        // 현재 상태 에 따라 메소드 호출
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
        // 만일, 플레이어 와 의 거리가 액션 범위 내라면 Move 로 전환
        if (Vector3.Distance(transform.position, player.position) <= findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Idle -> Move");
        }
    }

    void Move()
    {
        // 만일, 플레이어 와 의 거리가 공격 범위 밖 이라면 플레이어를 향해 이동
        if (Vector3.Distance(transform.position, player.position) >= attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized;

            // 캐릭터 컨트롤러 를 이용해 이동하기
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        // 그렇지 않으면
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");
        }
    }
}
