using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // 싱글톤 변수
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

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver,
    }

    // 현재 게임 상태 변수
    public GameState gState;

    // 게임상태 UI 오브젝트 변수
    public GameObject gameLabel;
    // 게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;

    // PlayerMove 클래스 변수(player 라고 하면 보통 player 오브젝트를 뜻 하기에 다른이름이 좋다)
    PlayerMove player;

    // 옵션 화면 UI 오브젝트 변수
    public GameObject gameOption;

    public Text killCountTxt;

    int enemyKillCnt;

    int killCount;
    public int KillCount { get { return killCount; } set { killCount = value; } }


    private void Start()
    {
        // 초기 게임 상태를 준비 상태로 설정
        gState = GameState.Ready;

        // 게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져옴
        gameText = gameLabel.GetComponent<Text>();

        // 상태 텍스트의 내용을 'Ready...' 로 설정
        gameText.text = "Ready...";

        // 상태 텍스트의 색상을 주황색으로 설정
        gameText.color = new Color32(255, 185, 0, 255);

        // 게임 준비 -> 게임중으로 상태 전환
        StartCoroutine(ReadyToStart());

        // 플레이어 오브젝트를 찾은 후 플레이어의 PlayerMove 컴포넌트 받아오기
        player = GameObject.Find("Player").GetComponent<PlayerMove>();

        SetKillCount();
    }

    private void Update()
    {
        // 만일, 플레이어의 hp 가 0 이하라면
        if (player.hp <= 0)
        {
            // 플레이어의 애니메이션을 멈춤
            player.GetComponentInChildren<Animator>().SetFloat("moveMotion", 0f);

            // 상태 텍스트를 활성화
            gameLabel.SetActive(true);

            //상태 텍스트의 내용을 'GameOver' 로 설정
            gameText.text = "GameOver";

            // 상태 텍스트의 색상을 빨강으로 설정
            gameText.color = new Color32(255, 0, 0, 255);

            // 상태 텍스트의 자식 오브젝트의 트랜스폼 컴포넌트를 가져옴(첫번째 자식)
            Transform buttons = gameText.transform.GetChild(0);

            // 버튼 오브젝트를 활성화
            buttons.gameObject.SetActive(true);

            // 상태를 '게임 오버' 상태로 변경
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
        // 2초간 대기
        yield return new WaitForSeconds(2f);

        // 상태 텍스트의 내용을 "Go!" 로 변경
        gameText.text = "Go!";

        //0.5초 대기
        yield return new WaitForSeconds(0.5f);

        // 상태 텍스트 비활성화
        gameLabel.SetActive(false);

        // 상태를 '게임중' 으로 변경
        gState = GameState.Run;
    }

    // 옵션 화면 켜기
    public void OpenOptionWindow()
    {
        // 옵션창 활성화
        gameOption.SetActive(true);

        // 게임 속도를 0배속으로
        Time.timeScale = 0f;

        // 게임상태 를 Pause(일시정지) 로 변경
        gState = GameState.Pause;
    }

    // 계속하기 옵션
    public void CloseOptionWindow()
    {
        // 옵션창 을 비활성화
        gameOption.SetActive(false);

        // 게임 속도를 1배속으로 전환
        Time.timeScale = 1f;

        // 게임 상태를 게임중(Run) 으로 변경
        gState = GameState.Run;
    }

    // 재시작 옵션
    public void RestartGame()
    {
        // 게임 속도 를 1배속으로 전환
        Time.timeScale = 1f;

        // 현재 씬 번호를 다시 로드
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // 로딩 화면 씬을 로드
        SceneManager.LoadScene(1);
    }

    // 게임 종료 옵션
    public void QuitGame()
    {
        // 애플리케이션 종료
        Application.Quit();
    }
}