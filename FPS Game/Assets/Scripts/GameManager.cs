using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // �̱��� ����
    public static GameManager gm;

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
    }

    private void Update()
    {
        // ����, �÷��̾��� hp �� 0 ���϶��
        if (player.hp <= 0)
        {
            // ���� �ؽ�Ʈ�� Ȱ��ȭ
            gameLabel.SetActive(true);

            //���� �ؽ�Ʈ�� ������ 'GameOver' �� ����
            gameText.text = "GameOver";

            // ���� �ؽ�Ʈ�� ������ �������� ����
            gameText.color = new Color32(255, 0, 0, 255);

            // ���¸� '���� ����' ���·� ����
            gState = GameState.GameOver;
        }
    }
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
}
