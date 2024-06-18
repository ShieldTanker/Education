using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // ���� �÷��� �ϴµ��� ��� ����ؾ��� ����
    public string UserId { get; set; }

    private void Start()
    {
        // ��ŸƮ �� ������Ʈ�� �����ɶ� �����
        // �̹� ������ ������Ʈ�� �ٸ������� �ٽÿ͵� ��ŸƮ ���� ����
        int count = FindObjectsOfType<PlayerData>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}