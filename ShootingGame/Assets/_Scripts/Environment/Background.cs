using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    //��� ���͸���
    public Material bgMaterial;

    //��ũ�� �ӵ�
    public float scrollSpeed;

    private void Update()
    {
        //��ũ�� ����
        Vector2 difrection = Vector2.up;

        //��ũ��
        bgMaterial.mainTextureOffset += difrection * scrollSpeed * Time.deltaTime;
    }
}