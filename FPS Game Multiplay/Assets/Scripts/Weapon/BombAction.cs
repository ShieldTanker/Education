using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BombAction : NetworkBehaviour
{
    // 폭발 이펙트 프리팹 변수
    public GameObject bombEffect;

    // 수류탄 데미지
    public int attackPower = 10;

    // 폭발 효과 반경
    public float explosionRadius = 5f;

    // 충돌했을 때의 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 폭발 효과 반경 내에서 레이어가 'Enemy'인 모든 게임 오브젝트들의
        // Collider 컴포넌트를 배열에 저장
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Collider[] cols = Physics.OverlapSphere(
            transform.position, explosionRadius, 1 << enemyLayer);
        
        // 저장된 Collider 배열에 있는 모든 에너미에게 수류탄 데미지를 적용
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }

        // 이펙트 프리팹을 생성
        Runner.Spawn(bombEffect, transform.position, Quaternion.identity);

        // 자기 자신을 제거
        // Object : 퓨전에서 처리되고있는 오브젝트 타입
        Runner.Despawn(Object);
    }
}
