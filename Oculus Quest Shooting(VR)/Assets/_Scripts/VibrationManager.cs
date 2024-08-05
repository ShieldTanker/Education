using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager singleton;

    private void Awake()
    {
        if (singleton && singleton != this)
            Destroy(this);
        else
            singleton = this;
    }

    /// <summary>
    /// �������� ��ȯ�� �Ҹ� �� ��Ʈ�ѷ� ����
    /// </summary>
    /// <param name="vibrationAudio"></param>
    /// <param name="controller"></param>
    public void TriggerVibration(AudioClip vibrationAudio, OVRInput.Controller controller)
    {
        // ������ ����� �Լ��� �������� ����� Ŭ�� ��ȯ
        OVRHapticsClip clip = new OVRHapticsClip(vibrationAudio);

        // ��Ʈ�ѷ��� ���� �̸�
        if (controller == OVRInput.Controller.LTouch)
            // ����� ������ ������ ������ ����
            OVRHaptics.LeftChannel.Preempt(clip);
        // ��Ʈ�ѷ��� ������ �̸�
        else
            OVRHaptics.RightChannel.Preempt(clip);
    }

    /// <summary>
    /// �ݺ��� Ƚ��, ��, ����
    /// </summary>
    /// <param name="iteration"></param>
    /// <param name="frequency"></param>
    /// <param name="strength"></param>
    /// <param name="controller"></param>
    public void TriggerVibration(int iteration, int frequency, int strength, OVRInput.Controller controller)
    {
        OVRHapticsClip clip = new OVRHapticsClip();

        for (int i = 0; i < iteration; i++)
            //  ����(�Ҹ�, ����) �� �׷���
            // �� �� ������ ¦�� �϶� ���⸸ŭ ���� �׸���
            clip.WriteSample(i % frequency == 0 ? (byte)strength : (byte)0);

        if (controller == OVRInput.Controller.LTouch)
            OVRHaptics.LeftChannel.Preempt(clip);
        else
            OVRHaptics.RightChannel.Preempt(clip);
    }
}