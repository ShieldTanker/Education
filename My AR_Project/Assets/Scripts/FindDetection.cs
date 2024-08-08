using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;
using UnityEngine.UI;


public class FindDetection : MonoBehaviour
{
    public GameObject smallCube;
    List<GameObject> faceCubes = new List<GameObject>();

    public Text vertexIndex;

    public ARFaceManager afm;
    private ARCoreFaceSubsystem subSys;

    // 특정 방식으로 설정할시 메모리 사용 최적화
    NativeArray<ARCoreFaceRegionData> regionData;

    void Start()
    {
        // 위치 표시를 위한 작은 큐브 3개를 생성
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(smallCube);
            faceCubes.Add(go);
            go.SetActive(false);
        }

        // AR Face Manager 가 얼굴을 인식할 때 실행할 함수를 연결
        // afm.facesChanged += OnDetectThreePoints;
        afm.facesChanged += OnDetectFaceAll;

        // AR Foundation의 XRFaceSubsystem 클래스 변수를
        // AR Core의 ARCoreFaceSubsystem 클래스 변수로 캐스팅
        subSys = (ARCoreFaceSubsystem)afm.subsystem;
    }
    void OnDetectFaceAll(ARFacesChangedEventArgs args)
    {
        // 얼굴을 인식했을 때에는
        if (args.updated.Count > 0)
        {
            // 텍스트 UI에 적힌 문자열 데이터를 정수형 데이터로 변환
            // 델리게이트에 메소드 넣어놓았고 델기게이트 호출은 다른쪽에서 호출함
            int num = int.Parse(vertexIndex.text);

            // 얼굴 정점 배열에서 지정한 인덱스에 해당하는 좌표를 가져옴
            // 총 468 개의 버텍스가 있음
            Vector3 vertPosition = args.updated[0].vertices[num];

            // 정점 좌표를 월드 좌표로 변환
            vertPosition = args.updated[0].transform.TransformPoint(vertPosition);

            // 준비된 큐브 하나를 활성화 하고, 정점 위치에 가져다 놓음
            faceCubes[0].SetActive(true);
            faceCubes[0].transform.position = vertPosition;
        }
        else if (args.removed.Count > 0)
        {
            faceCubes[0].SetActive(false);
        }
    }
    // facesChanged 델리게이트(Action)에 연결할 함수
    void OnDetectThreePoints(ARFacesChangedEventArgs args)
    {
        // 얼굴 인식 정보가 갱신된 것이 있다면
        if (args.updated.Count > 0)
        {
            // 인식된 얼굴에서 특정 위치를 가져옴
            subSys.GetRegionPoses(
                // update : 변경된것이 여러개일수 있어 배열
                args.updated[0].trackableId,
                // Persistent 방식으로 regionData 에 저장
                // Persistent : 가장 느린 할당이지만 애플리케이션 주기에 걸쳐 필요한만큼 오래 지속
                Allocator.Persistent, ref regionData);

            // 인식된 얼굴의 특정 위치(0: 코끝, 1:이마 좌측, 2: 이마 우측) 에 오브젝트 위치
            for (int i = 0; i < regionData.Length; i++)
            {
                faceCubes[i].transform.position = regionData[i].pose.position;
                faceCubes[i].transform.rotation = regionData[i].pose.rotation;
                faceCubes[i].SetActive(true);
            }
        }
        // 얼굴 인식 정보를 잃었다면
        else if (args.removed.Count > 0)       
        {
            // 오브젝트 비활성화
            for (int i = 0; i < regionData.Length; i++)
            {
                faceCubes[i].SetActive(false);
            }
        }
    }
}
