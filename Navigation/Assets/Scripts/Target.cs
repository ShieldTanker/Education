using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �׺���̼� ��� ��� ���� �ֱ�
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    NavMeshAgent[] navAgents;

    private void Start()
    {
        // �����ϴ� ������Ʈ�� �� Ư�� Ÿ��(Ŭ����) �� ���� ��� ������Ʈ �� ã��
        // as : �ش� Ÿ������ ��ȯ
        navAgents = FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];
        Debug.Log("Number of agent = " + navAgents.Length);
    }

    private void Update()
    {
        UpdateTargets(transform.position);
        /*        if (Input.GetMouseButton(0))
                {
                    // ���� ī�޶󿡼� Ŭ���� ��ġ������ Ray�� ��
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // Ray �� ���� ������Ʈ�� ������ ���� ����
                    RaycastHit hitInfo;

                    // Ray �� ���� ���� ������ hitInfo �� ����
                    // Physics.Raycast(ray, out hitInfo) �� �ᵵ ��
                    if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                    {
                        // targetPosition = ray �� ���� ������Ʈ�� ���Ͱ�
                        Vector3 targetPosition = hitInfo.point;

                        // �������� targetPosition ����
                        UpdateTargets(targetPosition);

                        // ������Ʈ�� ��ġ�� targetPosition ��ġ�� ����
                        transform.position = targetPosition;
                    }
                }*/
    }

    void UpdateTargets(Vector3 targetPosition)
    {
        foreach (NavMeshAgent agent in navAgents)
        {
            // destination : ������ ����
            agent.destination = targetPosition;
        }
    }
}
