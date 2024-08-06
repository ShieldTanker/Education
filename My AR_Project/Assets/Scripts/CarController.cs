using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject[] bodyObject;
    public Color32[] colors;
    public float rotSpeed;

    private Material[] carMats;

    private void Start()
    {
        // catMats �迭�� �ڵ��� �ٵ� ������Ʈ�� ����ŭ �ʱ�ȭ
        carMats = new Material[bodyObject.Length];

        // �ڵ��� �ٵ� ������Ʈ�� ���׸��� ������ carMats �迭�� ����
        for (int i = 0; i < carMats.Length; i++)
        {
            carMats[i] = bodyObject[i].GetComponent<MeshRenderer>().material;
        }

        // ���� �迭 0������ ���׸����� �ʱ� ������ ����
        colors[0] = carMats[0].color;
    }

    private void Update()
    {
        // ���� ��ġ�� ������ 1�� �̻��̶��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // ���� ��ġ ���°� �����̰� �ִ� ���̶��
            if (touch.phase == TouchPhase.Moved)
            {
                // ����, ī�޶� ��ġ���� ���� �������� ���̸� �߻��Ͽ� �ε��� �����
                // 6�� ���̾� ��� ��ġ �̵����� ����
                Ray ray = new Ray(
                    Camera.main.transform.position, Camera.main.transform.forward);

                RaycastHit hitInfo;
                
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 6))
                {
                    // deltaPosition : ó�� ��ġ �� �����κ��� �̵��� �Ÿ� �� ��ġ��
                    Vector3 deltaPos = touch.deltaPosition;

                    // ���� �����ӿ��� ���� �����ӱ����� x�� ��ġ �̵����� ����Ͽ�
                    // ���� y�� �������� ȸ��
                    transform.Rotate(transform.up, deltaPos.x * -1f * rotSpeed);
                }
            }
        }
    }

    public void ChangeColor(int num)
    {
        // �� LOD ���׸����� ������ ��ư�� ������ �������� ����
        for (int i = 0; i < carMats.Length; i++)
        {
            carMats[i].color = colors[num];
        }
    }
}
