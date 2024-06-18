using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private NetworkRunner _networkRunnerPrefab = null;
    [SerializeField] private PlayerData _playerDataPrefab = null;
    [SerializeField] private InputField _roomName = null;
    [SerializeField] private string _gameSceneName = null;

    // 오브젝트가 생성되고 그 객체를 저장할 변수
    private NetworkRunner _runnerInstance = null;

    // 유저 아이디 변수
    public InputField id;

    // 유저 패스워드 변수
    public InputField password;

    // 검사 텍스트 변수
    public Text notify;

    private void Start()
    {
        // 검사 텍스트 창을 비움
        notify.text = "";
    }

    public void StartSharedSession()
    {
        // 룸네임이 비어있거나 null 이면 BasicRoom
        string roomName = string.IsNullOrEmpty(_roomName.text) ? "BasicRoom" : _roomName.text;

        SetPlayerData();
        StartGame(GameMode.Shared, roomName, _gameSceneName);
    }

    private void SetPlayerData()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();

        // 오브젝트가 없으면 생성
        if (playerData == null)
            playerData = Instantiate(_playerDataPrefab);

        // 로그인 할때 입력한 아이디를 플레이어 데이타에 저장
        playerData.UserId = id.text;
    }

    // async : 비동기식 (안에 await 있음, 결과를 기다리는동안 다른 코드 실행가능)
    private async void StartGame(GameMode mode, string roomName, string sceneName)
    { // GameMode(포톤) : 서버모드,쉐어드모드,호스트 모드등 설정
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        
        // 네트워크 러너 객체 없으면 생성
        if (_runnerInstance == null)
            _runnerInstance = Instantiate(_networkRunnerPrefab);

        // 입력 권한(보통 트루, 관전만 하면 필요없음)
        _runnerInstance.ProvideInput = true;

        // 참조해야하는 변수들을 하나로 묶음
        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            // 플레이어 수 제한
            PlayerCount = 2,
        };

        // 스타트 게임 매소드 실행결과를 기다림
        StartGameResult res = await _runnerInstance.StartGame(startGameArgs);
        // 접속 실패시
        if (!res.Ok)
        {
            // 오류 이유가 방이 가득찬경우
            if (res.ShutdownReason == ShutdownReason.GameIsFull)
                notify.text = "방이 가득 찼습니다";
            else
                // 아닐결우 해당 오류를 그대로 출력
                notify.text = res.ShutdownReason.ToString();
            return;
        }

        // 실행 결과를 받으면 씬전환(SetActiveScene 을 사용해야 내가 접속했다는 정보를 넘김)
        _runnerInstance.SetActiveScene(sceneName);
    }

    // 아이디와 패스워드 저장 함수
    public void SaveUserData()
    {
        // 만일 입력 검사에 문제가 있으면 함수를 종료
        if (!CheckInput(id.text, password.text))
            return;

        // 만일 시스템에 저장돼 있는 아이디가 존재하지 않는다면
        if (!PlayerPrefs.HasKey(id.text))
        {
            // 사용자의 아이디는 키(key)로 패스워드를 값(value)으로 설정해 저장
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "아이디 생성이 완료됐습니다.";
        }
        // 그렇지 않으면, 이미 존재한다는 메시지를 출력
        else
        {
            notify.text = "이미 존재하는 아이디입니다.";
        }
    }

    // 로그인 함수
    public void CheckUserData()
    {
        // 만일 입력 검사에 문제가 있으면 함수를 종료
        if (!CheckInput(id.text, password.text))
            return;

        // 사용자가 입력한 아이디를 키로 사용해 시스템에 저장된 값을 불러옴
        string pass = PlayerPrefs.GetString(id.text);

        // 만일, 사용자가 입력한 패스워드와 시스템에서 불러온 값을 비교해서 동일하다면
        if (password.text == pass)
        {
            // 다음 씬(1번 씬)을 로드
            // SceneManager.LoadScene(1);
            StartSharedSession();
        }
        // 그렇지 않고 두 데이터의 값이 다르면, 유저 정보 불일치 메시지를 남김
        else
        {
            notify.text = "입력하신 아이디와 패스워드가 일치하지 않습니다.";
        }
    }

    // 입력 완료 확인 함수
    bool CheckInput(string id, string pwd)
    {
        // 만일, 입력란이 하나라도 비어 있으면 유저 정보 입력을 요구
        if (id == "" || pwd == "")
        {
            notify.text = "아이디 또는 패스워드를 입력해주세요.";
            return false;
        }
        // 입력이 비어 있지 않으면 true를 반환
        else
        {
            return true;
        }
    }
}
