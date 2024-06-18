using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    // ������ �� ��ȣ
    public int sceneNumber = 2;

    // �ε� �����̴� ��
    public Slider loadingBar;

    // �ε� ���� �ؽ�Ʈ
    public Text loadingText;

    private void Start()
    {
        // �񵿱� �� �ε� �ڷ�ƾ�� ����
        StartCoroutine(TransitionNextScene(sceneNumber));
    }

    // �񵿱� �� �ε� �ڷ�ƾ
    IEnumerator TransitionNextScene(int num)
    {
        // ������ ���� �񵿱� �������� �ε�
        AsyncOperation ao = SceneManager.LoadSceneAsync(num);

        // �ε�Ǵ� ���� ����� ȭ�鿡 ������ �ʰ� ��
        ao.allowSceneActivation = false;

        // �ε��� �Ϸ�� ������ �ݺ��ؼ� ���� ��ҵ��� �ε��ϰ� ���� ������ ȭ�鿡 ǥ��
        while (!ao.isDone)
        {
            // �ε� ������� �����̴� �ٿ� �ؽ�Ʈ�� ǥ��
            loadingBar.value = ao.progress;
            loadingText.text = (ao.progress * 100f).ToString() + "%";

            // ����, �� �ε� ������� 90%�� �Ѿ��
            if (ao.progress >= 0.9f)
            {
                break;
            }

            // ���� �������� �� ������ ��ٸ�
            yield return null;
        }

        loadingBar.value = 1f;
        loadingText.text = "100%";

        yield return new WaitForSeconds(1f);

        // �ε�� ���� ȭ�鿡 ���̰� ��
        ao.allowSceneActivation = true;
    }
}
