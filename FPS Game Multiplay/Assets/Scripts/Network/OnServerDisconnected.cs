using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class OnServerDisconnected : MonoBehaviour, INetworkRunnerCallbacks
{
    // 접속 끊겼을때 이동할 씬 이름
    [SerializeField] private string _loginSceneName;
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    // 서버와의 접속이 끊길때
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        // 접속을 종료 시킴
        GetComponent<NetworkRunner>().Shutdown();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    // 셧다운 되었을때
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene(_loginSceneName);
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}
