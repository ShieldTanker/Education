using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 이동 속도 변수
    public float moveSpeed = 7f;

    // 캐릭터 컨트롤러 변수
    CharacterController cc;

    Coroutine lowHP;

    // 중력 변수
    float gravity = -20f;

    // 수직 속도 변수
    [SerializeField]
    float yVelocity = 0f;

    // 점프력 변수
    public float jumpPower = 10f;

    // 점프 상태 변수
    public bool isJumping = false;

    // 플레이어 체력
    public int hp = 20;
    // 플레이어 최대 체력변수
    int maxHP = 20;

    // hp 슬라이더 변수
    public Slider hpSlider;

    // hit 효과 오브젝트
    public GameObject hitEffect;

    // 애니메이터 변수
    Animator anim;

    private void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        // 자식 오브젝트의 애니메이터 받아오기
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // 게임 상태가 '게임중' 상태 일 때만 조작할 수 있게 함
        if (GameManager.GM.gState != GameManager.GameState.Run)
            return;

        // 사용자의 입력을 받음
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 이동 방향을 설정
        Vector3 dir = new Vector3(h, 0, v);
        // nomalized : 벡터의 방향으로 값을 1로
        dir = dir.normalized;

        // 이동 블랜딩 트리를 호출하고 벡터의 크기 값을 넘겨줌(블랜드 트리의 파라메터는 float임)
        // magnitude : 벡터의 길이
        anim.SetFloat("moveMotion", dir.magnitude);

        // 메인 카메라를 기준으로 방향을 추가로 변환(Translate 말고 이방법도 있음)
        // 문제는 하늘을 바라보면 위로 떠버림
        dir = Camera.main.transform.TransformDirection(dir);

        // 점프 후 바닥에 닿은 상태인지 확인
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // 점프 가능하게 변경
            isJumping = false;

            // 캐릭터의 수직속도를 0으로 만듬
            yVelocity = 0f;
        }

        // 만약 Spacebar 키를 입력했으면
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // 캐릭터 수직 속도에 점프력 적용
            yVelocity = jumpPower;
            isJumping = true;
        }

        /* 이동 속도에 맞춰 이동 (캐릭터 컨트롤러로 이동함)
        transform.position += dir * moveSpeed * Time.deltaTime; */

        // 캐릭터 수직 속도에 중력 값을 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 이동속도에 맞춰 이동
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // 플레이어의 피격 함수
    public void DamageAction(int damage)
    {// 에너미의 공격력만큼 체력을 깎음
        hp -= damage;

        // 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영
        hpSlider.value = (float)hp / maxHP;

        if (hp > 0)
        {
            // 피격 이펙트 코루틴을 시작
            StartCoroutine(PlayerHitEffect());

            if (hpSlider.value <= 0.15f)
                lowHP = StartCoroutine(LowHP());
        }
        else
        {
            Debug.Log("LowHP");
            StopCoroutine(lowHP);
        }
    }

    IEnumerator LowHP()
    {
        while (hpSlider.value < 0.15f)
        {
            // 피격 UI 를 활성화
            hitEffect.SetActive(true);

            yield return new WaitForSeconds(1);

            // 피격 UI 비활성화
            hitEffect.SetActive(false);

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator PlayerHitEffect()
    {
        // 피격 UI 를 활성화
        hitEffect.SetActive(true);

        // 0.3 초간 대기
        yield return new WaitForSeconds(0.3f);

        // 피격 UI 비활성화
        hitEffect.SetActive(false);
    }
}