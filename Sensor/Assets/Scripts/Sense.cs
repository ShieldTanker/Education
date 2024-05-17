using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    // ����� ��� ���� Ȯ�ο� ����
    public bool bDebug;

    // Aspect �츮�� ���� Ŭ����(�÷��̾� ���� ������ Ȯ�ο�)
    public Aspect.AspectName aspectName;

    // �� �� �ֱ�� Ȯ�� �� ����
    public float detectionRate;


    // protected : �ڽ� Ŭ�������� ����
    // �ð� ���ϱ� ���� ����
    protected float elapsedTime;

    // �ڽ� Ŭ�������� Start()�޼ҵ� ������ ���
    // �޼ҵ带 �ڽ�Ŭ�������� �ϳ� �ϳ� ���� ��ŸƮ �޼ҵ带 �����ص� �Ǵµ�
    // �׷��� ���Ŭ���� ����°� �ǹ� ����(�������� �θ� Ŭ������ �־���� ����)
    
    // virtual(����) ���� �޼ҵ带 ���� �ڽĿ��� ���
    // �������� ���� ������ �ڽ� ������Ʈ���� �޼ҵ� ������ �ٸ��� ����
    protected virtual void Initialise() { }

    protected virtual void UpdateSense() { }

    private void Start()
    {
        elapsedTime = 0f;
        Initialise();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > detectionRate)
        {
            UpdateSense();
            elapsedTime = 0f;
        }
    }
}
