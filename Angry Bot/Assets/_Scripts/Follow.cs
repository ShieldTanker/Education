using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target;
    public float distance;
    public float height;
    public float speed;

    private Vector3 pos;

    private void Update()
    {
        // pos ������ ��ġ ���ϱ� (�÷��̾� x ��, ī�޶��� ����, �÷��̾� z��ü���� ������� �Ÿ� ��ŭ ���)
        pos = new Vector3(
            target.transform.position.x,
            height,
            target.transform.position.z - distance);

        // ������ ���� ���� �Ÿ��� ���ͱ��ϱ� (�븻������ �����ϸ� ����)
        // (ī�޶� ��ġ, ��������ġ, �����Ÿ�(�̵��� �Ÿ�))
        transform.position = Vector3.Lerp(
            transform.position,
            pos,
            speed * Time.deltaTime);
    }
}
