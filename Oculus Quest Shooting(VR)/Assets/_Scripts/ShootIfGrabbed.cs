using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootIfGrabbed : MonoBehaviour
{
    private SimpleShoot simpleShoot;
    private OVRGrabbable ovrGrabbable;

    // PrimaryIndexTrigger �� ����
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
        // ���� ����ִ��� Ȯ��
        if (ovrGrabbable.isGrabbed &&
            // Ű�� �����¼��� ���� �� ������ ������
            OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController()))
        {
            if (remainBullets > 0)
            {
                /*  ���Ʒ� ���� ���ϴ� ������� ����
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
