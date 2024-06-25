using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 네비게이션 기능 사용 위해 넣기
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    NavMeshAgent[] navAgents;

    private void Start()
    {
        // 존재하는 오브젝트들 중 특정 타입(클래스) 를 가진 모든 오브젝트 를 찾기
        // as : 해당 타입으로 변환
        navAgents = FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];
        Debug.Log("Number of agent = " + navAgents.Length);
    }

    private void Update()
    {
        UpdateTargets(transform.position);
        /*        if (Input.GetMouseButton(0))
                {
                    // 메인 카메라에서 클릭한 위치쪽으로 Ray를 쏨
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // Ray 에 맞은 오브젝트를 저장할 변수 선언
                    RaycastHit hitInfo;

                    // Ray 에 맞은 벡터 값들을 hitInfo 에 저장
                    // Physics.Raycast(ray, out hitInfo) 로 써도 됨
                    if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                    {
                        // targetPosition = ray 에 맞은 오브젝트의 벡터값
                        Vector3 targetPosition = hitInfo.point;

                        // 목적지에 targetPosition 설정
                        UpdateTargets(targetPosition);

                        // 오브젝트의 위치를 targetPosition 위치로 설정
                        transform.position = targetPosition;
                    }
                }*/
    }

    void UpdateTargets(Vector3 targetPosition)
    {
        foreach (NavMeshAgent agent in navAgents)
        {
            // destination : 목적지 설정
            agent.destination = targetPosition;
        }
    }
}
