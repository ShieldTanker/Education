using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    //싱글톤 객체 (Awake 실행후 Instance 변수에 자기 자신을 할당)
    public static ScoreManager Instance = null;

    //Awake 는 Start 보다 빨리 실행됨
    private void Awake()
    {
        // 싱글톤 객체에 값이 없으면 생성된 자기 자신을 할당
        if (Instance == null)
            //자기 자신 객체를 의미
            Instance = this;
    }

    //속성
    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            //ScoreManager 클래스의 속성에 값을 할당 한다
            currentScore = value;

            //현재 화면에 점수 표시
            currentScoreUI.text = "현재 점수 : " + currentScore;

            //만약 현재 점수가 최고 점수를 초과하였다면
            if (currentScore > bestScore)
            {
                //최고점수 갱신
                bestScore = currentScore;

                //최고점수 화면 출력
                bestScoreUI.text = "최고 점수 : " + bestScore;

                //최고점수를 저장
                //                 (    Key     ,    Value   )
                PlayerPrefs.SetInt("Best Score", bestScore);
                //디스크에 저장(마지막에 모아서 한번에 하는게 좋음)
                //비정상 종료 됬을때 저장 되게 하려면 이방식 사용
                PlayerPrefs.Save();
            }
        }
    }

    //현재 점수 UI
    public Text currentScoreUI;

    //최고 점수UI
    public Text bestScoreUI;

    //최고 점수
    public int bestScore;



    //현재 점수(캡슐화)
    private int currentScore;

    private void Start()
    {
        //최고점수를 불러와서 bestScore 에 넣어주기
        //불러올때는 GetInt,GetFloat,GetString 으로 Key 불러오기
        // GetInt("Best Score", 0) 의 의미는 Best Score 의 값이 없을떄 기본값으로 0 출력
        bestScore = PlayerPrefs.GetInt("Best Score", 0);

        //최고점수 화면 표기
        bestScoreUI.text = "최고점수 : " + bestScore;
    }


///////////////////////////////////////////////////////////////////////////////////////////////////////

/*
     public class ScoreManager : MonoBehaviour
{

    // 싱글톤 객체
    public static ScoreManager Instance = null;

    // 싱글톤 객체에 값이 없으면 생성된 자기 자신을 할당
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    // 속성
    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            // ScoreManager 클래스의 속성에 값을 할당 한다.
            currentScore = value;

            // 화면에 현재 점수 표시하기
            currentScoreUI.text = "현재점수 : " + currentScore;

            // 만약 현재 점수가 최고 점수를 초과하였다면
            if (currentScore > bestScore)
            {
                // 최고 점수를 갱신 시킨다.
                bestScore = currentScore;

                // 최고 점수를 UI에 표시
                bestScoreUI.text = "최고점수 : " + bestScore;

                // 최고점수를 저장
                PlayerPrefs.SetInt("Best Score", bestScore);
                PlayerPrefs.Save();
            }
        }
    }

    // 현재 점수 UI
    public Text currentScoreUI;

    // 현재 점수
    private int currentScore;

    // 최고 점수 UI
    public Text bestScoreUI;

    // 최고 점수
    public int bestScore;

    private void Start()
    {
        // 최고점수를 불러와서 bestScore에 넣어주기
        bestScore = PlayerPrefs.GetInt("Best Score", 0);
        // 최고점수 화면에 표시하기
        bestScoreUI.text = "최고점수 : " + bestScore;
    }
}
*/

//////////////////////////////////////////////////////////////////////////////////////////////////////////

/*

        위에 속성 하기전에 이런방식 이었음

        currentScore에 값을 넣고 화면에 표기하기

       public void SetScore(int value)
    {
        //ScoreManager 클래스의 속성에 값을 할당 한다
        currentScore = value;

        //현재 화면에 점수 표시
        currentScoreUI.text = "현재 점수 : " + currentScore;

        //만약 현재 점수가 최고 점수를 초과하였다면
        if (currentScore > bestScore)
        {
            //최고점수 갱신
            bestScore = currentScore;

            //최고점수 화면 출력
            bestScoreUI.text = "최고 점수 : " + bestScore;

            //최고점수를 저장
            //                 (    Key     ,    Value   )
            PlayerPrefs.SetInt("Best Score", bestScore);
            //디스크에 저장(마지막에 모아서 한번에 하는게 좋음)
            //비정상 종료 됬을때 저장 되게 하려면 이방식 사용
            PlayerPrefs.Save();
        }

    }
*/

    //current 값 가져오기
/*
       public int GetScore()
    {
        return currentScore;
    }
*/
}
