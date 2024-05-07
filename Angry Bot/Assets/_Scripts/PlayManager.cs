using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    // �÷��� ���� ����
    public bool playEnd;
    public float limitTime;
    public int enemyCount;

    public Text timeLabel;
    public Text enemyLabel;
    public GameObject finalGUI;
    public Text finalMessage;
    public Text finalScoreLabel;
    public PlayerController pc;

    private void Start()
    {
        enemyLabel.text = string.Format("Enemy {0}", enemyCount);
        UpdateTimeLabel();
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

    // �� ���� ���Ž� ȣ��
    public void Clear()
    {
        if (playEnd)
            return;

        // ���� �ӵ�
        Time.timeScale = 0;
        
        playEnd = true;
        finalMessage.text = "CLEAR!!";

        // PlayerController pc = GameObject.Find("Player").GetComponent<PlayerController>();

        float score = 12345f + limitTime * 123f + pc.hp * 123f;
        finalScoreLabel.text = string.Format("{0:N0}", score);

        finalGUI.SetActive(true);

    }

    // ���ӿ����� ȣ��
    public void GameOver()
    {
        if (playEnd)
            return;

        Time.timeScale = 0;

        playEnd = true;
        finalMessage.text = "Fail...";

        // ���� �� �� ��ŭ ���� ����
        float score = 1234f - enemyCount * 123f;
        finalScoreLabel.text = string.Format("{0:N0}", score);

        finalGUI.SetActive(true);

        // �ð��ʰ� �Ǿ ��ݰ����� ���� ����
        pc.playerState = PlayerState.Dead;
    }
    private void UpdateTimeLabel()
    {
        timeLabel.text = string.Format("Time : {0:N2}", limitTime);
    }

    public void EnemyDie()
    {
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
        // ���� �ش� ���� ���� ���߿� ���鿹��
        SceneManager.LoadScene("Title");
    }
}
