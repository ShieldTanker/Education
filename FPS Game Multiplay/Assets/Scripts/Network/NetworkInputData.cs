using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

enum PlayerButtons
{
    Jump = 0,
    Fire0 = 1,
    Fire1 = 2,
}

public struct NetworkInputData : INetworkInput
{// �Է±����� �ִ� ������Ʈ�� ������ �ٸ��ʿ� ������ ���� 
    public Vector3 dir;

    // ��ư�� �����ִ� �ȴ����ִ� 
    public NetworkButtons Buttons;

    // ���콺 ȸ���� ����
    public float mx;
}
