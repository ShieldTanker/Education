using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �̵��ӵ�
    public float bSpeed;

    public List<GameObject> myPool;

    private void Update()
    {
        // 1. ������ ���Ѵ�.
        Vector3 dir = Vector3.up;

        //2.�̵��ϰ� �ʹ�.
        transform.Translate(dir * bSpeed * Time.deltaTime);
    }
}
