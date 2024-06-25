using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, ISpawned
{
    public NetworkPrefabRef playerNetworkPrefab;
    public Transform[] spawnPoints;

    // �� ���α׷� ���� �� �ö� ������
    // �ش� ���� �ҷ������� ���ǿ� �����Ǵ� �ڽ� ������Ʈ ������ �Ű������� ���
    public void Spawned()
    {// ���ǿ� �����Ǵ� ������Ʈ.�ڽ� ������Ʈ
        SpawnPlayer(Runner.LocalPlayer);
    }

    public void SpawnPlayer(PlayerRef player)
    { // �ڽ� ������Ʈ ���� % ��������Ʈ �迭 ũ��(�ڽ� ������Ʈ ������ ���ǿ� �ִ� ����� ��ŭ ī��Ʈ ��)
        int index = player % spawnPoints.Length;
        var spawnPosition = spawnPoints[index].position;

        // ���� ��ų�� �ν��Ͻÿ���Ʈ �� �ƴ� Runner.Spawn() ���� �ؾ���
        var playerObject = Runner.Spawn(
            playerNetworkPrefab, spawnPosition, Quaternion.identity, player);
        //  ������ ������, ������ ��ġ, ȸ����, �ڽſ�����Ʈ ����

        // �÷��̾� ������Ʈ ���� (���ǿ� ������ �ڽſ�����Ʈ ����, ������ ������Ʈ)
        Runner.SetPlayerObject(player, playerObject);
    }
}
