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

    // ������Ʈ�� �����ǰ� �� ��ü�� ������ ����
    private NetworkRunner _runnerInstance = null;

    // ���� ���̵� ����
    public InputField id;

    // ���� �н����� ����
    public InputField password;

    // �˻� �ؽ�Ʈ ����
    public Text notify;

    private void Start()
    {
        // �˻� �ؽ�Ʈ â�� ���
        notify.text = "";
    }

    public void StartSharedSession()
    {
        // ������� ����ְų� null �̸� BasicRoom
        string roomName = string.IsNullOrEmpty(_roomName.text) ? "BasicRoom" : _roomName.text;

        SetPlayerData();
        StartGame(GameMode.Shared, roomName, _gameSceneName);
    }

    private void SetPlayerData()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();

        // ������Ʈ�� ������ ����
        if (playerData == null)
            playerData = Instantiate(_playerDataPrefab);

        // �α��� �Ҷ� �Է��� ���̵� �÷��̾� ����Ÿ�� ����
        playerData.UserId = id.text;
    }

    // async : �񵿱�� (�ȿ� await ����, ����� ��ٸ��µ��� �ٸ� �ڵ� ���డ��)
    private async void StartGame(GameMode mode, string roomName, string sceneName)
    { // GameMode(����) : �������,�������,ȣ��Ʈ ���� ����
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        
        // ��Ʈ��ũ ���� ��ü ������ ����
        if (_runnerInstance == null)
            _runnerInstance = Instantiate(_networkRunnerPrefab);

        // �Է� ����(���� Ʈ��, ������ �ϸ� �ʿ����)
        _runnerInstance.ProvideInput = true;

        // �����ؾ��ϴ� �������� �ϳ��� ����
        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            // �÷��̾� �� ����
            PlayerCount = 2,
        };

        // ��ŸƮ ���� �żҵ� �������� ��ٸ�
        StartGameResult res = await _runnerInstance.StartGame(startGameArgs);
        // ���� ���н�
        if (!res.Ok)
        {
            // ���� ������ ���� ���������
            if (res.ShutdownReason == ShutdownReason.GameIsFull)
                notify.text = "���� ���� á���ϴ�";
            else
                // �ƴҰ�� �ش� ������ �״�� ���
                notify.text = res.ShutdownReason.ToString();
            return;
        }

        // ���� ����� ������ ����ȯ(SetActiveScene �� ����ؾ� ���� �����ߴٴ� ������ �ѱ�)
        _runnerInstance.SetActiveScene(sceneName);
    }

    // ���̵�� �н����� ���� �Լ�
    public void SaveUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� ����
        if (!CheckInput(id.text, password.text))
            return;

        // ���� �ý��ۿ� ����� �ִ� ���̵� �������� �ʴ´ٸ�
        if (!PlayerPrefs.HasKey(id.text))
        {
            // ������� ���̵�� Ű(key)�� �н����带 ��(value)���� ������ ����
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "���̵� ������ �Ϸ�ƽ��ϴ�.";
        }
        // �׷��� ������, �̹� �����Ѵٴ� �޽����� ���
        else
        {
            notify.text = "�̹� �����ϴ� ���̵��Դϴ�.";
        }
    }

    // �α��� �Լ�
    public void CheckUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� ����
        if (!CheckInput(id.text, password.text))
            return;

        // ����ڰ� �Է��� ���̵� Ű�� ����� �ý��ۿ� ����� ���� �ҷ���
        string pass = PlayerPrefs.GetString(id.text);

        // ����, ����ڰ� �Է��� �н������ �ý��ۿ��� �ҷ��� ���� ���ؼ� �����ϴٸ�
        if (password.text == pass)
        {
            // ���� ��(1�� ��)�� �ε�
            // SceneManager.LoadScene(1);
            StartSharedSession();
        }
        // �׷��� �ʰ� �� �������� ���� �ٸ���, ���� ���� ����ġ �޽����� ����
        else
        {
            notify.text = "�Է��Ͻ� ���̵�� �н����尡 ��ġ���� �ʽ��ϴ�.";
        }
    }

    // �Է� �Ϸ� Ȯ�� �Լ�
    bool CheckInput(string id, string pwd)
    {
        // ����, �Է¶��� �ϳ��� ��� ������ ���� ���� �Է��� �䱸
        if (id == "" || pwd == "")
        {
            notify.text = "���̵� �Ǵ� �н����带 �Է����ּ���.";
            return false;
        }
        // �Է��� ��� ���� ������ true�� ��ȯ
        else
        {
            return true;
        }
    }
}
