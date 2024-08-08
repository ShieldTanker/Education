using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ARFaceManager faceManager;
    public Material[] faceMats;

    public Text indexText;

    int vertNum = 0;
    int vertCount = 468;

    private void Start()
    {
        // ������ �ε��� ���� 0���� �ʱ�ȭ
        indexText.text = vertNum.ToString();
    }

    public void indexIncrease()
    {
        // vertNum�� ���� 1 ������Ű�� �ִ� �ε����� ��ġ �ʵ��� ��
        // Mathf.Min : �� �� �� ���� ���� ��ȯ
        vertNum = Mathf.Min(++vertNum, vertCount - 1);
        indexText.text = vertNum.ToString();
    }

    public void IndexDecrease()
    {
        // vertNum�� ���� 1 ���� ��Ű�� 0 ���� �۾����� �ʵ��� ��
        // Mathf.Max : �� �� �� ū ���� ��ȯ
        vertNum = Mathf.Max(--vertNum, 0);
        indexText.text = vertNum.ToString();
    }

    // ��ư�� ������ �� ����� �Լ�
    public void ToggleMaskImage()
    {
        // ���� �ϳ��� �ƴ� �������� �� �ֱ⿡ foreach�� ���
        // faceManager ������Ʈ���� ���� ������ face ������Ʈ�� ��� ��ȸ
        foreach (ARFace face in faceManager.trackables)
        {
            // ���� face ������Ʈ�� ���� �ν��ϰ� �ִ� ���¶��
            if (face.trackingState == TrackingState.Tracking)
            {
                // face ������Ʈ �� Ȱ��ȭ ���¸� �ݴ�� ����
                // activeSelf : �ڱ� �ڽ��� Ȱ��ȭ ����(�θ� ������Ʈ�� ����)
                // activeInHierarchy : �θ� ������Ʈ�� ��Ȱ��ȭ �Ǹ� ���� ��Ȱ��
                face.gameObject.SetActive(!face.gameObject.activeSelf);
            }
        }
    }
    // ���׸��� ���� ��ư �Լ�
    public void SwitchFaceMaterial(int num)
    {
        // FaceManager ������Ʈ���� ���� ������ face ������Ʈ�� ��� ��ȸ
        foreach (ARFace face in faceManager.trackables)
        {
            // ���� face ������Ʈ�� �����ν��ϰ� �ִ� ���¶��
            if (face.trackingState == TrackingState.Tracking)
            {
                // Ž���� face ������Ʈ�� MeshRenderer ������Ʈ�� ����
                MeshRenderer mr = face.GetComponent<MeshRenderer>();

                // ��ư�� ������ ��ȣ (�̹���:0��, ����:1��)�� �ش��ϴ� ���׸���� ����
                mr.material = faceMats[num];
            }
        }
    }
}
