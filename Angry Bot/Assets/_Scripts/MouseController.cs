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
        // 메인 카메라의 마우스 위치
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        // ray 와 부딫힌 오브젝트의 정보를 hit 에 넣기 (return은 하나밖에 못주기 때문)
        // RayCast 는 bool 타입임 물체에 닿으면 true 아니면 false
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 부딫힌 위치는 hit 에 저장 되어 있음 (hit.pint.x, 1f (물체에 겹쳐 보일수있어 0.5f 만큼 띄움), hit.point.z)
            cursor.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
            // hit.collider -> 오브젝트의 콜라이더 객체

            // 마우스 방향으로 사격
            if (Input.GetMouseButtonDown(0) && playerCtrl.playerState != PlayerState.Dead)
            {
                target.position = new Vector3(hit.point.x, 0f, hit.point.z);
                playerCtrl.lookDirection = target.position - playerCtrl.transform.position;
                playerCtrl.StartCoroutine("Shot");
            }
        }
    }
}
