using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class DirectorAction : MonoBehaviour
{
    PlayableDirector pd;        // 감독 오브젝트

    public Camera targetCam;

    private void Start()
    {
        // Director 오브젝트가 가지고있는 PlayableDirector 컴포넌트를 가지고 옴
        pd = GetComponent<PlayableDirector>();

        // 타임라인 실행
        pd.Play();
    }

    private void Update()
    {
        // 현재 진행중인 시간이 전체 시간과 크거나 같으면 (재싱시간이 다되면)
        if (pd.time >= pd.duration)
        {
            // 만약 메인카메라가 타겟카메라(씨네마신에 활용되는 카메라)라면
            // 제어를 위해서 씨네머신 브레인을 비활성화
            if (Camera.main == targetCam)
            {
                targetCam.GetComponent<CinemachineBrain>().enabled = false;
            }
            // 씨네머신에 사용한 카메라도 비활성화
            targetCam.gameObject.SetActive(false);

            // Diretor 자신을 비활성화
            gameObject.SetActive(false);
        }
    }
}
