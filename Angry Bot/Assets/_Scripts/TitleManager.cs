using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public InputField nameInput;
    public GameObject[] bestData;
    public GameObject teacherBestData;
    public Text[] bestUserDatas;
    public Text bestUserData;

    private void Start()
    {
        // PlayerPrefs.DeleteAll();
    }

    public void GoPlay()
    {
        /* 연습문제 1. 이름입력 없을시 게임 시작 x
        if (nameInput.text == "")
            return; 
        if (string.IsNullOrEmpty(nameInput.text))
            return;  */
        if (nameInput.text != "")
        {
            PlayerPrefs.SetString("UserName", nameInput.text);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainPlay");
        }
    }

    // 강사님 풀이
    public void TeachersBestScore()
    {
        User[] users = PlayManager.GetUsers();
        
        bestUserData.text = "";

        for (int i = 0; i < PlayManager.rankUserCnt; i++)
        {
            if (users[i].name == "")
                break;

            bestUserData.text += string.Format(
                "{0} : {1:N0}\n", users[i].name, users[i].score);
        }

        teacherBestData.SetActive(true);
    }

    //내가 한거
    public void BestScore()
    {
        // 플레이어 프리팹 에 키값이 있을때만 메소드 실행
        if (!PlayerPrefs.HasKey("0BestName"))
            return;

        for (int i = 0; i < 3; i++)
        {
            bestUserDatas[i].text = string.Format(
               "{0} : {1:N0}\n",
               PlayerPrefs.GetString(i + "BestName"),
               PlayerPrefs.GetFloat(i + "BestScore"));
            
            bestData[i].SetActive(true);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
