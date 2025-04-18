using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // 유저 아이디 변수
    public InputField id;

    // 유저 패스워드 변수
    public InputField password;

    // 검사 텍스트 변수
    public Text notify;

    private void Start()
    {
        // 검사 텍스트 창을 비움
        notify.text = "";
    }

    // 아이디 와 패스워드 저장 함수
    public void SaveUserData()
    {
        // 만일 입력 검사에 문제가 있으면 함수를 종료
        if (!CheckInput(id.text,password.text))
            return;

        // 만일 시스템에 저장되어 있는 아이디가 존재하지 않는다면
        if (!PlayerPrefs.HasKey(id.text))
        {
            // 사용자의 아이디는 키(key)로 패스워드를 값(value)으로 설정
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "아이디 생성이 완료 되었습니다";
        }
        // 그렇지 않으면
        else
        {
            notify.text = "이미 존재하는 아이디 입니다.";
        }
    }

    // 로그인 함수
    public void CheckUserData()
    {
        // 만일 입력 검사에 문제가 있으면 함수를 종료
        if (!CheckInput(id.text, password.text))
            return;

        // 사용자가 입력한 아이디를 키(key)로 사용해 시스템에 저장된 값을 불러옴
        string pass = PlayerPrefs.GetString(id.text);

        // 만일, 사용자가 입력한 패스워드와 시스템에서 불러온 값을 비교해서 동일하다면
        if (password.text == pass)
        {
            // 다음 씬(1번 씬)을 로드
            SceneManager.LoadScene(1);
        }
        // 그렇지 않고 두 데이터의 값이 다르면, 유저 정보 불일치 메시지를 남김
        else
        {
            notify.text = "입력하신 아이디 또는 비밀번호 가 일치하지 않습니다.";
        }
    }

    bool CheckInput(string id, string pwd)
    {
        // 만일, 입력란이 하나라도 비어 있으면 유저 정보 입력을 요구
        if (id == "" || pwd == "")
        {
            notify.text = "아이디 또는 비밀번호를 입력해주세요.";
            return false;
        }
        // 입력이 비어있지 않으면
        else
            return true;
    }
}