using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayManager : MonoBehaviour
{    
    // 플레이 상태 관련
    public bool playEnd;
    public float limitTime;
    public int enemyCount;

    public Text timeLabel;
    public Text enemyLabel;
    public GameObject finalGUI;
    public Text finalMessage;
    public Text finalScoreLabel;
    public PlayerController pc;


    // 플레이어 랭킹 관련
    public Text playerName;
    private float[] bestScore = new float[3];
    private string[] bestName = new string[3];

    private void Start()
    {
        enemyLabel.text = string.Format("Enemy {0}", enemyCount);
        UpdateTimeLabel();

        playerName.text = PlayerPrefs.GetString("UserName");
    }
    private void Update()
    {
        if (playEnd)
            return;

        if (limitTime > 0)
        {
            limitTime -= Time.deltaTime;
            UpdateTimeLabel();
        }
        else
        {
            limitTime = 0;
            UpdateTimeLabel();
            GameOver();
        }
    }

    // 적 전부 제거시 호출
    public void Clear()
    {
        if (playEnd)
            return;

        // 게임 속도
        Time.timeScale = 0;
        
        playEnd = true;
        finalMessage.text = "CLEAR!!";

        float score = 12345f + limitTime * 123f + pc.hp * 123f;
        finalScoreLabel.text = string.Format("{0:N0}", score);

        finalGUI.SetActive(true);

        ScoreSet(PlayerPrefs.GetString("UserName"), score);
    }

    // 게임오버시 호출
    public void GameOver()
    {
        if (playEnd)
            return;

        Time.timeScale = 0;

        playEnd = true;
        finalMessage.text = "Fail...";

        // 남은 적 수 만큼 점수 제거
        float score = 1234f - enemyCount * 123f;
        finalScoreLabel.text = string.Format("{0:N0}", score);

        finalGUI.SetActive(true);

        // 시간초과 되어도 사격가능한 버그 수정
        pc.playerState = PlayerState.Dead;

        ScoreSet(PlayerPrefs.GetString("UserName"),score);
    }
    private void UpdateTimeLabel()
    {
        timeLabel.text = string.Format("Time : {0:N2}", limitTime);
    }

    public void EnemyDie()
    {
        // 연습문제 2. 적 제거시 5초 증가
        limitTime += 5;

        enemyCount--;
        enemyLabel.text = string.Format("Enemy : {0}", enemyCount);

        if (enemyCount <= 0)
            Clear();
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainPlay");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    public void ScoreSet(string currentName, float currentScore)
    {

        float tmpScore = 0f;
        string tmpName = "";

        for (int i = 0; i < 3; i++)
        {
            bestName[i] = PlayerPrefs.GetString(i + "BestName");
            bestScore[i] = PlayerPrefs.GetFloat(i + "BestScore");

            while (bestScore[i] < currentScore)
            {
                tmpName = bestName[i];
                tmpScore = bestScore[i];

                bestName[i] = currentName;
                bestScore[i] = currentScore;

                PlayerPrefs.GetString(i + "BestName", currentName);
                PlayerPrefs.GetFloat(i + "BestScore", currentScore);

                currentScore = tmpScore;
                currentName = tmpName;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetString(i + "BestName", bestName[i]);
            PlayerPrefs.SetFloat(i + "BestScore", bestScore[i]);
        }
    }
}
