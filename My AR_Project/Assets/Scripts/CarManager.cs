using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class CarManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject myCar;
    public float relocationdDistance = 1f;

    private ARRaycastManager arManager;
    private GameObject placedObject;

    void Start()
    {
        // ǥ�� ��Ȱ��ȭ
        indicator.SetActive(false);

        arManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // �ٴ� ����
        DetectedGround();

        // ����, �ε������Ͱ� Ȱ��ȭ ���̸鼭 ȭ�� ��ġ�� �ִ� ���¶��
        if (indicator.activeInHierarchy && Input.touchCount > 0)
        {
            // ù ��° ��ġ ���¸� ������
            Touch touch = Input.GetTouch(0);

            // ����, ���� Ŭ�� or ��ġ�� ������Ʈ�� UI ������Ʈ��� Update �Լ��� ����
            // EventSystem : UI ������Ʈ�� �ش�
            if (EventSystem.current.currentSelectedGameObject)
            {
                return;
            }

            // ���� ��ġ�� ���۵� ���¶�� �ڵ����� �ε������� �� ������ ���� ����
            // Began : ��ġ �ϴ� ����
            if (touch.phase == TouchPhase.Began)
            {
                // ���� ������ ������Ʈ�� ���ٸ� �������� ���� ���� �ϰ� placeObject �� �Ҵ�
                if (placedObject == null)
                {
                   placedObject = Instantiate(
                       myCar, indicator.transform.position, indicator.transform.rotation);
                }
                // ������ ������Ʈ�� �ִٸ� �� ������Ʈ�� ��ġ�� ȸ������ ����
                else
                {
                    // ���� ������ ������Ʈ�� ǥ��(�ε�������) ������ �Ÿ���
                    // �ְ� �̵����� �̻��̶��
                    float distacne = Vector3.Distance(
                        placedObject.transform.position,
                        indicator.transform.position);

                    if (distacne > relocationdDistance)
                    {
                        placedObject.transform.SetPositionAndRotation(
                            indicator.transform.position, indicator.transform.rotation);

                    }
                }
            }
        }

    }

    void DetectedGround()
    {
        // ��ũ�� �߾����� ã��
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        // ���̿� �ε��� ������ ������ ������ ����Ʈ ������ ����
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        ARRaycastHit hitInfo = new ARRaycastHit();

        // ����, ��ũ�� �߾��������� ���̸� �߻��Ͽ��� �� Plane Ÿ�� ���� ����� �ִٸ�
        if (arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
            // ǥ�� ������Ʈ�� Ȱ��ȭ
            indicator.SetActive(true);

            // ǥ�� ������Ʈ�� ��ġ �I ȸ�� ���� ���̰� ���� ������ ��ġ
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation = hitInfos[0].pose.rotation;

            indicator.transform.position += indicator.transform.up * 0.1f;
        }
        else
        {
            // �׷��� �ʴٸ� ǥ�� ������Ʈ ��Ȱ��ȭ
            indicator.SetActive(false);
        }
    }
}
