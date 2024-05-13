using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public GameObject enemyFirePosition;
    public GameObject enemyBullet;

    public float shootLate;

    private float timeCount;

    private void Start()
    {
        timeCount = 0;
    }

    private void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount >= shootLate)
        {
            Instantiate(enemyBullet, transform.position, Quaternion.Euler(180, 0, 0));
            Debug.Log("shot");

            timeCount = 0;
        }
    }
}
