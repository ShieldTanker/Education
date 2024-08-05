using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootIfGrabbed : MonoBehaviour
{
    private SimpleShoot simpleShoot;
    private OVRGrabbable ovrGrabbable;

    // PrimaryIndexTrigger 로 설정
    public OVRInput.Button shootingButton;

    public int remainBullets;
    public TMP_Text remainBulletsText;

    public AudioSource audioSource;
    public AudioClip shootingAudio;

    private void Start()
    {
        simpleShoot = GetComponent<SimpleShoot>();
        ovrGrabbable = GetComponent<OVRGrabbable>();
        audioSource = GetComponent<AudioSource>();

        remainBulletsText.text = remainBullets.ToString();

    }

    private void Update()
    {
        // 총을 잡고있는지 확인
        if (ovrGrabbable.isGrabbed &&
            // 키를 누르는순간 잡은 손 정보를 가져옴
            OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController()))
        {
            if (remainBullets > 0)
            {
                /*  위아래 둘중 원하는 방식으로 선택
                VibrationManager.singleton.TriggerVibration(shootingAudio, ovrGrabbable.grabbedBy.GetController());
                */

                VibrationManager.singleton.TriggerVibration(40, 2, 255, ovrGrabbable.grabbedBy.GetController());

                audioSource.PlayOneShot(shootingAudio);
                simpleShoot.TriggerShoot();

                --remainBullets;
                remainBulletsText.text = remainBullets.ToString();
            }
        }
    }
}
