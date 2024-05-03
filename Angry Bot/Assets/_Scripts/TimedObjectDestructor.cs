using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectDestructor : MonoBehaviour
{
    float timeOut = 1f;

    private void Start()
    {
        // Destroy(gameObject, 1f); ���� �ڵ�
        Invoke("DestroyNow", timeOut);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }
}
