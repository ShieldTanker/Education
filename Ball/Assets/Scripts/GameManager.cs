using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;
    // ���� �����
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // ���� ������
    public void GetCoin()
    {
        coinCount++;
        coinText.text = "Coin : " + coinCount;
    }

    //���� ���� ������
   public void RedCoinStart()
    {
        DestroyObstacles(); 
    }
    void DestroyObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        for (int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i]);
        }
    }
}
