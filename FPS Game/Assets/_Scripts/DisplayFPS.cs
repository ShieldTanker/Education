using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFPS : MonoBehaviour
{
    [SerializeField] Text text;

    float frames = 0f;
    float timeElap = 0f;
    float frameTime = 0f;

    private void Update()
    {
        // 프레임마다 1올림
        frames++;
        // TimeScale 값 무시한 프레임 시간 더하기
        timeElap += Time.unscaledDeltaTime; // frame time 더하기

        // 약 1초가 흘렀을때
        if (timeElap >= 1f)
        {
            // 프레임타임 = 약1초 / 프레임
            frameTime = timeElap / frames;

            UpdateText();

            timeElap = 0;
            frames = 0f;
        }
    }
    void UpdateText()
    {
        text.text = string.Format("FPS : {0}, FrameTime : {1:F2} ms", frames, frameTime * 1000);
    }
}
