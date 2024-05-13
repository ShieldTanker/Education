using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    Vector3 target;
    private void Start()
    {
        //ó�� �ѹ� ��ġ �������� �����Ǿ����� Ball ��ġ
        target = GameObject.Find("Ball").transform.position;
    }
    private void Update()
    {
        //MoveTowards ���� 3���� �ŰԺ����� �ʿ� (Vector3, Vector3 , float)
        //ù��° �Ű������� ������, �ι�° �Ű������� ������, ����° �Ű������� �̵��ϴ� �Ÿ�

        //�Ʒ��ڵ�� �ϸ� ��� Ball��ġ���� ������ ������ ��
        //target = GameObject.Find("Ball").transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target, 0.02f);

        //Rotate �޼ҵ带 ���� ��(���Ҽ�����) �̿��Ͽ� 0,0,1 �������� ��� ȸ��
        //localEulerAngles �� ������ ���ϸ� �� �� �״�� �̾
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
