using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 부모클래스 상속
public class Touch : Sense
{
    private void OnTriggerEnter(Collider other)
    {
        // 닿은 오브젝트의 Aspect 컴포넌트 가져오기
        Aspect aspect = other.GetComponent<Aspect>();

        // 비어있지 않으면
        if (aspect != null)
        {
            // 목표 이름과 같은지 확인
            if (aspect.aspectName == aspectName)
            {
                Debug.Log("Enemy Touch Detected");
            }
        }
    }
}
