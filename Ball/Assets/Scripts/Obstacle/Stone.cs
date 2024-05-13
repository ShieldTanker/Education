using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    Vector3 target;
    private void Start()
    {
        //처음 한번 위치 저장으로 생성되었을때 Ball 위치
        target = GameObject.Find("Ball").transform.position;
    }
    private void Update()
    {
        //MoveTowards 에는 3가지 매게변수가 필요 (Vector3, Vector3 , float)
        //첫번째 매개변수는 시작점, 두번째 매개변수는 도착점, 세번째 매개변수는 이동하는 거리

        //아래코드로 하면 계속 Ball위치값을 줌으로 유도가 됨
        //target = GameObject.Find("Ball").transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target, 0.02f);

        //Rotate 메소드를 현재 값(변할수있음) 이용하여 0,0,1 방향으로 계속 회전
        //localEulerAngles 는 각도를 정하면 그 각 그대로 이어감
        transform.Rotate(new Vector3(0, 0, 1));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Ball")
        {
            GameManager gmComponent = GameObject.Find("GameManager").GetComponent<GameManager>();
            gmComponent.RestartGame();
        }   
        else
        {
            Destroy(gameObject);
        }
    }
}
