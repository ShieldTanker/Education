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
            //Debug.Log("�ε���");

            // ��ũ�� �Ŵ����� Game �̶�� �̸��� ���� �ҷ���
            //SceneManager.LoadScene("Game");

            //GameManager ��� ������Ʈ��ã�� �� ������Ʈ���� ��ũ��Ʈ �� RestartGame �� ȣ��
            //GameObject.Find("GameManager").SendMessage("ReStartGame");
            
            GameManager gmComponent = GameObject.Find("GameManager").GetComponent<GameManager>();
            gmComponent.RestartGame();
        }
    }
}
