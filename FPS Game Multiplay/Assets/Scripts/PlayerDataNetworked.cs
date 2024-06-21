using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerDataNetworked : NetworkBehaviour
{
    public string UserId { get; private set; }

    public override void Spawned()
    {// ��ŸƮ �� �����, ������Ʈ�� �����ɶ� ȣ���
        if (Object.HasStateAuthority)
        {// Object : ��Ʈ��ũ�� ������Ʈ
         // HasStateAuthority : ���±��� �� �ִ���
            UserId = FindObjectOfType<PlayerData>().UserId;
        }

        GameManager.gm.AddPlayer(gameObject);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        GameManager.gm.RemovePlayer(gameObject);
    }
}
