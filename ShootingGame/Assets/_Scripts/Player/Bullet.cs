using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 이동속도
    public float bSpeed;

    public List<GameObject> myPool;

    private void Update()
    {
        // 1. 방향을 구한다.
        Vector3 dir = Vector3.up;

        //2.이동하고 싶다.
        transform.Translate(dir * bSpeed * Time.deltaTime);
    }
}
