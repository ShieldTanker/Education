using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

public class LocalInputPoller : MonoBehaviour, INetworkRunnerCallbacks
{
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    // 입력이 있을때 자동으로 호출
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (GameManager.gm == null || GameManager.gm.pr == null)
            return;

        // NetworkInputData : 우리가 만든 구조체
        NetworkInputData localInput = new NetworkInputData();

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;  // 대각선 입력을 정규화
        dir = Camera.main.transform.TransformDirection(dir);  // 메인카메라가 바라보는 방향을 기준으로 새로 계산
        dir = new Vector3(dir.x, 0, dir.z);

        localInput.dir = dir; // 구조체에 dir 값 넣기

        // 버튼 세팅(열거형 Jump = 0, Input.GetButton("Jump") = bool)
        localInput.Buttons.Set(PlayerButtons.Jump, Input.GetButton("Jump"));
        // GetMouseButtonDown()은 Update() 에서 사용하는거라 여기에 쓰면 안됨
        localInput.Buttons.Set(PlayerButtons.Fire0, Input.GetMouseButton(0));
        localInput.Buttons.Set(PlayerButtons.Fire1, Input.GetMouseButton(1));

        localInput.mx = GameManager.gm.pr.mx;

        // 매개변수가 참조형이라서 값이 변함
        input.Set(localInput);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}
