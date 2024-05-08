using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Transform target;
    public GameObject cursor;
    public PlayerController playerCtrl;

    private void Update()
    {
        // ���� ī�޶��� ���콺 ��ġ
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        // ray �� �΋H�� ������Ʈ�� ������ hit �� �ֱ� (return�� �ϳ��ۿ� ���ֱ� ����)
        // RayCast �� bool Ÿ���� ��ü�� ������ true �ƴϸ� false
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // �΋H�� ��ġ�� hit �� ���� �Ǿ� ���� (hit.pint.x, 1f (��ü�� ���� ���ϼ��־� 0.5f ��ŭ ���), hit.point.z)
            cursor.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
            // hit.collider -> ������Ʈ�� �ݶ��̴� ��ü

            // ���콺 �������� ���
            if (Input.GetMouseButtonDown(0) && playerCtrl.playerState != PlayerState.Dead)
            {
                target.position = new Vector3(hit.point.x, 0f, hit.point.z);
                playerCtrl.lookDirection = target.position - playerCtrl.transform.position;
                playerCtrl.StartCoroutine("Shot");
            }
        }
    }
}
