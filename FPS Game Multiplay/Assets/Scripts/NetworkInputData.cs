using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

enum PlayerButtons
{
    Jump = 0,
}

public struct NetworkInputData : INetworkInput
{// �Է±����� �ִ� ������Ʈ�� ������ �ٸ��ʿ� ������ ���� 
    public Vector3 dir;

    // ��ư�� �����ִ� �ȴ����ִ� 
    public NetworkButtons Buttons;
}
