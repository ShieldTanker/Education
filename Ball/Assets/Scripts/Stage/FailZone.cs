using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ball")
        {
            //Debug.Log("부딪힘");

            // 스크린 매니저로 Game 이라는 이름의 씬을 불러옴
            //SceneManager.LoadScene("Game");

            //GameManager 라는 오브젝트를찾고 그 오브젝트안의 스크립트 의 RestartGame 을 호출
            //GameObject.Find("GameManager").SendMessage("ReStartGame");
            
            GameManager gmComponent = GameObject.Find("GameManager").GetComponent<GameManager>();
            gmComponent.RestartGame();
        }
    }
}
