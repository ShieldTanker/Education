using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = FindObjectOfType<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Vector3 targetPosition = hitInfo.point;
                transform.position = targetPosition;

                navAgent.destination = targetPosition;
            }
        }
    }
}
