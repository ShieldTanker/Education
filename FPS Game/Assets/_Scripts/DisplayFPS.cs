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
        // �����Ӹ��� 1�ø�
        frames++;
        // TimeScale �� ������ ������ �ð� ���ϱ�
        timeElap += Time.unscaledDeltaTime; // frame time ���ϱ�

        // �� 1�ʰ� �귶����
        if (timeElap >= 1f)
        {
            // ������Ÿ�� = ��1�� / ������
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
