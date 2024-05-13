using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
        public float rootSpeed;

    private void Update()
    {
        //매 프레임마다 각 축으로 15 , 30, 45 각도를 준다
        //Time.deltaTime 이 있으면 앞의 숫자는 1초에 이동하는 거리,각도 등으로 이해하면 편함
        transform.Rotate(new Vector3(15, 30, 45) * rootSpeed * Time.deltaTime);
    }
}
