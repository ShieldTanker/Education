using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public InputField nameInput;
    public GameObject[] bestData;
    public Text[] bestUserData;

    private void Start()
    {
        // PlayerPrefs.DeleteAll();
    }
    public void GoPlay()
    {
        // �������� 1. �̸��Է� ������ ���� ���� x
        if (nameInput.text != "")
        {
            PlayerPrefs.SetString("UserName", nameInput.text);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainPlay");
        }
    }

    public void BestScore()
    {
        // �÷��̾� ������ �� Ű���� �������� �޼ҵ� ����
        if (!PlayerPrefs.HasKey("0BestName"))
            return;

        for (int i = 0; i < 3; i++)
        {
            bestUserData[i].text = string.Format(
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
