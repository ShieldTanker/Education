using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        // �ڱ� �ڽ��� ������ ī�޶��� ����� ��ġ��Ŵ
        transform.forward = Camera.main.transform.forward;
    }
}
