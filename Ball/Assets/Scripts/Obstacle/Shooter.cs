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
        //Update�� �����Ӹ��� ȣ�� �ϱ⿡ ������ �ұ�Ģ�Ҽ�����
        //Time.deltaTime = Update ȣ�� ���� ����
        timeCount += Time.deltaTime;

        if(timeCount > 3.0f)
        {
            ShootPoint sPoint = GameObject.Find("ShootPoint").GetComponent<ShootPoint>();
            
            //Isntantiate<>(�ҷ��� ������Ʈ, �ҷ��� ��ġ, ȸ����)
            //Quaternion.identity = ���� ȸ����

            Instantiate(stoneObject, sPoint.transform.position, Quaternion.identity);
            Debug.Log("Fire!");
            //�ð� �ʱ�ȭ
            timeCount = 0;
        }
    }
}
