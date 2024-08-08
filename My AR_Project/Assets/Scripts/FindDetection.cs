using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;
using UnityEngine.UI;


public class FindDetection : MonoBehaviour
{
    public GameObject smallCube;
    List<GameObject> faceCubes = new List<GameObject>();

    public Text vertexIndex;

    public ARFaceManager afm;
    private ARCoreFaceSubsystem subSys;

    // Ư�� ������� �����ҽ� �޸� ��� ����ȭ
    NativeArray<ARCoreFaceRegionData> regionData;

    void Start()
    {
        // ��ġ ǥ�ø� ���� ���� ť�� 3���� ����
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(smallCube);
            faceCubes.Add(go);
            go.SetActive(false);
        }

        // AR Face Manager �� ���� �ν��� �� ������ �Լ��� ����
        // afm.facesChanged += OnDetectThreePoints;
        afm.facesChanged += OnDetectFaceAll;

        // AR Foundation�� XRFaceSubsystem Ŭ���� ������
        // AR Core�� ARCoreFaceSubsystem Ŭ���� ������ ĳ����
        subSys = (ARCoreFaceSubsystem)afm.subsystem;
    }
    void OnDetectFaceAll(ARFacesChangedEventArgs args)
    {
        // ���� �ν����� ������
        if (args.updated.Count > 0)
        {
            // �ؽ�Ʈ UI�� ���� ���ڿ� �����͸� ������ �����ͷ� ��ȯ
            // ��������Ʈ�� �޼ҵ� �־���Ұ� �������Ʈ ȣ���� �ٸ��ʿ��� ȣ����
            int num = int.Parse(vertexIndex.text);

            // �� ���� �迭���� ������ �ε����� �ش��ϴ� ��ǥ�� ������
            // �� 468 ���� ���ؽ��� ����
            Vector3 vertPosition = args.updated[0].vertices[num];

            // ���� ��ǥ�� ���� ��ǥ�� ��ȯ
            vertPosition = args.updated[0].transform.TransformPoint(vertPosition);

            // �غ�� ť�� �ϳ��� Ȱ��ȭ �ϰ�, ���� ��ġ�� ������ ����
            faceCubes[0].SetActive(true);
            faceCubes[0].transform.position = vertPosition;
        }
        else if (args.removed.Count > 0)
        {
            faceCubes[0].SetActive(false);
        }
    }
    // facesChanged ��������Ʈ(Action)�� ������ �Լ�
    void OnDetectThreePoints(ARFacesChangedEventArgs args)
    {
        // �� �ν� ������ ���ŵ� ���� �ִٸ�
        if (args.updated.Count > 0)
        {
            // �νĵ� �󱼿��� Ư�� ��ġ�� ������
            subSys.GetRegionPoses(
                // update : ����Ȱ��� �������ϼ� �־� �迭
                args.updated[0].trackableId,
                // Persistent ������� regionData �� ����
                // Persistent : ���� ���� �Ҵ������� ���ø����̼� �ֱ⿡ ���� �ʿ��Ѹ�ŭ ���� ����
                Allocator.Persistent, ref regionData);

            // �νĵ� ���� Ư�� ��ġ(0: �ڳ�, 1:�̸� ����, 2: �̸� ����) �� ������Ʈ ��ġ
            for (int i = 0; i < regionData.Length; i++)
            {
                faceCubes[i].transform.position = regionData[i].pose.position;
                faceCubes[i].transform.rotation = regionData[i].pose.rotation;
                faceCubes[i].SetActive(true);
            }
        }
        // �� �ν� ������ �Ҿ��ٸ�
        else if (args.removed.Count > 0)       
        {
            // ������Ʈ ��Ȱ��ȭ
            for (int i = 0; i < regionData.Length; i++)
            {
                faceCubes[i].SetActive(false);
            }
        }
    }
}
