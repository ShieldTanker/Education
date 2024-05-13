using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScripts : MonoBehaviour
{
    // �߰��� �� �������� �����ϴ� �迭
    public GameObject[] availableRooms;
    // ���� ���ӿ� �߰�(����)�� �� ������Ʈ ����Ʈ
    public List<GameObject> currentRooms;
    
    // ȭ���� ���� ũ�� (���� : ����)
    float screenWidthInPoints;

    public MouseController mouseController;

    const string floor = "Floor";

    public float startToFever;
    public float continueToFever;

    public MouseController mc;
    



    // �߰��� ������Ʈ �������� �����ϴ� �迭
    public GameObject[] availableObjects;
    // ���� ������ ������Ʈ(����,������) ����Ʈ
    public List<GameObject> objects;



    // ������Ʈ�� �ּ� ����
    public float objectsMinDistance = 5f;
    // ������Ʈ�� �ִ� ����
    public float objectsMaxDistance = 10f;
    
    // ������Ʈ Y�� ��ġ �ּҰ�
    public float objectsMinY = -1.4f;
    // ������Ʈ Y�� ��ġ �ִ밪
    public float objectsMaxY = 1.4f;
    
    // ������Ʈ �ּ� ȸ����
    public float objectsMinRotation = -45f;
    // ������Ʈ �ִ� ȸ����
    public float objectsMaxRotation = 45f;


    private int listIndexLength;

    private bool isDead;


    private void Start()
    {
        /* MainCamera �±״޸� ������Ʈ�� Camera ������Ʈ ��
           Projection Ÿ���� orthographic �϶� ������
           (���� 3.2 �̸� �� �Ʒ� ���ļ� �� 6.4 ������)
           �׷��� ���� ī�޶� ���� ũ�⸦ ���ϱ� ���� 2�� ���ϴ°�                                                             */
        float height = 2.0f * Camera.main.orthographicSize;       // height = 6.4f
        
        /* 16(����):9(����) ó�� ���� ���� ���� �����ϱ�
           aspect = ���θ� 1�� ������ �ξ��� �� ���� ����(16:9 �̸� 1.777:1)
           ȭ���� ���� ũ�⸦ ���ϱ� ���� ���̿� aspect �� ����(���� ũ�� ���ϴ� �޼ҵ尡 ����)                                  */
        screenWidthInPoints = height * Camera.main.aspect;
        
        listIndexLength = availableObjects.Length;


        mouseController = GetComponent<MouseController>();

        //StartCoroutine(FeverTime());
    }

    private void Update()
    {
        GenerateRoomIfReauired();
        GenerateObjectsIfRequired();
        isDead = mouseController.died;
    }


    // �� ����
    // GenerateRoomIfReauired() �ȿ��� ����
    private void AddRoom(float farthestRoomEndX)
    {
        // �� ������ �߿��� �ϳ��� ���� ����
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        // �������� �� ��(��) ������Ʈ �߰�
        GameObject room = Instantiate(availableRooms[randomRoomIndex]);
        
        // ���� ���� ũ�� ���ϱ�
        float roomWidth = room.transform.Find(floor).localScale.x;
        
        /* ������ ���� ��ġ
           ��(��) ������Ʈ�� x ���� + ��(��)�� ����ũ�� / 2                */
        float roomCenter = farthestRoomEndX + roomWidth / 2;
        
        // ������ ���� ��ġ�� ��ġ ��Ŵ
        room.transform.position = new Vector3(roomCenter, 0, 0);

        // �߰��� ���� ���� �߰��� �� ����Ʈ�� �߰�
        currentRooms.Add(room);
    }

    private void GenerateRoomIfReauired()
    {
        /* ������ ���� ����� �����ϴ� ����Ʈ
           �ٷ� ������ �ʰ� ����Ʈ�� �־���� �ѹ��� ������°�
           �� ������ �� ���� ���� �����ɼ� �ֱ⿡ ����Ʈ�� ó��                    */
        List<GameObject> roomsToMove = new List<GameObject>();
        
        // ���� �����ӿ� ���� �������� ����
        bool addRooms = true;
        
        // ���콺 ������Ʈ�� x �� ��ġ
        float playerX = transform.position.x;
        
        /* �÷��̾� ���� ��ũ�� ���α��� ��ŭ �ڷ� ����
           ������ ��(��) �� ���� ��ġ�� ��              */
        float removeRoomX = playerX - screenWidthInPoints;

        /* �÷��̾� ���� ��ũ�� ���� ���̸�ŭ ������ ����
           �߰��� ��(��) �� ���� ��ġ�� ��(������ġ �� ���̾����� ���� �ϱ� ����)           */
        float addRoomX = playerX + screenWidthInPoints;

        // ���� ������ ��(��) �� ���� �������� ��ġ�� ���� ������ �� ��ġ
        float farthestRoomEndX = 0f;


        //���� ������ �� �� �ϳ��� ó��
        foreach (var room in currentRooms)
        {
            /* ���� floor = "Floor" �� �ʱ�ȭ �Ǿ�����
               ���� ó�� �ϰ��ִ� ���� �ڽĿ�����Ʈ �߿��� �ٴڿ�����Ʈ�� ã�� ����ũ�⸦ ������      */
            float roomWidth = room.transform.Find(floor).localScale.x;

            /* ��(��) �� �߾���ġ ���� ��(��) �� ����ũ�� �� ��������
               (���� ���� ��ġ�� ����)                           */
            float roomStartX = room.transform.position.x - roomWidth / 2;

            /* ��(��) �� ���� ����ġ ���� ��(��)�� ũ�⸸ŭ ����
               (������ �� ��ġ�� ����)                           */
            float roomEndX = roomStartX + roomWidth;
            
            /* ��(��) ���� ó��,
               ��(��)�� ������(���� ��) �� �߰��� x �� ���� �����ʿ�������             */
            if (roomStartX > addRoomX)
                // ���� �߰� ���� �ʰ� �������� �ٲ�
                addRooms = false;

            // ��(��)�� ����(������ ��) �� ������ x �� ���� ���ʿ� ������
            if (roomEndX < removeRoomX)
                /* ����� ��Ͽ� �߰�
                   �ٷ������� �ʴ� ������ ���� currentRooms ����Ʈ��
                   ��� ���ε� �߰��� �����ع����� ���� �Ǿ���� ���ϼ� ����         */
                roomsToMove.Add(room);

            // ���� ������ ���� ������ �� ��ġ�� �ִ밪 �޼ҵ带 �̿��Ͽ� ����
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        //������ �� ������Ʈ�� �ϳ��� �����ϸ鼭
        foreach (var room in roomsToMove)
        {
            // ����Ʈ���� ����
            currentRooms.Remove(room);
            // ������Ʈ ����
            Destroy(room);
        }
        // ���� �߰��ؾ��Ѵٸ� ���� �߰�
        if (addRooms)
            AddRoom(farthestRoomEndX);
    }



    //������Ʈ ����
    private void AddObject(float lastObjectX)
    {
        // �߰��� ������Ʈ �� ���� �ε��� ���� ����
        int randomIndex;

        if (mc.isFever)
        {
            do
            {
                randomIndex = Random.Range(0, listIndexLength);
            } while (availableObjects[randomIndex].CompareTag("Laser"));
        }
        else
            randomIndex = Random.Range(0, availableObjects.Length);

        // �������� ���� �ε��� ��ȣ�� ������Ʈ�� ����
        GameObject obj = Instantiate(availableObjects[randomIndex]);

        // ���ο� ������Ʈ�� X �� ��ġ�� ���
        float objectPositionX =
            lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);

        // ���ο� ������Ʈ�� Y �� ��ġ�� ���
        float randomY = Random.Range(objectsMinY, objectsMaxY);

        // ���� ��ġ���� ������Ʈ�� ��ġ�� ����
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);

        // �������� ȸ���� �� ���
        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);

        /* �������� ������ ȸ�������� ������Ʈ�� ���ʹϾ� ȸ���� ���� ����Ͽ� ȸ��
           ���Ͱ� �����δ� ȸ�� ��������(tranform.rotation �� �Ӽ��� Quaternion ��)
           Quaternion.Euler �� ���Ͱ�(x, y, z) ����� ���ʹϾ� ������ ��������    */ 
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
        
        // ������Ʈ ����Ʈ�� �߰�
        objects.Add(obj);
    }

    private void GenerateObjectsIfRequired()
    {
        // �÷��̾� X �� ��ġ ��
        float playerX = transform.position.x;

        /* ������ ������Ʈ�� ���� ��ġ
           �÷��̾� ��ġ ���� �������� ��ũ�� x�� ����            */
        float removeObjectX = playerX - screenWidthInPoints;
        
        /* �߰��� ������Ʈ�� ���� ��ġ
           �÷��̾� ���� ���������� ��ũ�� x ���� ����            */
        float addObjectX = playerX + screenWidthInPoints;
        
        // ���� �������� ��ġ�� ������Ʈ �� X�� ��ġ
        float farthestObjectX = 0f;

        // ������ ������Ʈ �� ���� ����Ʈ
        List<GameObject> objectsToRemove = new List<GameObject>();

        // ���� �߰��Ǿ��ִ� ������Ʈ�� �� �ϳ��� ó��
        foreach (var obj in objects)
        {
            // ������Ʈ�� X �� ��ġ
            float objX = obj.transform.position.x;

            // �ִ밪 �񱳷� ���� �������� ��ġ�� ������Ʈ �� ��ġ�� ����
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            // ������Ʈ ��ġ�� ���� ���� ��ġ���� �����̸� 
            if (objX < removeObjectX)
            {
                // ������Ʈ ���� ����Ʈ�� �߰�
                objectsToRemove.Add(obj);
            }
        }

        // ���� ����Ʈ�� �߰��� ������Ʈ�� ��� ����
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        // ���� �����ʿ� ��ġ�� ������Ʈ�� �߰� ������ġ ���� �����̸�
        if (farthestObjectX < addObjectX)
        {
            // ������Ʈ �߰� ����Ʈ�� �ֱ�
            AddObject(farthestObjectX);
        }
    }



    /*// �ǹ�Ÿ��
    IEnumerator FeverTime()
    {
        while (!isDead)
        {
            listIndexLength = availableObjects.Length;
            
            GameObject[] removeLaser;

            //�ǹ� ���۱��� �ɸ��� �ð�
            yield return new WaitForSeconds(startToFever);

            if (!isDead)
            {
                listIndexLength = 3;
                removeLaser = GameObject.FindGameObjectsWithTag("Laser");

                for (int i = 0; i < removeLaser.Length; i++)
                {
                    objects.Remove(removeLaser[i]);
                    Destroy(removeLaser[i]);
                }

                Debug.Log("FeverTime");
            }
            else
                break;

            //�ǹ� ���� �ð�
            yield return new WaitForSeconds(continueToFever);
            
            if (!isDead)
                Debug.Log("FeverEnd");
        }
    }
    */
}
        /* ����Ʈ ��� ȣ���Ҷ�
        IEnumerator FlashFire()
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
        }
        */
