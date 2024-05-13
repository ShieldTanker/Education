using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //�Ѿ� ������ ����(������)(�Ҹ� ���� ����)
    public GameObject smallBulletFactory;
    public GameObject bigBulletFactory;

    //źâ�� ���� �Ѿ� ����(������ƮǮ ���)
    public int poolSize;

    //ū�Ѿ� ������ƮǮ ������
    public int bigBulletPoolSize;

    // ������ƮǮ ����Ʈ
    public List<GameObject> smallBulletObjectPool;
    public List<GameObject> bigBulletObjectPool;


    //�ѱ� (�Ѿ� ���� ��ġ)
    public GameObject firePosition;

    
    private void Start()
    {
        //������ƮǮ ����Ʈ
        smallBulletObjectPool = InitBulletObjectPool(smallBulletFactory);
        bigBulletObjectPool = InitBulletObjectPool(bigBulletFactory);
    }


    
    private void Update()
    {
        //����Ƽ�����Ϳ� PC ȯ���� �� ����
        //# ���� �� �͵��� ��ó���� �����(��,��)
        //������ ������(����) �ϱ��� ����
#if UNITY_EDITOR || UNITY_STANDALONE

        //���� ����ڰ� �߻� ��ư�� ������
        
        if (Input.GetButtonDown("Fire1"))
            Fire(smallBulletObjectPool);

        if (Input.GetKeyDown(KeyCode.Space))
            Fire(bigBulletObjectPool);

#endif
    }

    //Inint �̴ϼȶ����� (�ʱ�ȭ) �� ���̾���
    List<GameObject> InitBulletObjectPool(GameObject bulletFactory)
    {
        List<GameObject> bulletObjectPool = new List<GameObject>();
       
        //źâ(������ƮǮ) �� ���� �Ѿ� ���� ��ŭ �ݺ�
        for (int i = 0; i < poolSize; i++)
        {
            // �Ѿ˰���(������) ���� �Ѿ�(������Ʈ)�� ����
            GameObject bullet = Instantiate(bulletFactory);

            //������ƮǮ ����Ʈ �� �Ѿ��� �ִ´�
            bulletObjectPool.Add(bullet);

            //��Ȱ��ȭ(ȭ�鿡 ����̶� �������� ����, ��Ȱ��ȭ �� ���·� �����ϱ�)
            bullet.SetActive(false);
        }
        return bulletObjectPool;
    }


    public void Fire(List<GameObject> objectPool)
    {
        if (objectPool.Count > 0)
        {
            GameObject bullet = objectPool[0];

            //�Ѿ� ��ġ ��Ű��
            bullet.transform.position = firePosition.transform.position;

            //�Ѿ��� Ȱ��ȭ ��Ų��
            bullet.SetActive(true);

            //������ƮǮ ���� �Ѿ� ����
            objectPool.Remove(bullet);
        }
    }
    public void SmallFire()
    {
        Fire(smallBulletObjectPool);
    }
}



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
    //������ƮǮ �迭

   public class PlayerFire : MonoBehaviour
{
    // �Ѿ� ������ ����(������)
    public GameObject bulletFactory;

    //źâ�� ���� �Ѿ� ����(������ƮǮ ���)
    public int poolSize;

    // ������ƮǮ �迭
    GameObject[] bulletObjectPool;


    //�ѱ� (�Ѿ� ���� ��ġ)
    public GameObject firePosition;


    private void Start()
    {
        //źâ(������ƮǮ �迭) �� �Ѿ�(������Ʈ)�� ������ �ִ� ũ��� �����
        bulletObjectPool = new GameObject[poolSize];

        // źâ�� ���� �Ѿ� ���� ��ŭ �ݺ�
        for (int i = 0; i < poolSize; i++)
        {
            // �Ѿ� ����(������) ���� �Ѿ��� ����
            GameObject bullet = Instantiate(bulletFactory);

            //�Ѿ�(������Ʈ��)�� źâ(������ƮǮ)�� �ִ´�
            bulletObjectPool[i] = bullet;
            
            //��Ȱ��ȭ(ȭ�鿡 ����̶� �������� ����
            //��Ȱ��ȭ �� ���·� ���� ��Ŵ
            bullet.SetActive(false);
        }
    }

    private void Update()
    {
        // ���� ����ڰ� �߻� ��ư�� ������
        if (Input.GetButtonDown("Fire1"))
        {
            // źâ �ȿ� �ִ� �Ѿ˵� �߿���
            for (int i = 0; i < poolSize; i++)
            {

                //������ƮǮ i�� �ε����� �ִ� ������Ʈ�� GameObject bullet ������ ����
                GameObject bullet = bulletObjectPool[i];


                // ���� �Ѿ��� ��Ȱ��ȭ �Ǿ��ٸ�
                if (bullet.activeSelf == false)
                {
                    // �Ѿ��� Ȱ��ȭ��Ų��.
                    bullet.SetActive(true);

                    // �Ѿ� ��ġ ��Ű��
                    bullet.transform.position = firePosition.transform.position;

                    // �Ѿ��� �߻��߱� ������ ��Ȱ��ȭ �Ѿ� �˻� �ߴ�
                    break;
                }
            }
        }
    }
}
*/

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//  ������ƮǮ ��� ���ҽ�
            /*//�Ѿ˰��忡�� �Ѿ��� �����
            // �ν��Ͻÿ���Ʈ�� ������ ������Ʈ�� �ٷ� ������ ����
            GameObject bullet = Instantiate(bulletFactory);

            //�Ѿ��� �ѱ���ġ�� �ű��
            bullet.transform.position = firePosition.transform.position;*/
