using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;

public class UIManager : MonoBehaviour
{
    public ARFaceManager faceManager;
    public Material[] faceMats;

    // 버튼을 눌렀을 때 실행될 함수
    public void ToggleMaskImage()
    {
        // 얼굴이 하나가 아닌 여러명일 수 있기에 foreach문 사용
        // faceManager 컴포넌트에서 현재 생성된 face 오브젝트를 모두 순회
        foreach (ARFace face in faceManager.trackables)
        {
            // 만일 face 오브젝트가 얼굴을 인식하고 있는 상태라면
            if (face.trackingState == TrackingState.Tracking)
            {
                // face 오브젝트 의 활성화 상태를 반대로 설정
                // activeSelf : 자기 자신의 활성화 여부(부모 오브젝트와 무관)
                // activeInHierarchy : 부모 오브젝트가 비활성화 되면 같이 비활성
                face.gameObject.SetActive(!face.gameObject.activeSelf);
            }
        }
    }
    // 마테리얼 변경 버튼 함수
    public void SwitchFaceMaterial(int num)
    {
        // FaceManager 컴포넌트에서 현재 생성된 face 오브젝트를 모두 순회
        foreach (ARFace face in faceManager.trackables)
        {
            // 만일 face 오브젝트가 얼굴을인식하고 있는 상태라면
            if (face.trackingState == TrackingState.Tracking)
            {
                // 탐지된 face 오브젝트의 MeshRenderer 컴포넌트에 접근
                MeshRenderer mr = face.GetComponent<MeshRenderer>();

                // 버튼에 설정된 번호 (이미지:0번, 영상:1번)에 해당하는 머테리얼로 변경
                mr.material = faceMats[num];
            }
        }
    }
}
