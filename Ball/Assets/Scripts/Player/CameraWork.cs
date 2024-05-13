using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    public GameObject ball;
    private void Start()
    {
        // "Ball"�̶�� �̸��� ���ӿ�����Ʈ �� ball ������ ����
        //ball = GameObject.Find("Ball");

    }
    private void Update()
    {
        // �α׿�  "I ama camera. And ball is at" ������ ball ������ z���� ǥ��
        //Debug.Log("I am camera. And ball is at " + ball.transform.position.z);
        
        // ���� ballPosition �� ball.transform.position ���� ����
        // Ball ������Ʈ�� ���Ѱ��� ����
        Vector3 ballPosition = ball.transform.position;
        
        //�տ� �ƹ� Ű���� ������ ��ũ��Ʈ ����� ������Ʈ�� ����
        // x���� 0����, y���� ball ������ position.y ���� 6��ŭ �߰��Ѱ�, z���� ball ������ position.z. ���� -14 ��ŭ �߰��� ��
        transform.position = new Vector3(
            0,
            ballPosition.y + 6,
            ballPosition.z - 14);
    }
}
