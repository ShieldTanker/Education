using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCoin : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        //Ball ������Ʈ�� �΋H������
        if (col.gameObject.name == "Ball")
        {
            // DestroyObstacles() �޼ҵ� ����
            //DestroyObstacles();

            GameManager gmComponent = GameObject.Find("GameManager").GetComponent<GameManager>();
            gmComponent.RedCoinStart();

            // ���ִ� (�ڱ��ڽ��� ������Ʈ) ��� ��
            Destroy(gameObject);
        }
    }

}
