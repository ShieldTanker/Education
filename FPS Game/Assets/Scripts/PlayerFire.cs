using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 발사 위치
    public GameObject firePosition;

    // 투척 무기 오브젝트
    public GameObject bombFactory;

    // 투척 파워
    public float throwPower = 15f;

    private void Update()
    {
        // 마우스 오른쪽 버튼을 입력 받음
        if (Input.GetMouseButtonDown(1))
        {
            // 수류탄 오브젝트를 생성한 후 수류탄의 생성 위치를 발사 위치로 변경
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            // 수류탄의 오브젝트의 Rigidbody 컴포넌트를 가져옴
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            // 카메라의 정면 방향으로 수류탄에 물리적인 힘을 가함 (Impulse : 순간적인 힘)
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
    }
}
