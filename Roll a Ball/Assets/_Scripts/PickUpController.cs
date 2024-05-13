using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public int score;

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
            gameManager.score += score;

            gameManager.SetCountText();
            gameObject.SetActive(false);
        }
    }
}
