using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    Coroutine hitCoroutine;

    //에너미 상태 변수
    EnemyState m_State;

    // 플레이어 트랜스폼
    Transform player;

    // 캐릭터 컨트롤러 컴포넌트
    CharacterController cc;

    // 플레이어 발견 범위
    public float findDistance = 8f;
    // 공격 가능 범위
    public float attackDistance = 2f;
    // 공격 딜레이 시간
    float attackDelay = 2f;
    // 에너미 공격력
    public int attackPower = 3;
    // 누적 시간
    float currentTime = 0f;

    // 이동 속도
    public float moveSpeed = 5;

    // 초기 위치 저장
    Vector3 originPos;
    Quaternion originRot;

    // 이동 가능 위치
    public float moveDistance = 20f;

    // 에너미 의 체력
    public int hp = 15;

    // 에너미의 최대 체력
    int maxHp = 15;

    // hp 슬라이더 변수
    public Slider hpSlider;

    // 애니메이터 변수
    Animator anim;


    private void Start()
    {
        // 최초의 에너미 상태를 대기 상태로
        m_State = EnemyState.Idle;

        /* 플레이어 의 트랜트폼 컴포넌트 받아오기
           (오브젝트가 게임 중간에 생성 되기에 미리 참조연결 할 수 없음)  */
        player = GameObject.Find("Player").transform;

        // 캐릭터 컴포넌트 불러오기
        cc = GetComponent<CharacterController>();

        // 자신의 초기 위치값 저장
        originPos = transform.position;
        originRot = transform.rotation;

        // 자식 오브젝트로부터 애니메이터 변수 받아오기
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // 현재 상태 를 체크해 상태에 따라 메소드 호출
        CheckState();

        // 현재 hp(%)를 슬라이더의 value 에 반영
        hpSlider.value = (float)hp / maxHp;
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
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                // Damaged(); HitEnemy() 에서 불러오기에 주석 제거 안함
                break;
            case EnemyState.Die:
                // Die(); HitEnemy() 에서 불러오기에 주석 제거 안함
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

            // 이동 애니메이션으로 전환
            anim.SetTrigger("idleToMove");
        }
    }

    void Move()
    {
        // 만일, 현재 위치가 초기 위치에서 이동 가능 범위를 넘어간다면
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 현재 상태를 복귀(Return) 으로 전환
            m_State = EnemyState.Return;
            print("상태 전환 : Move -> Return");
        }
        // 만일, 플레이어 와 의 거리가 공격 범위 밖 이라면 플레이어를 향해 이동
        else if (Vector3.Distance(transform.position, player.position) >= attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized;

            // 캐릭터 컨트롤러 를 이용해 이동하기
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // 플레이어 향해 방향 전환
            transform.forward = dir;
        }
        // 그렇지 않으면, 현재 상태를 공격(Attack) 으로 전환
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");

            //누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓음
            currentTime = attackDelay;

            //공격 대기 애니메이션 플레이
            anim.SetTrigger("moveToAttackDelay");
        }
    }
    
    void Attack()
    {
        // 만일, 플레이어가 공격 범위 이내에 있다면 플레이어를 공격
        if (Vector3.Distance(transform.position, player.position)< attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0f;

                // 공격 애니메이션 실행
                anim.SetTrigger("startAttack");
            }
        }
        // 그렇지 않다면, 현재 상태를 이동으로 전환(재추격)
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0f;

            // 공격 상태에서 이동상태로 전환
            anim.SetTrigger("attackToMove");
        }
    }

    // 플레이어의 스크립트의 데미지 처리 함수를 실행
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        // 만일, 초기 위치에서 거리가 0.1f 이상이라면 초기 위치 쪽으로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // 복귀 지점으로 방향 전환 
            transform.forward = dir;
        }
        // 그렇지 않다면, 자신의 위치를 초기 위치로 조정하고 현재상태를 대기로 전환
        else
        {
            transform.position = originPos;
            transform.rotation = originRot;

            // hp 를 다시 회복
            hp = maxHp;

            m_State = EnemyState.Idle;
            print("상태 전환 : Return -> Idle");

            // 대기 애니메이션 재생
            anim.SetTrigger("moveToIdle");
        }
    }

    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴 실행
        hitCoroutine = StartCoroutine(DamagedProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamagedProcess()
    {
        // 피격 모션 시간만큼 기다림
        yield return new WaitForSeconds(1f);

        // 현재 상태를 이동 상태로 전환
        m_State = EnemyState.Move;
        print("상태 전환 : Damaged -> Move");
    }

    // 데미지 실행 함수
    // 플레이어 파이어 쪽에서 호출
    public void HitEnemy(int hitPower)
    {
        // 만일, 이미 피격 상태 이거나 사망 상태 또는 복귀 상태라면
        // 아무 처리 하지않고 함수 종료
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die ||
            m_State == EnemyState.Return)
        {
            return;
        }

        // 플레이어의 공격력 만큼 에너미의 체력을 감소시킴
        hp -= hitPower;

        // 에너미의 체력이 0보다 크면 피격 상태로 전환
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환 : AnyState -> Damaged");

            // 피격 애니메이션을 플레이
            anim.SetTrigger("damaged");

            Damaged();
        }
        // 그렇지 않다면 죽음 상태로 전환
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환 : AnyState -> Die");

            // 죽음 애니메이션 재생
            anim.SetTrigger("die");

            Die();
        }
    }

    void Die()
    {
        // 진행중인 피격 코루틴 중지
        StopCoroutine(hitCoroutine);

        // 죽음 상태를 처리하기 위한 코루틴을 실행
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 컨틀롤러 컴포넌트를 비활성화
        cc.enabled = false;

        // 2초 동안 기다린 후에 자기 자신을 제거
        yield return new WaitForSeconds(2f);

        print("소멸");
        Destroy(gameObject);
    }
}
