using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostItem : MonoBehaviour
{
    int idx = 3;

    public float ghostTime;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMove pm = GameObject.Find("Player").GetComponent<PlayerMove>();
        pm.ghostTime = ghostTime;

        EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        gameObject.SetActive(false);
        em.enemyPool[idx].Add(gameObject);
    }
}
