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
    /// 진동으로 변환할 소리 와 컨트롤러 정보
    /// </summary>
    /// <param name="vibrationAudio"></param>
    /// <param name="controller"></param>
    public void TriggerVibration(AudioClip vibrationAudio, OVRInput.Controller controller)
    {
        // 진동을 만드는 함수에 쓰기위해 오디오 클립 변환
        OVRHapticsClip clip = new OVRHapticsClip(vibrationAudio);

        // 컨트롤러가 왼쪽 이면
        if (controller == OVRInput.Controller.LTouch)
            // 오디오 파일을 가지고 진동을 만듬
            OVRHaptics.LeftChannel.Preempt(clip);
        // 컨트롤러가 오른쪽 이면
        else
            OVRHaptics.RightChannel.Preempt(clip);
    }

    /// <summary>
    /// 반복할 횟수, 빈도, 세기
    /// </summary>
    /// <param name="iteration"></param>
    /// <param name="frequency"></param>
    /// <param name="strength"></param>
    /// <param name="controller"></param>
    public void TriggerVibration(int iteration, int frequency, int strength, OVRInput.Controller controller)
    {
        OVRHapticsClip clip = new OVRHapticsClip();

        for (int i = 0; i < iteration; i++)
            //  파형(소리, 진동) 을 그려냄
            // 빈도 로 나눈게 짝수 일때 세기만큼 파형 그리기
            clip.WriteSample(i % frequency == 0 ? (byte)strength : (byte)0);

        if (controller == OVRInput.Controller.LTouch)
            OVRHaptics.LeftChannel.Preempt(clip);
        else
            OVRHaptics.RightChannel.Preempt(clip);
    }
}