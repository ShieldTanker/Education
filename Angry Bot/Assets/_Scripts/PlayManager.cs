using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// Ȯ�� �޼ҵ� �߰�
using System.Linq;


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


    // �÷��̾� ��ŷ ����
    public Text playerName;
    static public int rankUserCnt = 3;
    private float[] bestScore;
    private string[] bestName;

    private void Start()
    {
        bestScore = new float[rankUserCnt];
        bestName = new string[rankUserCnt];

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

    // �� ���� ���Ž� ȣ��
    public void Clear()
    {
        if (playEnd)
            return;

        // ���� �ӵ�
        Time.timeScale = 0;
        
        playEnd = true;
        finalMessage.text = "CLEAR!!";

        float score = 12345f + limitTime * 123f + pc.hp * 123f;
        finalScoreLabel.text = string.Format("{0:N0}", score);

        finalGUI.SetActive(true);

        BestCheck(score);
        // ScoreSet(PlayerPrefs.GetString("UserName"), score);
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

        BestCheck(score);
        // ScoreSet(PlayerPrefs.GetString("UserName"),score);
    }
    private void UpdateTimeLabel()
    {
        timeLabel.text = string.Format("Time : {0:N2}", limitTime);
    }

    public void EnemyDie()
    {
        // �������� 2. �� ���Ž� 5�� ����
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

    static public User[] GetUsers()
    {
        // ���� ������ ��ũ�� ����
        User[] users = new User[rankUserCnt + 1];

        for (int i = 0; i < rankUserCnt; i++)
        {
            string bestName = PlayerPrefs.GetString("BestPlayer" + (i + 1));
            float bestScore = PlayerPrefs.GetFloat("BestScore" + (i + 1));

            users[i] = new User(bestName, bestScore);
        }
        return users;
    }

    // ����� Ǯ��
    private void BestCheck(float score)
    {
        // ���� ������ ��ũ�� ����
        User[] users = GetUsers();

        string userName = PlayerPrefs.GetString("UserName");
        users[rankUserCnt] = new User(userName, score);

        // Ȯ�� �޼ҵ�� ������������ ����
        users = users.OrderByDescending(x => x.score).ToArray();

        for (int i = 0; i < rankUserCnt; i++)
        {
            PlayerPrefs.SetString("BestPlayer" + (i + 1), users[i].name);
            PlayerPrefs.SetFloat("BestScore" + (i + 1), users[i].score);
        }
        PlayerPrefs.Save();
    }

    public void ScoreSet(string currentName, float currentScore)
    {

        float tmpScore = 0f;
        string tmpName = "";

        for (int i = 0; i < rankUserCnt; i++)
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
        for (int i = 0; i < rankUserCnt; i++)
        {
            PlayerPrefs.SetString(i + "BestName", bestName[i]);
            PlayerPrefs.SetFloat(i + "BestScore", bestScore[i]);
        }
    }
}
