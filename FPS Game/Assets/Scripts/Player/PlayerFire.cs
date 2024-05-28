using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
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

    private void Start()
    {
        // 피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        // 게임 상태가 '게임중' 상태 일 때만 조작할 수 있게 함
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;

        // 마우스 오른쪽 버튼을 입력 받음
        if (Input.GetMouseButtonDown(1))
        {
            // 수류탄 오브젝트를 생성한 후 수류탄의 생성 위치를 발사 위치로 변경
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            // 수류탄의 오브젝트의 Rigidbody 컴포넌트를 가져옴
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            // 카메라의 정면 방향으로 수류탄에 물리적인 힘을 가함 (Impulse : 순간적인 힘)
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }

        // 마우스 왼쪽 버튼을 입력
        if (Input.GetMouseButtonDown(0))
        {
            // 레이를 생성한 후 발사될 위치와 진행 방향을 설정
            Ray ray = new Ray(
                Camera.main.transform.position,
                Camera.main.transform.forward);

            // 레이가 부딯인 대상의 정보를 저장할 변수를 생성
            RaycastHit hitInfo;
            if (Physics.Raycast(ray,out hitInfo))
            {
                // 만일 레이에 부딪힌 대산의 레이어가 "Enemy" 라면 데미지 함수를 실행
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFsm = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFsm.HitEnemy(weaponPower);
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
        }
    }
}
