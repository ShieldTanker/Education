using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wonder : MonoBehaviour
{
    // 목적지
    Vector3 tarPos;

    float minX = -45f;
    float maxX = 45f;
    
    float minZ = -45f;
    float maxZ = 45f;

    float rotSpeed = 2f;
    float movementSpeed = 5f;

    private void Start()
    {
        GetNextPosition();
    }

    private void Update()
    {
        // 타겟의 위치와 오브젝트의 거리 차이가 5 이하이면 도착했다고 판정
        if (Vector3.Distance(tarPos, transform.position) <= 5f)
        {
            // 다음 목적지 설정
            GetNextPosition();
        }

        // 부드럽게 회전 시키기
        Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);

        // 지금 회전방향, 목적 회전방향, 회전양(델타타임 넣음으로서 속도 조절 가능)  
        transform.rotation = Quaternion.Slerp(
            transform.rotation, tarRot, rotSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }

    void GetNextPosition()
    {
        tarPos = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
    }
}
