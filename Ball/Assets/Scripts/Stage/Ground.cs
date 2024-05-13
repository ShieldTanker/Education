using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    float rotSpeed = 300;
    private void Update()
    {

        //Debug.Log(Input.GetAxis("Horizontal"));
        // localEulerAngles �� transform.rotation �� ���� (�����̼��� ������������)
        float zRotation = transform.localEulerAngles.z;

        //"Horizontal"�̶� ���� Input �Ŵ����� ������ �ҷ���
        //zRotation ���� - Horizontal ���� ���ϰ� zRotation�� ����
        zRotation = zRotation - (Input.GetAxis("Horizontal")) * Time.deltaTime * rotSpeed;

        //���Ʒ� Ű�� �������� x���� �������� ȸ���ϵ��� ����
        float xRotation = transform.localEulerAngles.x;
        xRotation = xRotation - (Input.GetAxis("Vertical")) * Time.deltaTime * rotSpeed;

        //�� ������Ʈ tranform.localEulerAngles ���� xRotation,0,zRotation ���� ����
        transform.localEulerAngles = new Vector3(xRotation, 0, zRotation);
        // touchCount ��ġ �ǰų� Ŭ���� ����,  GetMouseButton ���콺 ��ư ������ ������
        if(Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Debug.Log("mouse down : " + Input.mousePosition);
            //Screen ����� �ػ� �� ���� ����/2 ��ŭ�� ��
            if (Input.mousePosition.x < Screen.width / 2)
            { //ȭ�� ���� Ŭ��
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x,
                    0,
                    transform.localEulerAngles.z + 0.5f);
            }
            else
            {   //ȭ�� ������ Ŭ��
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x,
                    0,
                    transform.localEulerAngles.z - 0.5f);
            }
        }
    }
}
