using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator startButton;
    public Animator settingsBotton;
    public Animator settingDlg;
    public Animator contentPanel;
    public Animator gearImg;

    public AudioSource bgmVolumes;

    public Slider volumeSlider;
    public Slider volSdr;

    private void Start()
    {
        LoadVolume();
        // volumeSlider.value = PlayerPrefs.GetFloat("bgmSlider", 1f);
    }
    /*
    private void Update()
    {
        PlayerPrefs.SetFloat("bgmSlider", volumeSlider.value);
        PlayerPrefs.Save();
    }
    */

    public void ToggleMenu()
    {
        bool isHidden = contentPanel.GetBool("isHidden");

        //누를때 마다 참 거짓 이 반대로 입력되야함
        //메소드가 호출될때 현재 불리언 값의 반대값으로 변경
        contentPanel.SetBool("isHidden", !isHidden);
        gearImg.SetBool("isHidden", !isHidden);
    }

    public void OpenCloseSettings(bool isOpen)
    {
        startButton.SetBool("isHidden", isOpen);
        settingsBotton.SetBool("isHidden", isOpen);
        settingDlg.SetBool("isHidden", !isOpen);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
 
    public void SaveVolume()
    {
        float volume = volSdr.value;
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat("BGMVolume",1f);
        volSdr.value = volume;
    }
}

/*
   public class UIManager : MonoBehaviour
{
    public Animator startButton;
    public Animator settingsBotton;
    public Animator settingDlg;

    public void OpenSetting()
    {
        startButton.SetBool("isHidden", true);
        settingsBotton.SetBool("isHidden", true);
        settingDlg.SetBool("isHidden", false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void CloseSetting()
    {
        startButton.SetBool("isHidden", false);
        settingsBotton.SetBool("isHidden", false);
        settingDlg.SetBool("isHidden", true);
    }
}
*/
