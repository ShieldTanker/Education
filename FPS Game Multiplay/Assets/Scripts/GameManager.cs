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
    // 싱글톤 변수
    public static GameManager gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
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

    // 현재의 게임 상태 변수
    public GameState gStateLocal;
    // [Networked] : 같은 세션에서 고유할수 있게 만듬(단 사용하려면 속성으로 사용해야함)
    [Networked] public GameState gState { get; set; }

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;

    // 게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;

    // PlayerMove 클래스 변수
    public PlayerMove player;

    // 옵션 화면 UI 오브젝트 변수
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

    // 플레이어 로테이트 변수
    public PlayerRotate pr;

    //마지막 접속 시간
    public Text lastDateTxt;
    string lastDate;
    PlayerData pData;
    string userID;

    // Start() 를 Spawned() 로 바꿈
    public override void Spawned()
    {
        pData = FindObjectOfType<PlayerData>();
        userID = pData.UserId;
        StartCoroutine(ShowLastDate(userID));

        // 초기 게임 상태를 준비 상태로 설정
        gState = GameState.Ready;

        // 게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져옴
        gameText = gameLabel.GetComponent<Text>();

        // 상태 텍스트의 내용을 'Ready...'로 설정
        gameText.text = "Ready...";

        // 상태 텍스트의 색상을 주황색으로 설정
        gameText.color = new Color32(255, 185, 0, 255);

        // 게임 준비 -> 게임 중 상태로 전환
        StartCoroutine(ReadyToStart());
    }

    // 수신하고 처리 하는 프레임에 처리 됨
    public override void FixedUpdateNetwork()
    {
        gStateLocal = gState;
        // 만일, 플레이어가 null 이 아니고 hp가 0이하라면
        if (player != null && player.hp <= 0)
        {
            // 플레이어의 애니메이션을 멈춤
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

            // 상태 텍스트를 활성화
            gameLabel.SetActive(true);

            // 상태 텍스트의 내용을 'GameOver'로 설정
            gameText.text = "Game Over";

            // 상태 텍스트의 색상을 붉은색으로 설정
            gameText.color = new Color32(255, 0, 0, 255);

            // 상태 텍스트의 자식 오브젝트의 트랜스폼 컴포넌트를 가져옴
            Transform buttons = gameText.transform.GetChild(0);

            // 버튼 오브젝트를 활성화
            buttons.gameObject.SetActive(true);

            // 상태를 '게임 오버' 상태로 변경
            gState = GameState.GameOver;
        }
    }

    IEnumerator ReadyToStart()
    {
        // 2초간 대기
        yield return new WaitForSeconds(2f);

        // 상태 텍스트의 내용을 'Go!'로 변경
        gameText.text = "Go!";

        // 0.5초간 대기
        yield return new WaitForSeconds(0.5f);

        // 상태 텍스트를 비활성화
        gameLabel.SetActive(false);

        // 상태를 '게임 중' 상태로 변경
        gState = GameState.Run;
    }

    // 옵션 화면 켜기
    public void OpenOptionWindow()
    {
        // 옵션 창을 활성화
        gameOption.SetActive(true);
        // 게임 속도를 0배속으로 전환
        Time.timeScale = 0f;
        // 게임 상태를 일시 정지 상태로 변경
        gState = GameState.Pause;
    }

    // 계속하기 옵션
    public void CloseOptionWindow()
    {
        // 옵션 창을 비활성화
        gameOption.SetActive(false);
        // 게임 속도를 1배속으로 전환
        Time.timeScale = 1f;
        // 게임 상태를 게임 중 상태로 변경
        gState = GameState.Run;
    }

    // 다시하기 옵션
    public void RestartGame()
    {
        // 게임 속도를 1배속으로 전환
        Time.timeScale = 1f;
        // 현재 씬 번호를 다시 로드
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // 로딩 화면 씬을 로드
        SceneManager.LoadScene(1);
    }

    // 게임 종료 옵션
    public void QuitGame()
    {
        // 애플리케이션을 종료
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
                lastDateTxt.text = "마지막 접속 시간 : " + lastDate;
            }
            else
            {
                Debug.Log("에러");
            }
        }
    }
}
