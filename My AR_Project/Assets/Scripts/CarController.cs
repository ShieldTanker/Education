using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject[] bodyObject;
    public Color32[] colors;
    public float rotSpeed;

    private Material[] carMats;

    private void Start()
    {
        // catMats 배열을 자동차 바디 오브젝트의 수만큼 초기화
        carMats = new Material[bodyObject.Length];

        // 자동차 바디 오브젝트의 마테리얼 각각을 carMats 배열에 저장
        for (int i = 0; i < carMats.Length; i++)
        {
            carMats[i] = bodyObject[i].GetComponent<MeshRenderer>().material;
        }

        // 색상 배열 0번에는 마테리얼의 초기 색상을 저장
        colors[0] = carMats[0].color;
    }

    private void Update()
    {
        // 만일 터치된 부위가 1개 이상이라면
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 만일 터치 상태가 움직이고 있는 중이라면
            if (touch.phase == TouchPhase.Moved)
            {
                // 만일, 카메라 위치에서 정면 방향으로 레이를 발사하여 부딪힌 대상이
                // 6번 레이어 라면 터치 이동량을 구함
                Ray ray = new Ray(
                    Camera.main.transform.position, Camera.main.transform.forward);

                RaycastHit hitInfo;
                
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 6))
                {
                    // deltaPosition : 처음 터치 한 곳으로부터 이동한 거리 의 위치값
                    Vector3 deltaPos = touch.deltaPosition;

                    // 직전 프레임에서 현재 프레임까지의 x축 터치 이동량에 비례하여
                    // 로컬 y축 방향으로 회전
                    transform.Rotate(transform.up, deltaPos.x * -1f * rotSpeed);
                }
            }
        }
    }

    public void ChangeColor(int num)
    {
        // 각 LOD 머테리얼의 색상을 버튼에 지정된 색상으로 변경
        for (int i = 0; i < carMats.Length; i++)
        {
            carMats[i].color = colors[num];
        }
    }
}
