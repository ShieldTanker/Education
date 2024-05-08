using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public InputField nameInput;
    public GameObject bestData;
    public Text bestUserData;

    public void GoPlay()
    {
        PlayerPrefs.SetString("UserName", nameInput.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainPlay");
    }

    public void BestScore()
    {
        // 플레이어 프리팹 에 키값이 있을때만 메소드 실행
        if (!PlayerPrefs.HasKey("BestPlayer"))
            return;

        bestUserData.text = string.Format(
           "{0} : {1:N0}",
           PlayerPrefs.GetString("BestPlayer"),
           PlayerPrefs.GetFloat("BestScore"));

        bestData.SetActive(true);
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
