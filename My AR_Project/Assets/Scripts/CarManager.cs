using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class CarManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject myCar;
    public float relocationdDistance = 1f;

    private ARRaycastManager arManager;
    private GameObject placedObject;

    void Start()
    {
        // 표식 비활성화
        indicator.SetActive(false);

        arManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // 바닥 감지
        DetectedGround();

        // 만일, 인디케이터가 활성화 중이면서 화면 터치가 있는 상태라면
        if (indicator.activeInHierarchy && Input.touchCount > 0)
        {
            // 첫 번째 터치 상태를 가져옴
            Touch touch = Input.GetTouch(0);

            // 만일, 현재 클릭 or 터치한 오브젝트가 UI 오브젝트라면 Update 함수를 종료
            // EventSystem : UI 오브젝트만 해당
            if (EventSystem.current.currentSelectedGameObject)
            {
                return;
            }

            // 만일 터치가 시작된 상태라면 자동차를 인디케이터 와 동일한 곳에 생성
            // Began : 터치 하는 순간
            if (touch.phase == TouchPhase.Began)
            {
                // 만일 생성된 오브젝트가 없다면 프리팹을 씬에 생성 하고 placeObject 에 할당
                if (placedObject == null)
                {
                   placedObject = Instantiate(
                       myCar, indicator.transform.position, indicator.transform.rotation);
                }
                // 생성된 오브젝트가 있다면 그 오브젝트의 위치와 회전값을 변경
                else
                {
                    // 만일 생성된 오브젝트와 표식(인디케이터) 사이의 거리가
                    // 최고 이동범위 이상이라면
                    float distacne = Vector3.Distance(
                        placedObject.transform.position,
                        indicator.transform.position);

                    if (distacne > relocationdDistance)
                    {
                        placedObject.transform.SetPositionAndRotation(
                            indicator.transform.position, indicator.transform.rotation);

                    }
                }
            }
        }

    }

    void DetectedGround()
    {
        // 스크린 중앙지점 찾기
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        // 레이에 부딪힌 대상들의 정보를 저장할 리스트 변수를 만듬
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        ARRaycastHit hitInfo = new ARRaycastHit();

        // 만일, 스크린 중앙지점에서 레이를 발사하였을 때 Plane 타입 추적 대상이 있다면
        if (arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
            // 표식 오브젝트를 활성화
            indicator.SetActive(true);

            // 표식 오브젝트의 위치 밒 회전 값을 레이가 닿은 지점에 일치
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation = hitInfos[0].pose.rotation;

            indicator.transform.position += indicator.transform.up * 0.1f;
        }
        else
        {
            // 그렇지 않다면 표식 오브젝트 비활성화
            indicator.SetActive(false);
        }
    }
}
