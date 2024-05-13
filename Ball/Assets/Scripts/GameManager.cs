using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;
    // 게임 재실행
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // 코인 먹을때
    public void GetCoin()
    {
        coinCount++;
        coinText.text = "Coin : " + coinCount;
    }

    //레드 코인 먹을때
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
