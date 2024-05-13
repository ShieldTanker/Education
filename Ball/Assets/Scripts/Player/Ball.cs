using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        //if (Input.GetKeyDown(KeyCode.Space)) 
        //{
            //rb.AddForce(Vector3.up * 300);
        //}

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(Vector3.left * 4);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Vector3.right * 4);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (collision.gameObject.name == "Ground" && Input.GetKeyDown(KeyCode.Space)) 
        {
            rb.AddForce(Vector3.up * 300);
        }
    }
}
