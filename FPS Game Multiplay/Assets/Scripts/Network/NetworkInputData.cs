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
{// 입력권한이 있는 오브젝트를 가지고 다른쪽에 정보를 전파 
    public Vector3 dir;

    // 버튼이 눌려있다 안눌려있다 
    public NetworkButtons Buttons;

    // 마우스 회전값 변수
    public float mx;
}
