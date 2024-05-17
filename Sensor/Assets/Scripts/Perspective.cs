using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sense Ŭ���� �� ��� ����
// �ð� ����
public class Perspective : Sense
{
    // �þ� ����
    public float fieldOfView;
    // �þ� �Ÿ�
    public float viewDistance;

    // �� ������Ʈ�� ��ġ��
    Transform enemyTrans;

    // �þ� ���� ����Ҷ� �����ɽ�Ʈ ���
    Vector3 rayDirection;

    // �θ��� ���� Ŭ���� ���� ����
    // �θ�ũ�������� ��ŸƮ �޼ҵ带 ��ӹ޾���
    // �θ�Ŭ������ ����޼ҵ带 �����
    protected override void Initialise()
    {
        enemyTrans = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    protected override void UpdateSense()
    {
        DetectAspect();
    }

    void DetectAspect()
    {
        RaycastHit hit;
        rayDirection = enemyTrans.position - transform.position;

        /* Vector3.Angle : �� ���Ͱ� �� ������ �� ��� ����
           (��ǥ ����, ���غ���(transform.forward : ����) */
        if (Vector3.Angle(rayDirection, transform.forward) < fieldOfView)
        {
            /* �߰��� ���� ���θ����� Ȯ�� �Ϸ��� Ray�� ��
               ���غ���, ��ǥ����, ������ ����, ��� �ִ�Ÿ� */
            if (Physics.Raycast(transform.position, rayDirection, out hit, viewDistance))
            {
                // ���� �������� �� ������Ʈ�� �������� Ȯ�ο�(Ray �� �� ���̾� ������ �ص� ��)
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                // Aspect ������Ʈ�� ������ if���� ��
                if (aspect != null)
                {
                    // aspect.aspectName �� aspectName(�θ�Ŭ�������� ������� ����) �� ������
                    if (aspect.aspectName == aspectName)
                    {
                        Debug.Log("Enemy Detected");
                    }
                }
            }
        }
    }
    
    // ����׿� �þ߰� Ȯ�ο�
    private void OnDrawGizmos()
    {
        // ����� ��尡 �����̰ų� ��������Ʈ�� ������ ����
        if (enemyTrans == null || !bDebug)
            return;

        // �ش������Ʈ���� �� ������Ʈ �� �������� ��
        Debug.DrawLine(transform.position, enemyTrans.position, Color.red);

        // �ش� ������Ʈ ��ġ���� �������� �þ߰Ÿ���ŭ �� ���Ͱ�
        Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);

        // �¿� ���� ���ϱ�
        Vector3 dirRight = transform.forward + transform.right;
        Vector3 dirLeft = transform.forward - transform.right;

        // �þ߰Ÿ����� ��� ������ ����ȭ ��
        // Normalize() �� ������ ���� �ٲ�
        // normalized �� ���� �Ѱ��ִ� ���
        dirRight.Normalize();
        // dirRight = dirRight.normalized; �� ���� �ڵ�
        dirLeft.Normalize();

        // ���� ���� ����Ÿ� ���ϴ°Ͱ� ���� ���
        Vector3 leftRayPoint = transform.position + dirLeft * viewDistance;
        Vector3 rigRayPoint = transform.position + dirRight * viewDistance;

        // ���� ��������Ʈ �� �� ���� �� �� �� ���� ���
        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
        Debug.DrawLine(transform.position, leftRayPoint, Color.green);
        Debug.DrawLine(transform.position, rigRayPoint, Color.green);

    }
}
