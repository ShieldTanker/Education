using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    float delta = -0.01f;

    private void Update()
    {
        float newXposition = transform.localPosition.x + delta;
        transform.localPosition = new Vector3(newXposition, transform.localPosition.y, transform.localPosition.z);

        if (transform.localPosition.x < -3.5f)
        {
            delta = 0.01f;
        }
        else if (transform.localPosition.x > 3.5f)
        {
            delta = -0.01f;
        }
    }

    // collison = �浹�� ������Ʈ
    private void OnCollisionEnter(Collision collision)
    {   // ����) �浹�� ������Ʈ�� ��ġ(8,5) - �� ��ũ��Ʈ�� ���� ������Ʈ�� ��ġ(0,0) = (8,5) �������� ����
        Vector3 direction = collision.transform.position - transform.position;
        
        // nomalized = ������ ������ �״�� �̸鼭 ���̴� 1�� �������(��ü�� ũ�� ������� ���� ������ ���ư�)
        // �� �ڵ�� ���� ������� ���͹������� 1000 �� ���̷� �������
        direction = direction.normalized * 100;
        
        // �浹�� ������Ʈ(collision) �� ���ӿ�����Ʈ �� Rigidbody�� �����ͼ� direction ����ŭ ���� ���Ѵ�
        // gameObject = ����Ƽ���� ������ ����
        collision.gameObject.GetComponent<Rigidbody>().AddForce(direction);
    }

    private void Start()
    {
        TestMethod("Ball");
    }

    void TestMethod(string name)
    {
        // Vector3.Distance �� Vector3.Distance(Vector3 a,Vector3 b) �� �� �Ÿ��� ������
        float distance = Vector3.Distance(
            GameObject.Find(name).transform.position,
            transform.position);
        //Debug.Log(name + "���� �Ÿ� : " + distance);
    }
}
