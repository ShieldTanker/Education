using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;


    // 충돌시 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 이펙트 프리팹 생성
        GameObject eff = Instantiate(bombEffect);

        // 이펙트의 위치는 자기 자신과 동일
        eff.transform.position = transform.position;

        // 자기자신 제거
        Destroy(gameObject);
    }
}
