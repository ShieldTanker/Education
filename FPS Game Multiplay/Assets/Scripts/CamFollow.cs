using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // ��ǥ�� �� Ʈ������ ������Ʈ
    public Transform target;

    private void Update()
    {
        // ī�޶��� ��ġ�� ��ǥ Ʈ�������� ��ġ�� ��ġ��Ŵ
        transform.position = target.position;
    }
}
