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

    // 중력 변수
    float gravity = -20f;

    // 수직 속도 변수
    float yVelocity = 0;

    // 점프력 변수
    public float jumpPower = 10f;

    // 점프 상태 변수
    public bool isJumping = false;

    // 플레이어 체력 변수
    public int hp = 20;

    // 최대 체력 변수
    int maxHp = 20;

    // hp 슬라이더 변수
    public Slider hpSlider;

    // Hit 효과 오브젝트
    public GameObject hitEffect;

    // 애니메이터 변수
    Animator anim;

    private void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        // 애니메이터 받아오기
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 함
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;

        // 사용자의 입력을 받음
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 이동 방향을 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 이동 블랜딩 트리를 호출하고 벡터의 크기 값을 넘겨줌
        anim.SetFloat("MoveMotion", dir.magnitude);

        // 메인 카메라를 기준으로 방향을 변환
        dir = Camera.main.transform.TransformDirection(dir);

        // 만일, 점프 중이었고, 다시 바닥에 착지했다면
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // 점프 전 상태로 초기화
            isJumping = false;

            // 캐릭터 수직 속도를 0으로 만듦
            yVelocity = 0;
        }

        // 만일 키보드 Spacebar 키를 입력했다면
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // 캐릭터 수직 속도에 점프력을 적용
            yVelocity = jumpPower;
            isJumping = true;
        }

        // 캐릭터 수직 속도에 중력 값을 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 이동 속도에 맞춰 이동
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영
        hpSlider.value = (float)hp / maxHp;
    }

    // 플레이어의 피격 함수
    public void DamageAction(int damage)
    {
        // 에너미의 공격력만큼 플레이어의 체력을 깎음
        hp -= damage;

        // 만일, 플레이어의 체력이 0보다 크면 피격 효과를 출력
        if (hp > 0)
        {
            // 피격 이펙트 코루틴을 시작
            StartCoroutine(PlayHitEffect());
        }
    }

    // 피격 효과 코루틴 함수
    IEnumerator PlayHitEffect()
    {
        // 피격 UI를 활성화
        hitEffect.SetActive(true);
        // 0.3초간 대기
        yield return new WaitForSeconds(0.3f);
        // 피격 UI를 비활성화
        hitEffect.SetActive(false);
    }
}
