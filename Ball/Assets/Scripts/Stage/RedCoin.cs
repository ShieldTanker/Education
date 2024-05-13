using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCoin : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        //Ball 오브젝트와 부딫혔을때
        if (col.gameObject.name == "Ball")
        {
            // DestroyObstacles() 메소드 실행
            //DestroyObstacles();

            GameManager gmComponent = GameObject.Find("GameManager").GetComponent<GameManager>();
            gmComponent.RedCoinStart();

            // 없애다 (자기자신의 오브젝트) 라는 뜻
            Destroy(gameObject);
        }
    }

}
