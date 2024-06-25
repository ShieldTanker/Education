using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, ISpawned
{
    public NetworkPrefabRef playerNetworkPrefab;
    public Transform[] spawnPoints;

    // 각 프로그램 마다 씬 올때 스폰됨
    // 해당 씬이 불러와질때 세션에 생성되는 자신 오브젝트 정보를 매개변수로 사용
    public void Spawned()
    {// 세션에 생성되는 오브젝트.자신 오브젝트
        SpawnPlayer(Runner.LocalPlayer);
    }

    public void SpawnPlayer(PlayerRef player)
    { // 자신 오브젝트 순서 % 스폰포인트 배열 크기(자신 오브젝트 순서는 세션에 있는 사람수 만큼 카운트 됨)
        int index = player % spawnPoints.Length;
        var spawnPosition = spawnPoints[index].position;

        // 생성 시킬때 인스턴시에이트 가 아닌 Runner.Spawn() 으로 해야함
        var playerObject = Runner.Spawn(
            playerNetworkPrefab, spawnPosition, Quaternion.identity, player);
        //  생성할 프리팹, 생성할 위치, 회전값, 자신오브젝트 정보

        // 플레이어 오브젝트 설정 (세션에 생성된 자신오브젝트 정보, 생성된 오브젝트)
        Runner.SetPlayerObject(player, playerObject);
    }
}
