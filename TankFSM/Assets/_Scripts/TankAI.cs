using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : MonoBehaviour
{
    // 목적지
    Transform pointA;
    Transform pointB;

    // 자기 자신 오브젝트 넣기
    NavMeshAgent navMeshAgent;

    // 플레이어 감지
    GameObject player;
    // 애니메이터 변환으로 FSM 전환
    Animator animator;

    Ray ray;
    RaycastHit hit;

    // 최대 감지 거리
    float maxDistanceToCheck = 10f;
    // 현재 거리
    float currentDistance;
    //목표와의 거리
    float distanceFromTarget;

    // 방향 체크
    Vector3 checkDirection;

    // 0이면 0번 인덱스로 1이면 1번 인덱스로 목적지
    int currentTarget;

    // 목적지 배열
    Transform[] waypoints;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        pointA = GameObject.Find("P1").transform;
        pointB = GameObject.Find("P2").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        // 초기값 으로 A,B 포인트 넣기
        waypoints = new Transform[2] { pointA, pointB };
        
        // 처음 이동은 0번 인덱스 인 pointA 로 이동
        currentTarget = 0;
        
        // 목적지를 currentTarget 의 번호로 이동
        navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }

    // 굳이 FixedUpdate 할 필요는 없음
    private void FixedUpdate()
    {
        // 플레이어 와 현재 오브젝트 와 의 거리 구하기
        currentDistance = Vector3.Distance(player.transform.position, transform.position);
        
        // float 타입의 파라매터에 값을 전달
        animator.SetFloat("distanceFromPlayer", currentDistance);
        
        // 플레이어 오브젝트로 향하는 방향 구하기
        checkDirection = player.transform.position - transform.position;
        
        // 현재 위치에서 플레이어 방향으로 ray 쏘기
        ray = new Ray(transform.position, checkDirection);

        Vector3 drawDirection = maxDistanceToCheck * checkDirection.normalized;

        // DrawLine 은 위치와 위치끼리 선을 그리기
        // DrawRay 는 지정위치 에서 지정벡터 방향으로 선 그리기
        Debug.DrawRay(transform.position, drawDirection, Color.red);

        // ray 가 최대 거리 안에 있으면 hit 에 결과값 전달
        if (Physics.Raycast(ray, out hit, maxDistanceToCheck))
        {
            // 객체 끼리 서로 비교(같은 객체를 참조하는지 비교함)
            // 값이 같은지 비교하면 메모리 소모가 엄청나기 때문에 참조방식으로 비교함
            if (hit.collider.gameObject == player)
                animator.SetBool("isPlayerVisible", true);
            else
                animator.SetBool("isPlayerVisible", false);
        }
        else
            animator.SetBool("isPlayerVisible", false);


        // 목적지 까지 거리
        distanceFromTarget = Vector3.Distance(waypoints[currentTarget].position, transform.position);

        // distanceFromWaypoint 에 현재 목적지 까지 의 거리를 전달
        animator.SetFloat("distanceFromWaypoint", distanceFromTarget);
    }

    // 애니메이터뷰에서 FindNewTarget 상태일때 호출
    public void SetNextPoint()
    {
        switch (currentTarget)
        {
            case 0: //목적지 번호가 0
                currentTarget = 1;
                break;
            
            case 1: // 목적지 번호가 1
                currentTarget = 0;
                break;
        }
        navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }

    public void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
        
        if (!animator.GetBool("isPlayerVisible"))
            navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }
}
