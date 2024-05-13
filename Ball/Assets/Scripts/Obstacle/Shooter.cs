using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    float timeCount;

    public GameObject stoneObject;
       
    private void Start()
    {
        timeCount = 0;
    }
    private void Update()
    {
        //Update는 프레임마다 호출 하기에 간격이 불규칙할수있음
        //Time.deltaTime = Update 호출 사이 간격
        timeCount += Time.deltaTime;

        if(timeCount > 3.0f)
        {
            ShootPoint sPoint = GameObject.Find("ShootPoint").GetComponent<ShootPoint>();
            
            //Isntantiate<>(불러올 오브젝트, 불러올 위치, 회전값)
            //Quaternion.identity = 기존 회전값

            Instantiate(stoneObject, sPoint.transform.position, Quaternion.identity);
            Debug.Log("Fire!");
            //시간 초기화
            timeCount = 0;
        }
    }
}
