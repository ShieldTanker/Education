using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEevnt : MonoBehaviour
{
    // 에너미 FSM 스크립트를 사용하기 위한 변수
    public EnemyFSM eFsm;

    // 플레이어에게 데미지를 입히기 위한 이벤트 함수
    public void PlayerHit()
    {
        eFsm.AttackAction();
    }
}
