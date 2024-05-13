using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        //Ball ������Ʈ�� �΋H������
        if (col.gameObject.name == "Ball")
        {
            // ���ӸŴ��� �� GetCoin �̶�� �޽��� ȣ��
            GameManager gmComponent = GameObject.Find("GameManager").GetComponent<GameManager>();
            gmComponent.GetCoin();
            // ���ִ� (�ڱ��ڽ��� ������Ʈ) ��� ��
            Destroy(gameObject);
        }
    }
}
