using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
        public float rootSpeed;

    private void Update()
    {
        //�� �����Ӹ��� �� ������ 15 , 30, 45 ������ �ش�
        //Time.deltaTime �� ������ ���� ���ڴ� 1�ʿ� �̵��ϴ� �Ÿ�,���� ������ �����ϸ� ����
        transform.Rotate(new Vector3(15, 30, 45) * rootSpeed * Time.deltaTime);
    }
}
