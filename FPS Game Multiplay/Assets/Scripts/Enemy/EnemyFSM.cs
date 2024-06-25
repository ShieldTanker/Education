using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Fusion;
using System.Threading.Tasks;

public class EnemyFSM : NetworkBehaviour
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

    // 에너미 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    public float findDistance = 8f;

    // 가장 가까운 플레이어
    GameObject nearPlayer = null;


    // 공격 가능 범위
    public float attackDistance = 2f;

    // 이동 속도
    public float moveSpeed = 5f;

    // 캐릭터 컨트롤러 컴포넌트
    CharacterController cc;

    // 누적 시간
    float currentTime = 0;

    // 공격 딜레이 시간
    float attackDelay = 2f;

    // 에너미 공격력
    public int attackPower = 3;

    // 초기 위치 저장용 변수
    Vector3 originPos;
    Quaternion originRot;

    // 이동 가능 범위
    public float moveDistance = 20f;

    // 에너미의 체력
    // [Networked] 는 속성으로 사용해야함
    [Networked] public int hp { get; set; } = 15;

    // 에너미의 최대 체력
    int maxHp = 15;

    // 에너미 hp 슬라이더 변수
    public Slider hpSlider;

    // 애니메이터 변수
    Animator anim;

    // 내비게이션 에이전트 변수
    NavMeshAgent smith;

    private void Start()
    {
        // 최초의 에너미 상태를 대기
        m_State = EnemyState.Idle;

        // 캐릭터 컴트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        // 자신의 초기 위치 저장하기
        originPos = transform.position;
        originRot = transform.rotation;

        // 자식 오브젝트로부터 애니메이터 변수 받아오기
        anim = GetComponentInChildren<Animator>();

        // 내비게이션 에이전트 컴포넌트 받아오기
        smith = GetComponent<NavMeshAgent>();
    }

    public override void FixedUpdateNetwork()
    {
        // 현재 상태를 체크해 해당 상태별로 정해진 기능을 수행
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

        // 현재 hp(%)를 hp 슬라이더의 value에 반영
        hpSlider.value = (float)hp / maxHp;
    }

    void Idle()
    {
        // 만일, 아무 플레이어와 거리가 액션 범위 이내라면 Move 상태로 전환
        foreach (var player in GameManager.gm.players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < findDistance)
            {
                m_State = EnemyState.Move;
                print("상태 전환: Idle -> Move");

                // 이동 애니메이션으로 전환
                anim.SetTrigger("IdleToMove");
            }
        }
    }

    void Move()
    {
        // 만일, 현재 위치가 초기 위치에서 이동 가능 범위를 넘어간다면
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 현재 상태를 복귀(Return)로 전환
            m_State = EnemyState.Return;
            print("상태 전환 : Move -> Return");

            return;
        }

        // 플레이어 리스트중 가장 가까운 플레이어 검색
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

        // 만일, 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동
        if (minDistance > attackDistance)
        {
            // 내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정
            smith.stoppingDistance = attackDistance;

            // 내비게이션의 목적지를 플레이어의 위치로 설정
            smith.destination = nearPlayer.transform.position;
        }
        // 그렇지 않다면, 현재 상태를 공격(Attack)으로 전환
        else
        {
            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화
            smith.isStopped = true;
            smith.ResetPath();

            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓음
            currentTime = attackDelay;

            // 공격 대기 애니메이션 플레이
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        // 만일, 플레이어가 공격 범위 이내에 있다면 플레이어를 공격
        if (Vector3.Distance(transform.position, nearPlayer.transform.position) < attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격
            currentTime += Runner.DeltaTime;
            if (currentTime > attackDelay)
            {
                // player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;

                // 공격 애니메이션 플레이
                anim.SetTrigger("StartAttack");
            }
        }
        // 그렇지 않다면, 현재상태를 이동으로 전환(재추격 실시)
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0;

            // 이동 애니메이션 플레이
            anim.SetTrigger("AttackToMove");
        }
    }

    // 플레이어의 스크립트의 데미지 처리 함수를 실행
    public void AttackAction()
    {
        nearPlayer.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        // 만일, 초기 위치에서 거리가 0.1f 이상이라면 초기 위치 쪽으로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            // 내비게이션의 목적지를 초기 저장된 위치로 설정
            smith.destination = originPos;

            // 내비게이션으로 접근하는 최소 거리를 0으로 설정
            smith.stoppingDistance = 0;
        }
        // 그렇지 않다면, 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환
        else
        {
            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화
            smith.isStopped = true;
            smith.ResetPath();

            // 위치 값과 회전 값을 초기 상태로 변환
            transform.position = originPos;
            transform.rotation = originRot;

            // hp를 다시 회복
            hp = maxHp;

            m_State = EnemyState.Idle;
            print("상태 전환 : Return -> Idle");

            // 대기 애니메이션으로 전환하는 트랜지션을 호출
            anim.SetTrigger("MoveToIdle");
        }
    }

    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다림
        yield return new WaitForSeconds(1f);

        // 현재 상태를 이동 상태로 전환
        m_State = EnemyState.Move;
        print("상태 전환 : Damaged => Move");
    }
    
    // 데미지 실행 함수
    // async : 비동기
    public async void HitEnemy(int hitPower)
    {
        // 만일, 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면
        // 아무런 처리도 하지 않고 함수를 종료
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die 
            || m_State == EnemyState.Return)
        {
            return;
        }

        // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화
        smith.isStopped = true;
        smith.ResetPath();

        // 오브젝트에 상태 권한이 없으면
        if (!Object.HasStateAuthority)
        {
            // 상태권한 요청
            Object.RequestStateAuthority();
            while(Object.HasStateAuthority)
            {
                // 0.1 초를 잠깐 쉼(미리초 단위)
                await Task.Delay(100);
            }
        }

        // 플레이어의 공격력만큼 에너미의 체력을 감소시킴
        hp -= hitPower;

        // 에너미의 체력이 0보다 크면 피격 상태로 전환
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환 : Any state -> Damaged");

            // 피격 애니메이션을 플레이
            anim.SetTrigger("Damaged");

            Damaged();
        }
        // 그렇지 않다면 죽음 상태로 전환
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환 : Any state -> Die");

            // 죽음 애니메이션을 플레이
            anim.SetTrigger("Die");
            Die();
        }
    }

    // 죽음 상태 함수
    void Die()
    {
        // 진행중인 피격 코루틴을 중지
        StopAllCoroutines();

        // 죽음 상태를 처리하기 위한 코루틴을 실행
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 컨트롤러 컴포넌트를 비활성화
        cc.enabled = false;

        // 2초 동안 기다린 후에 자기 자신을 제거
        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);
    }
}
