using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public AudioClip gunSound;
    public AudioSource audio;

    // 발사 위치
    public GameObject firePosition;

    // 투척 무기 오브젝트
    public GameObject bombFactory;

    // 투척 파워
    public float throwPower = 15f;

    // 피격 이펙트 오브젝트
    public GameObject bulletEffect;

    // 피격 이펙트 파티클 시스템
    ParticleSystem ps;

    // 발사 무기 공격력
    public int weaponPower = 5;

    // 애니메이터 변수
    Animator anim;

    //무기 모드 변수
    enum WeaponMode
    {
        Normal,
        Sniper,
    }
    // 카메라 확대 확인용 변수
    WeaponMode wMode;

    bool zoomMode = false;

    // 무기 모드 텍스트
    public Text wModeText;

    // 무기 아이콘 스프라이트 변수
    public GameObject weapon01;
    public GameObject weapon02;

    // 크로스헤어 스프라이트 변수
    public GameObject crossHair01;
    public GameObject crossHair02;

    // 마우스 오른쪽 버튼 클릭 아이콘 스프라이트 변수
    public GameObject weapon01_R;
    public GameObject weapon02_R;

    // 총발사 효과 모음
    public GameObject[] effects;

    // 마우스 오른쪽 버튼 클릭 줌 모드 스프라이트 변수
    public GameObject crosshair02_zoom;

    private void Start()
    {
        // 피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();

        // 자식오브젝트의 애니메이터 불러오기
        anim = GetComponentInChildren<Animator>();

        // 무기 초기 모드를 노말 모드로 설정
        wMode = WeaponMode.Normal;
    }

    private void Update()
    {
        // 게임 상태가 '게임중' 상태 일 때만 조작할 수 있게 함
        if (GameManager.GM.gState != GameManager.GameState.Run)
            return;

        // 마우스 오른쪽 버튼을 입력 받음
        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:
                    // 수류탄 오브젝트를 생성한 후 수류탄의 생성 위치를 발사 위치로 변경
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    // 수류탄의 오브젝트의 Rigidbody 컴포넌트를 가져옴
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    // 카메라의 정면 방향으로 수류탄에 물리적인 힘을 가함 (Impulse : 순간적인 힘)
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;

                case WeaponMode.Sniper:
                    // 만일, 줌 모드 상태가 아니라면 카메라를 확대 하고 줌 모드 상태로 변경
                    if (!zoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        zoomMode = true;

                        // 줌 모드 일때 크로스헤어를 변경
                        crosshair02_zoom.SetActive(true);
                        crossHair02.SetActive(false);
                    }
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;

                        // 크로스헤어를 스나이퍼 모드로 돌려놓기 
                        crosshair02_zoom.SetActive(false);
                        crossHair02.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }

        // 마우스 왼쪽 버튼을 입력
        if (Input.GetMouseButtonDown(0))
        {
            audio.clip = gunSound;
            audio.Play();

            // 만일 이동 블랜드 트리 파라미터의 값이 0 이라면, 공격 애니메이션 실시
            if (anim.GetFloat("moveMotion") == 0)
            {
                anim.SetTrigger("attack");
            }

            // 레이를 생성한 후 발사될 위치와 진행 방향을 설정
            Ray ray = new Ray(
                Camera.main.transform.position,
                Camera.main.transform.forward);

            // 레이가 부딯인 대상의 정보를 저장할 변수를 생성
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 만일 레이에 부딪힌 대산의 레이어가 "Enemy" 라면 데미지 함수를 실행
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFsm = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFsm.HitEnemy(weaponPower);
                }
                else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("EnemyHead"))
                {
                    HeadShot eHead = hitInfo.transform.GetComponent<HeadShot>();
                    eHead.EnemyHeadShot();
                }
                // 그렇지 않다면 레이에 부딪힌 지점에 피격 이펙트 플레이
                else
                {
                    // 피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동
                    bulletEffect.transform.position = hitInfo.point;

                    /* 피격 이펙트 의  forward 방향을 레이가
                     부딪힌 지점의 법선(부딫힌 면의 수직) 벡터와 일치 시킴 */
                    bulletEffect.transform.forward = hitInfo.normal;

                    // 피격 이펙트 플레이
                    ps.Play();
                }
            }
            // 총 이펙트를 실시
            StartCoroutine(ShootEffectOn(0.05f));
        }

        // 만일 키보드의 숫자 1번 입력을 받으면, 무기 모드를 일반 모트로 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 스나이퍼 모드에서 일반 모드 키를 눌렀을때 Weapone01_R_zoom 은 비활성화
            crosshair02_zoom.SetActive(false);
            zoomMode = false;

            wMode = WeaponMode.Normal;

            // 일반 모드 텍스트 출력
            wModeText.text = "Normal Mode";

            // 카메라의 화면을 다시 원래대로 돌려줌
            Camera.main.fieldOfView = 60;

            // 1번 스프라이트는 활성화, 2번 스프라이트는 비활성화
            weapon01.SetActive(true);
            weapon01_R.SetActive(true);
            crossHair01.SetActive(true);

            weapon02.SetActive(false);
            weapon02_R.SetActive(false);
            crossHair02.SetActive(false);
        }
        // 만일 키보드의 숫자 2번입력을 받으면, 무기 모드를 스나이퍼 모드로 변경
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            // 스나이퍼 모드 텍스트 출력
            wModeText.text = "Sniper Mode";

            // 2번 스프라이트는 활성화, 1번 스프라이트는 비활성화
            weapon02.SetActive(true);
            weapon02_R.SetActive(true);
            crossHair02.SetActive(true);

            weapon01.SetActive(false);
            weapon01_R.SetActive(false);
            crossHair01.SetActive(false);
        }
    }

    // 총구 이펙트 코루틴 함수
    IEnumerator ShootEffectOn(float duration)
    {
        // 랜덤하게 숫자를 뽑음
        int num = Random.Range(0, effects.Length);
        // 이펙트 오브젝트 배열에서 뽑힌 숫자에 해당하는 이펙트 오브젝트를 활성화
        effects[num].SetActive(true);
        // 지정한 시간 만큼 기다림
        yield return new WaitForSeconds(duration);
        // 이펙트 오브젝트를 비활성화
        effects[num].SetActive(false);
    }
}
