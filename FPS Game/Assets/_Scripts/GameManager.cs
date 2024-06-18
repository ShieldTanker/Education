using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // �̱��� ����
    static GameManager gm;
    public static GameManager GM { get { return gm; } set { gm = value; } }

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���� ���� ���
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver,
    }

    // ���� ���� ���� ����
    public GameState gState;

    // ���ӻ��� UI ������Ʈ ����
    public GameObject gameLabel;
    // ���� ���� UI �ؽ�Ʈ ������Ʈ ����
    Text gameText;

    // PlayerMove Ŭ���� ����(player ��� �ϸ� ���� player ������Ʈ�� �� �ϱ⿡ �ٸ��̸��� ����)
    PlayerMove player;

    // �ɼ� ȭ�� UI ������Ʈ ����
    public GameObject gameOption;

    public Text killCountTxt;

    int enemyKillCnt;

    int killCount;
    public int KillCount { get { return killCount; } set { killCount = value; } }


    private void Start()
    {
        // �ʱ� ���� ���¸� �غ� ���·� ����
        gState = GameState.Ready;

        // ���� ���� UI ������Ʈ���� Text ������Ʈ�� ������
        gameText = gameLabel.GetComponent<Text>();

        // ���� �ؽ�Ʈ�� ������ 'Ready...' �� ����
        gameText.text = "Ready...";

        // ���� �ؽ�Ʈ�� ������ ��Ȳ������ ����
        gameText.color = new Color32(255, 185, 0, 255);

        // ���� �غ� -> ���������� ���� ��ȯ
        StartCoroutine(ReadyToStart());

        // �÷��̾� ������Ʈ�� ã�� �� �÷��̾��� PlayerMove ������Ʈ �޾ƿ���
        player = GameObject.Find("Player").GetComponent<PlayerMove>();

        SetKillCount();
    }

    private void Update()
    {
        // ����, �÷��̾��� hp �� 0 ���϶��
        if (player.hp <= 0)
        {
            // �÷��̾��� �ִϸ��̼��� ����
            player.GetComponentInChildren<Animator>().SetFloat("moveMotion", 0f);

            // ���� �ؽ�Ʈ�� Ȱ��ȭ
            gameLabel.SetActive(true);

            //���� �ؽ�Ʈ�� ������ 'GameOver' �� ����
            gameText.text = "GameOver";

            // ���� �ؽ�Ʈ�� ������ �������� ����
            gameText.color = new Color32(255, 0, 0, 255);

            // ���� �ؽ�Ʈ�� �ڽ� ������Ʈ�� Ʈ������ ������Ʈ�� ������(ù��° �ڽ�)
            Transform buttons = gameText.transform.GetChild(0);

            // ��ư ������Ʈ�� Ȱ��ȭ
            buttons.gameObject.SetActive(true);

            // ���¸� '���� ����' ���·� ����
            gState = GameState.GameOver;
        }
    }

    public void SetKillCount()
    {
        killCountTxt.text = "Kill Count : " + killCount;
    }


/*
    public void EnemyKill()
    {
        enemyKillCnt++;
        killCountTxt.text = "Kill Count : " + enemyKillCnt;
    }
*/


    IEnumerator ReadyToStart()
    {
        // 2�ʰ� ���
        yield return new WaitForSeconds(2f);

        // ���� �ؽ�Ʈ�� ������ "Go!" �� ����
        gameText.text = "Go!";

        //0.5�� ���
        yield return new WaitForSeconds(0.5f);

        // ���� �ؽ�Ʈ ��Ȱ��ȭ
        gameLabel.SetActive(false);

        // ���¸� '������' ���� ����
        gState = GameState.Run;
    }

    // �ɼ� ȭ�� �ѱ�
    public void OpenOptionWindow()
    {
        // �ɼ�â Ȱ��ȭ
        gameOption.SetActive(true);

        // ���� �ӵ��� 0�������
        Time.timeScale = 0f;

        // ���ӻ��� �� Pause(�Ͻ�����) �� ����
        gState = GameState.Pause;
    }

    // ����ϱ� �ɼ�
    public void CloseOptionWindow()
    {
        // �ɼ�â �� ��Ȱ��ȭ
        gameOption.SetActive(false);

        // ���� �ӵ��� 1������� ��ȯ
        Time.timeScale = 1f;

        // ���� ���¸� ������(Run) ���� ����
        gState = GameState.Run;
    }

    // ����� �ɼ�
    public void RestartGame()
    {
        // ���� �ӵ� �� 1������� ��ȯ
        Time.timeScale = 1f;

        // ���� �� ��ȣ�� �ٽ� �ε�
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // �ε� ȭ�� ���� �ε�
        SceneManager.LoadScene(1);
    }

    // ���� ���� �ɼ�
    public void QuitGame()
    {
        // ���ø����̼� ����
        Application.Quit();
    }
}