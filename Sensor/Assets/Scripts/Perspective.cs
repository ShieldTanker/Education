using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sense 클래스 를 상속 받음
// 시각 센서
public class Perspective : Sense
{
    // 시야 각도
    public float fieldOfView;
    // 시야 거리
    public float viewDistance;

    // 적 오브젝트의 위치값
    Transform enemyTrans;

    // 시야 센서 사용할때 레이케스트 사용
    Vector3 rayDirection;

    // 부모의 가상 클래스 덮어 쓰기
    // 부모크래스에서 스타트 메소드를 상속받았음
    // 부모클래스의 가상메소드를 덮어쓰기
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

        /* Vector3.Angle : 두 벡터값 의 작은값 을 계산 해줌
           (목표 벡터, 기준벡터(transform.forward : 정면) */
        if (Vector3.Angle(rayDirection, transform.forward) < fieldOfView)
        {
            /* 중간에 벽이 가로막는지 확인 하려고 Ray를 쏨
               기준벡터, 목표벡터, 저장할 변수, 쏘는 최대거리 */
            if (Physics.Raycast(transform.position, rayDirection, out hit, viewDistance))
            {
                // 벽에 맞은건지 적 오브젝트에 맞은건지 확인용(Ray 쏠 때 레이어 감지로 해도 됨)
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                // Aspect 컴포넌트가 있으면 if문이 참
                if (aspect != null)
                {
                    // aspect.aspectName 이 aspectName(부모클래스에서 만들어진 변수) 와 같은지
                    if (aspect.aspectName == aspectName)
                    {
                        Debug.Log("Enemy Detected");
                    }
                }
            }
        }
    }
    
    // 디버그용 시야각 확인용
    private void OnDrawGizmos()
    {
        // 디버그 모드가 거짓이거나 적오브젝트가 없을때 리턴
        if (enemyTrans == null || !bDebug)
            return;

        // 해당오브젝트에서 적 오브젝트 로 빨간선을 쏨
        Debug.DrawLine(transform.position, enemyTrans.position, Color.red);

        // 해당 오브젝트 위치에서 정면으로 시야거리만큼 의 벡터값
        Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);

        // 좌우 벡터 구하기
        Vector3 dirRight = transform.forward + transform.right;
        Vector3 dirLeft = transform.forward - transform.right;

        // 시야거리보다 길기 때문에 정규화 함
        // Normalize() 는 원본의 값을 바꿈
        // normalized 는 값을 넘겨주는 방식
        dirRight.Normalize();
        // dirRight = dirRight.normalized; 와 같은 코드
        dirLeft.Normalize();

        // 위에 정면 방향거리 구하는것과 같은 방식
        Vector3 leftRayPoint = transform.position + dirLeft * viewDistance;
        Vector3 rigRayPoint = transform.position + dirRight * viewDistance;

        // 위에 적오브젝트 와 선 연결 한 것 과 같은 방식
        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
        Debug.DrawLine(transform.position, leftRayPoint, Color.green);
        Debug.DrawLine(transform.position, rigRayPoint, Color.green);

    }
}
