using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : MonoBehaviour
{
    // ������
    Transform pointA;
    Transform pointB;

    // �ڱ� �ڽ� ������Ʈ �ֱ�
    NavMeshAgent navMeshAgent;

    // �÷��̾� ����
    GameObject player;
    // �ִϸ����� ��ȯ���� FSM ��ȯ
    Animator animator;

    Ray ray;
    RaycastHit hit;

    // �ִ� ���� �Ÿ�
    float maxDistanceToCheck = 10f;
    // ���� �Ÿ�
    float currentDistance;
    //��ǥ���� �Ÿ�
    float distanceFromTarget;

    // ���� üũ
    Vector3 checkDirection;

    // 0�̸� 0�� �ε����� 1�̸� 1�� �ε����� ������
    int currentTarget;

    // ������ �迭
    Transform[] waypoints;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        pointA = GameObject.Find("P1").transform;
        pointB = GameObject.Find("P2").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        // �ʱⰪ ���� A,B ����Ʈ �ֱ�
        waypoints = new Transform[2] { pointA, pointB };
        
        // ó�� �̵��� 0�� �ε��� �� pointA �� �̵�
        currentTarget = 0;
        
        // �������� currentTarget �� ��ȣ�� �̵�
        navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }

    // ���� FixedUpdate �� �ʿ�� ����
    private void FixedUpdate()
    {
        // �÷��̾� �� ���� ������Ʈ �� �� �Ÿ� ���ϱ�
        currentDistance = Vector3.Distance(player.transform.position, transform.position);
        
        // float Ÿ���� �Ķ���Ϳ� ���� ����
        animator.SetFloat("distanceFromPlayer", currentDistance);
        
        // �÷��̾� ������Ʈ�� ���ϴ� ���� ���ϱ�
        checkDirection = player.transform.position - transform.position;
        
        // ���� ��ġ���� �÷��̾� �������� ray ���
        ray = new Ray(transform.position, checkDirection);

        Vector3 drawDirection = maxDistanceToCheck * checkDirection.normalized;

        // DrawLine �� ��ġ�� ��ġ���� ���� �׸���
        // DrawRay �� ������ġ ���� �������� �������� �� �׸���
        Debug.DrawRay(transform.position, drawDirection, Color.red);

        // ray �� �ִ� �Ÿ� �ȿ� ������ hit �� ����� ����
        if (Physics.Raycast(ray, out hit, maxDistanceToCheck))
        {
            // ��ü ���� ���� ��(���� ��ü�� �����ϴ��� ����)
            // ���� ������ ���ϸ� �޸� �Ҹ� ��û���� ������ ����������� ����
            if (hit.collider.gameObject == player)
                animator.SetBool("isPlayerVisible", true);
            else
                animator.SetBool("isPlayerVisible", false);
        }
        else
            animator.SetBool("isPlayerVisible", false);


        // ������ ���� �Ÿ�
        distanceFromTarget = Vector3.Distance(waypoints[currentTarget].position, transform.position);

        // distanceFromWaypoint �� ���� ������ ���� �� �Ÿ��� ����
        animator.SetFloat("distanceFromWaypoint", distanceFromTarget);
    }

    // �ִϸ����ͺ信�� FindNewTarget �����϶� ȣ��
    public void SetNextPoint()
    {
        switch (currentTarget)
        {
            case 0: //������ ��ȣ�� 0
                currentTarget = 1;
                break;
            
            case 1: // ������ ��ȣ�� 1
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
