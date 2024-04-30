using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target;
    public float distance;
    public float height;
    public float speed;

    private Vector3 pos;

    private void Update()
    {
        // pos 목적지 위치 구하기 (플레이어 x 값, 카메라의 높이, 플레이어 z위체에서 어느정도 거리 만큼 띄움)
        pos = new Vector3(
            target.transform.position.x,
            height,
            target.transform.position.z - distance);

        // 목적지 까지 일정 거리의 벡터구하기 (노말라이즈 생각하면 편함)
        // (카메라 위치, 목적지위치, 일정거리(이동할 거리))
        transform.position = Vector3.Lerp(
            transform.position,
            pos,
            speed * Time.deltaTime);
    }
}
