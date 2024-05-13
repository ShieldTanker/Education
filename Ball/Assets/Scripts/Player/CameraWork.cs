using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    public GameObject ball;
    private void Start()
    {
        // "Ball"이라는 이름의 게임오브젝트 를 ball 변수에 저장
        //ball = GameObject.Find("Ball");

    }
    private void Update()
    {
        // 로그에  "I ama camera. And ball is at" 문구와 ball 변수의 z값을 표시
        //Debug.Log("I am camera. And ball is at " + ball.transform.position.z);
        
        // 변수 ballPosition 에 ball.transform.position 값을 저장
        // Ball 오브젝트의 변한값을 따라감
        Vector3 ballPosition = ball.transform.position;
        
        //앞에 아무 키워드 없으면 스크립트 저장된 오브젝트만 포함
        // x축은 0고정, y축은 ball 변수의 position.y 값의 6만큼 추가한값, z축은 ball 변수의 position.z. 값의 -14 만큼 추가한 값
        transform.position = new Vector3(
            0,
            ballPosition.y + 6,
            ballPosition.z - 14);
    }
}
