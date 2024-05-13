using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point10 : MonoBehaviour
{
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            gameManager.count += 1;
            gameObject.SetActive(false);
        }
    }
}
