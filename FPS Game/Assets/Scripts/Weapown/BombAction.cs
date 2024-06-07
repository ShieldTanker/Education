using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // 폭발 이펙트 프리팹 변수
    public GameObject bombEffect;

    // 수류탄 데미지
    public int attackPower = 10;

    // 폭발 효과 반경
    public float explosionRadius = 5f;

    // 충돌시 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 폭발 효과 반경 내에서 레이어가 "Enemy" 인 모근 게임 오브젝트 들의
        // Collider 컴포넌트를 배열에 저장
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        /* 8번 레이어 일경우 2^8 이기에 시프트 연산자로 8자리를 자리옮김(비트로 처리)
           00100010000 이면 8번 레이어와 4번 레이어 둘다 확인 가능

           두개 이상 레이어 하고 싶으면( 1 << enemyLayer | 다른 레이어 )
           이런식으로 하면 됨 */
        Collider[] cols = Physics.OverlapSphere(
            transform.position, explosionRadius, 1 << enemyLayer);

        // 저장된 Collider 배열의 있는 모든 에너미에세 수류탄 데미지를 적용
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }

        // 이펙트 프리팹 생성
        GameObject eff = Instantiate(bombEffect);

        // 이펙트의 위치는 자기 자신과 동일
        eff.transform.position = transform.position;

        // 자기자신 제거
        Destroy(gameObject);
    }
}
