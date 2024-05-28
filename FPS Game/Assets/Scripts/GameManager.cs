using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글톤 변수
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

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
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
    }

    private void Update()
    {
        // 만일, 플레이어의 hp 가 0 이하라면
        if (player.hp <= 0)
        {
            // 상태 텍스트를 활성화
            gameLabel.SetActive(true);

            //상태 텍스트의 내용을 'GameOver' 로 설정
            gameText.text = "GameOver";

            // 상태 텍스트의 색상을 빨강으로 설정
            gameText.color = new Color32(255, 0, 0, 255);

            // 상태를 '게임 오버' 상태로 변경
            gState = GameState.GameOver;
        }
    }
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
}
