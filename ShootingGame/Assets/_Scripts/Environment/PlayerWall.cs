using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWall : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.velocity = Vector3.zero;
        }
    }
}
