using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    float rotSpeed = 300;
    private void Update()
    {

        //Debug.Log(Input.GetAxis("Horizontal"));
        // localEulerAngles 는 transform.rotation 과 같음 (로테이션은 오류날수있음)
        float zRotation = transform.localEulerAngles.z;

        //"Horizontal"이라 적힌 Input 매니저의 설정을 불러옴
        //zRotation 값에 - Horizontal 값을 더하고 zRotation에 저장
        zRotation = zRotation - (Input.GetAxis("Horizontal")) * Time.deltaTime * rotSpeed;

        //위아래 키를 눌렀을떄 x축을 기준으로 회전하도록 수정
        float xRotation = transform.localEulerAngles.x;
        xRotation = xRotation - (Input.GetAxis("Vertical")) * Time.deltaTime * rotSpeed;

        //이 오브젝트 tranform.localEulerAngles 값에 xRotation,0,zRotation 값을 저장
        transform.localEulerAngles = new Vector3(xRotation, 0, zRotation);
        // touchCount 터치 되거나 클릭된 숫자,  GetMouseButton 마우스 버튼 누르고 있을때
        if(Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Debug.Log("mouse down : " + Input.mousePosition);
            //Screen 모니터 해상도 의 기준 가로/2 만큼의 값
            if (Input.mousePosition.x < Screen.width / 2)
            { //화면 왼쪽 클릭
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x,
                    0,
                    transform.localEulerAngles.z + 0.5f);
            }
            else
            {   //화면 오른쪽 클릭
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x,
                    0,
                    transform.localEulerAngles.z - 0.5f);
            }
        }
    }
}
