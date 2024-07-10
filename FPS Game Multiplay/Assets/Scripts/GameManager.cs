using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;

public class GameManager : NetworkBehaviour
{
    // �̱��� ����
    public static GameManager gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
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

    // ������ ���� ���� ����
    public GameState gStateLocal;
    // [Networked] : ���� ���ǿ��� �����Ҽ� �ְ� ����(�� ����Ϸ��� �Ӽ����� ����ؾ���)
    [Networked] public GameState gState { get; set; }

    // ���� ���� UI ������Ʈ ����
    public GameObject gameLabel;

    // ���� ���� UI �ؽ�Ʈ ������Ʈ ����
    Text gameText;

    // PlayerMove Ŭ���� ����
    public PlayerMove player;

    // �ɼ� ȭ�� UI ������Ʈ ����
    public GameObject gameOption;

    public Slider hpSlider;
    public GameObject hitEffect;

    public Text wModeText;

    public GameObject weapon01;
    public GameObject weapon02;
    public GameObject crosshair01;
    public GameObject crosshair02;
    public GameObject weapon01_R;
    public GameObject weapon02_R;
    public GameObject crosshair02_zoom;

    public List<GameObject> players;

    // �÷��̾� ������Ʈ ����
    public PlayerRotate pr;

    //������ ���� �ð�
    public Text lastDateTxt;
    string lastDate;
    PlayerData pData;
    string userID;

    // Start() �� Spawned() �� �ٲ�
    public override void Spawned()
    {
        pData = FindObjectOfType<PlayerData>();
        userID = pData.UserId;
        StartCoroutine(ShowLastDate(userID));

        // �ʱ� ���� ���¸� �غ� ���·� ����
        gState = GameState.Ready;

        // ���� ���� UI ������Ʈ���� Text ������Ʈ�� ������
        gameText = gameLabel.GetComponent<Text>();

        // ���� �ؽ�Ʈ�� ������ 'Ready...'�� ����
        gameText.text = "Ready...";

        // ���� �ؽ�Ʈ�� ������ ��Ȳ������ ����
        gameText.color = new Color32(255, 185, 0, 255);

        // ���� �غ� -> ���� �� ���·� ��ȯ
        StartCoroutine(ReadyToStart());
    }

    // �����ϰ� ó�� �ϴ� �����ӿ� ó�� ��
    public override void FixedUpdateNetwork()
    {
        gStateLocal = gState;
        // ����, �÷��̾ null �� �ƴϰ� hp�� 0���϶��
        if (player != null && player.hp <= 0)
        {
            // �÷��̾��� �ִϸ��̼��� ����
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

            // ���� �ؽ�Ʈ�� Ȱ��ȭ
            gameLabel.SetActive(true);

            // ���� �ؽ�Ʈ�� ������ 'GameOver'�� ����
            gameText.text = "Game Over";

            // ���� �ؽ�Ʈ�� ������ ���������� ����
            gameText.color = new Color32(255, 0, 0, 255);

            // ���� �ؽ�Ʈ�� �ڽ� ������Ʈ�� Ʈ������ ������Ʈ�� ������
            Transform buttons = gameText.transform.GetChild(0);

            // ��ư ������Ʈ�� Ȱ��ȭ
            buttons.gameObject.SetActive(true);

            // ���¸� '���� ����' ���·� ����
            gState = GameState.GameOver;
        }
    }

    IEnumerator ReadyToStart()
    {
        // 2�ʰ� ���
        yield return new WaitForSeconds(2f);

        // ���� �ؽ�Ʈ�� ������ 'Go!'�� ����
        gameText.text = "Go!";

        // 0.5�ʰ� ���
        yield return new WaitForSeconds(0.5f);

        // ���� �ؽ�Ʈ�� ��Ȱ��ȭ
        gameLabel.SetActive(false);

        // ���¸� '���� ��' ���·� ����
        gState = GameState.Run;
    }

    // �ɼ� ȭ�� �ѱ�
    public void OpenOptionWindow()
    {
        // �ɼ� â�� Ȱ��ȭ
        gameOption.SetActive(true);
        // ���� �ӵ��� 0������� ��ȯ
        Time.timeScale = 0f;
        // ���� ���¸� �Ͻ� ���� ���·� ����
        gState = GameState.Pause;
    }

    // ����ϱ� �ɼ�
    public void CloseOptionWindow()
    {
        // �ɼ� â�� ��Ȱ��ȭ
        gameOption.SetActive(false);
        // ���� �ӵ��� 1������� ��ȯ
        Time.timeScale = 1f;
        // ���� ���¸� ���� �� ���·� ����
        gState = GameState.Run;
    }

    // �ٽ��ϱ� �ɼ�
    public void RestartGame()
    {
        // ���� �ӵ��� 1������� ��ȯ
        Time.timeScale = 1f;
        // ���� �� ��ȣ�� �ٽ� �ε�
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // �ε� ȭ�� ���� �ε�
        SceneManager.LoadScene(1);
    }

    // ���� ���� �ɼ�
    public void QuitGame()
    {
        // ���ø����̼��� ����
        Application.Quit();
    }

    public void AddPlayer(GameObject obj)
    {
        players.Add(obj);
    }

    public void RemovePlayer(GameObject obj)
    {
        players.Remove(obj);
    }

    IEnumerator ShowLastDate(string id)
    {
        string url = "http://localhost/fps_game/lastdate.php";
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", id);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                lastDate = www.downloadHandler.text;
                lastDateTxt.text = "������ ���� �ð� : " + lastDate;
            }
            else
            {
                Debug.Log("����");
            }
        }
    }
}
