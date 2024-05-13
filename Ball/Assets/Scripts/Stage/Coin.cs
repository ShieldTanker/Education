using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        //Ball 오브젝트와 부딫혔을때
        if (col.gameObject.name == "Ball")
        {
            // 게임매니저 의 GetCoin 이라는 메시지 호출
            GameManager gmComponent = GameObject.Find("GameManager").GetComponent<GameManager>();
            gmComponent.GetCoin();
            // 없애다 (자기자신의 오브젝트) 라는 뜻
            Destroy(gameObject);
        }
    }
}
