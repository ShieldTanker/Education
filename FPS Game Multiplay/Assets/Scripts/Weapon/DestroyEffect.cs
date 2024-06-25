using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class DestroyEffect : NetworkBehaviour
{
    // ���ŵ� �ð� ����
    public float destroyTime = 1.5f;

    // ��� �ð� ������ ����
    float currentTime = 0;

    // ���� �ϴ°� ������ FixedUpdateNetwork ��� �ؾ� ������
    // ���� �� �ϰ� �ð��� ��� �Ƚᵵ ��
    private void Update()
    {
        // ���� ��� �ð��� ���ŵ� �ð��� �ʰ��ϸ� �ڱ� �ڽ��� ����
        if (currentTime > destroyTime)
        {
            // Object : ǻ������ ó���ǰ��ִ� ������Ʈ Ÿ��
            Runner.Despawn(Object);
        }
        // ��� �ð��� ����
        currentTime += Time.deltaTime;
    }
}
