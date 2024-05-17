using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �θ�Ŭ���� ���
public class Touch : Sense
{
    private void OnTriggerEnter(Collider other)
    {
        // ���� ������Ʈ�� Aspect ������Ʈ ��������
        Aspect aspect = other.GetComponent<Aspect>();

        // ������� ������
        if (aspect != null)
        {
            // ��ǥ �̸��� ������ Ȯ��
            if (aspect.aspectName == aspectName)
            {
                Debug.Log("Enemy Touch Detected");
            }
        }
    }
}
