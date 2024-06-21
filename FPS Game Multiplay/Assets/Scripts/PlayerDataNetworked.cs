using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerDataNetworked : NetworkBehaviour
{
    public string UserId { get; private set; }

    public override void Spawned()
    {// 스타트 와 비슷함, 오브젝트가 스폰될때 호출됨
        if (Object.HasStateAuthority)
        {// Object : 네트워크용 오브젝트
         // HasStateAuthority : 상태권한 이 있는지
            UserId = FindObjectOfType<PlayerData>().UserId;
        }

        GameManager.gm.AddPlayer(gameObject);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        GameManager.gm.RemovePlayer(gameObject);
    }
}
