using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScripts : MonoBehaviour
{
    // 추가될 방 프리팹을 저장하는 배열
    public GameObject[] availableRooms;
    // 현재 게임에 추가(생성)된 방 오브젝트 리스트
    public List<GameObject> currentRooms;
    
    // 화면의 가로 크기 (단위 : 유닛)
    float screenWidthInPoints;

    public MouseController mouseController;

    const string floor = "Floor";

    public float startToFever;
    public float continueToFever;

    public MouseController mc;
    



    // 추가될 오브젝트 프리팹을 저장하는 배열
    public GameObject[] availableObjects;
    // 현재 생성된 오브젝트(코인,레이저) 리스트
    public List<GameObject> objects;



    // 오브젝트간 최소 간격
    public float objectsMinDistance = 5f;
    // 오브젝트간 최대 간격
    public float objectsMaxDistance = 10f;
    
    // 오브젝트 Y축 위치 최소값
    public float objectsMinY = -1.4f;
    // 오브젝트 Y축 위치 최대값
    public float objectsMaxY = 1.4f;
    
    // 오브젝트 최소 회전값
    public float objectsMinRotation = -45f;
    // 오브젝트 최대 회전값
    public float objectsMaxRotation = 45f;


    private int listIndexLength;

    private bool isDead;


    private void Start()
    {
        /* MainCamera 태그달린 오브젝트의 Camera 컴포넌트 의
           Projection 타입이 orthographic 일때 사이즈
           (값이 3.2 이면 위 아래 합쳐서 총 6.4 사이즈)
           그래서 실제 카메라 세로 크기를 구하기 위해 2를 곱하는것                                                             */
        float height = 2.0f * Camera.main.orthographicSize;       // height = 6.4f
        
        /* 16(가로):9(세로) 처럼 가로 세로 비율 설정하기
           aspect = 세로를 1로 기준을 두었을 때 가로 비율(16:9 이면 1.777:1)
           화면의 가로 크기를 구하기 위해 높이와 aspect 를 곱함(가로 크기 구하는 메소드가 없음)                                  */
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


    // 방 생성
    // GenerateRoomIfReauired() 안에서 사용됨
    private void AddRoom(float farthestRoomEndX)
    {
        // 방 프리팹 중에서 하나를 랜덤 선택
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        // 랜덤선택 된 방(룸) 오브젝트 추가
        GameObject room = Instantiate(availableRooms[randomRoomIndex]);
        
        // 방의 가로 크기 구하기
        float roomWidth = room.transform.Find(floor).localScale.x;
        
        /* 생성될 방의 위치
           방(룸) 오브젝트의 x 끝점 + 방(룸)의 가로크기 / 2                */
        float roomCenter = farthestRoomEndX + roomWidth / 2;
        
        // 생성될 방의 위치로 위치 시킴
        room.transform.position = new Vector3(roomCenter, 0, 0);

        // 추가한 방을 현재 추가된 방 리스트에 추가
        currentRooms.Add(room);
    }

    private void GenerateRoomIfReauired()
    {
        /* 삭제할 방의 목록을 저장하는 리스트
           바로 지우지 않고 리스트에 넣어놓고 한번에 지우려는것
           한 프레임 에 여러 룸이 삭제될수 있기에 리스트로 처리                    */
        List<GameObject> roomsToMove = new List<GameObject>();
        
        // 지금 프레임에 방을 생성할지 여부
        bool addRooms = true;
        
        // 마우스 오브젝트의 x 축 위치
        float playerX = transform.position.x;
        
        /* 플레이어 기준 스크린 가로길이 만큼 뒤로 보냄
           삭제할 방(룸) 의 기준 위치가 됨              */
        float removeRoomX = playerX - screenWidthInPoints;

        /* 플레이어 기준 스크린 가로 길이만큼 앞으로 보냄
           추가할 방(룸) 의 기준 위치가 됨(기준위치 에 방이없으면 생성 하기 위함)           */
        float addRoomX = playerX + screenWidthInPoints;

        // 현재 생성된 방(룸) 중 가장 오른쪽의 위치한 방의 오른쪽 끝 위치
        float farthestRoomEndX = 0f;


        //현재 생성된 룸 을 하나씩 처리
        foreach (var room in currentRooms)
        {
            /* 위에 floor = "Floor" 로 초기화 되어있음
               현재 처리 하고있는 방의 자식오브젝트 중에서 바닥오브젝트를 찾아 가로크기를 가져옴      */
            float roomWidth = room.transform.Find(floor).localScale.x;

            /* 방(룸) 의 중앙위치 에서 방(룸) 의 가로크기 의 절반을뺌
               (왼쪽 끝의 위치를 구함)                           */
            float roomStartX = room.transform.position.x - roomWidth / 2;

            /* 방(룸) 의 왼쪽 끝위치 에서 방(룸)의 크기만큼 더함
               (오른쪽 끝 위치를 구함)                           */
            float roomEndX = roomStartX + roomWidth;
            
            /* 방(룸) 마다 처리,
               방(룸)의 시작점(왼쪽 끝) 이 추가할 x 값 보다 오른쪽에있으면             */
            if (roomStartX > addRoomX)
                // 방을 추가 하지 않게 거짓으로 바꿈
                addRooms = false;

            // 방(룸)의 끝점(오른쪽 끝) 이 제거할 x 값 보다 왼쪽에 있으면
            if (roomEndX < removeRoomX)
                /* 방삭제 목록에 추가
                   바로지우지 않는 이유는 현재 currentRooms 리스트를
                   사용 중인데 중간에 수정해버리는 것이 되어버려 꼬일수 있음         */
                roomsToMove.Add(room);

            // 가장 오른쪽 방의 오른쪽 끝 위치를 최대값 메소드를 이용하여 구함
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        //삭제할 방 오브젝트를 하나씩 접근하면서
        foreach (var room in roomsToMove)
        {
            // 리스트에서 제거
            currentRooms.Remove(room);
            // 오브젝트 제거
            Destroy(room);
        }
        // 방을 추가해야한다면 방을 추가
        if (addRooms)
            AddRoom(farthestRoomEndX);
    }



    //오브젝트 생성
    private void AddObject(float lastObjectX)
    {
        // 추가할 오브젝트 의 랜덤 인덱스 값을 구함
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

        // 랜덤으로 구한 인덱스 번호의 오브젝트를 생성
        GameObject obj = Instantiate(availableObjects[randomIndex]);

        // 새로운 오브젝트의 X 축 위치를 계산
        float objectPositionX =
            lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);

        // 새로운 오브젝트의 Y 축 위치를 계산
        float randomY = Random.Range(objectsMinY, objectsMaxY);

        // 계산된 위치값을 오브젝트의 위치로 변경
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);

        // 랜덤으로 회전값 을 계산
        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);

        /* 랜덤으로 구해진 회전값으로 오브젝트를 쿼터니언 회전값 으로 계산하여 회전
           벡터값 만으로는 회전 하지못함(tranform.rotation 의 속성은 Quaternion 임)
           Quaternion.Euler 는 벡터값(x, y, z) 방식을 쿼터니언 값으로 변경해줌    */ 
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
        
        // 오브젝트 리스트에 추가
        objects.Add(obj);
    }

    private void GenerateObjectsIfRequired()
    {
        // 플레이어 X 축 위치 값
        float playerX = transform.position.x;

        /* 삭제할 오브젝트의 기준 위치
           플레이어 위치 기준 왼쪽으로 스크린 x축 길이            */
        float removeObjectX = playerX - screenWidthInPoints;
        
        /* 추가할 오브젝트의 기준 위치
           플레이어 기준 오른쪽으로 스크린 x 축의 길이            */
        float addObjectX = playerX + screenWidthInPoints;
        
        // 가장 오른쪽의 위치한 오브젝트 의 X축 위치
        float farthestObjectX = 0f;

        // 삭제할 오브젝트 를 담을 리스트
        List<GameObject> objectsToRemove = new List<GameObject>();

        // 현재 추가되어있는 오브젝트들 을 하나씩 처리
        foreach (var obj in objects)
        {
            // 오브젝트의 X 축 위치
            float objX = obj.transform.position.x;

            // 최대값 비교로 가장 오른쪽의 위치한 오브젝트 의 위치를 저장
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            // 오브젝트 위치가 삭제 기준 위치보다 왼쪽이면 
            if (objX < removeObjectX)
            {
                // 오브젝트 삭제 리스트에 추가
                objectsToRemove.Add(obj);
            }
        }

        // 삭제 리스트에 추가된 오브젝트를 모두 제거
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        // 가장 오른쪽에 위치한 오브젝트가 추가 기준위치 보다 왼쪽이면
        if (farthestObjectX < addObjectX)
        {
            // 오브젝트 추가 리스트에 넣기
            AddObject(farthestObjectX);
        }
    }



    /*// 피버타임
    IEnumerator FeverTime()
    {
        while (!isDead)
        {
            listIndexLength = availableObjects.Length;
            
            GameObject[] removeLaser;

            //피버 시작까지 걸리는 시간
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

            //피버 지속 시간
            yield return new WaitForSeconds(continueToFever);
            
            if (!isDead)
                Debug.Log("FeverEnd");
        }
    }
    */
}
        /* 이펙트 잠깐 호출할때
        IEnumerator FlashFire()
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
        }
        */
