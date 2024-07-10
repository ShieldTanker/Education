using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using System.Collections;
using UnityEngine.Networking;
using System;

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

    public Text userList;

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
            PlayerCount = 4,
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

        StartCoroutine(JoinDataPost(id.text, password.text));

/*        // ���� �ý��ۿ� ����� �ִ� ���̵� �������� �ʴ´ٸ�
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
        }*/
    }

    IEnumerator JoinDataPost(string id, string password)
    {
        string url = "http://127.0.0.1/fps_game/join.php";

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", id);
        form.AddField("passwordPost", password);
        // ��ü ���� �� Post �� �ֱ�
        using(UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // ��ü�� ������ ���� ��û
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                // ������ ������
                switch (www.downloadHandler.text)
                {
                    case "success":
                        notify.text = "���̵� ������ �Ϸ� �Ǿ����ϴ�";
                        break;
                    case "already exist":
                        notify.text = "�̹� �����ϴ� ���̵� �Դϴ�.";
                        break;
                    case "fail":
                        notify.text = "���̵� ������ ���� �߽��ϴ�.";
                        break;
                    default:
                        notify.text = "�� �� ���� ����";
                        break;
                }
            }
            else
            {
                notify.text = "�α��� ���� ���� ����";
            }
        }
    }

    // �α��� �Լ�
    public void CheckUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� ����
        if (!CheckInput(id.text, password.text))
            return;

        StartCoroutine(LoginDataPost(id.text, password.text));


/*        // ����ڰ� �Է��� ���̵� Ű�� ����� �ý��ۿ� ����� ���� �ҷ���
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
        }*/
    }

    IEnumerator LoginDataPost(string id, string password)
    {
        // url �ּ� ������ ���� ����(������ Ȥ�� ������ �ּ�/���/����)
        string url = "http://127.0.0.1/fps_game/login.php";

        WWWForm form = new WWWForm();
        // form ��ü�� �ֵ��ʵ� �ؼ� �̸��� ������� ����
        form.AddField("usernamePost", id);
        form.AddField("passwordPost", password);
        form.AddField("datePost", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

        // using : �ȿ��� ������ ��ü�� ������� Dispos(���ϴݱ�) �� ȣ���
        // UnityWebRequest.Post(url : ���� �ּ�, form : ���� ������)
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // ������ �ö����� �ڵ� ����
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                // downloadHandler.text : ���ͳ� Ŭ���̾�Ʈ �� �������� ȭ��(php �� echo ������)
                Debug.Log(www.downloadHandler.text);

                switch (www.downloadHandler.text)
                {
                    case "login success":
                        StartSharedSession();
                        break;
                    case "password incorrect":
                        notify.text = "�߸��� ��й�ȣ";
                        break;
                    case "user not found":
                        notify.text = "����� ����";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.Log("error");
                notify.text = "�α��� ����";
            }
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

    public void UserList()
    {
        StartCoroutine(UserListPost());
    }

    IEnumerator UserListPost()
    {
        string url = "http://127.0.0.1/fps_game/user_list.php";
        WWWForm form = new WWWForm();
        // ������ �߰����� ���� ����
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            // ������ ������
            if(www.error == null)
            {
                // ����Ƽ ���� ���Ǵ� json ���� : {\"Items(UsersŬ������ �ִ� ���� �̸��� ����)\" + ������ ���̴� json �ڵ� + "}"
                //  "{\"Items\":" + [{"id":"1","username":"user1"},{"id":"2","username":"user2"},{"id":"3","username":"user3"}] + "}"
                string jsonStr = "{\"Items\":" + www.downloadHandler.text + "}";
                
                // json ������ ��ü�� �ٲ���
                Users users = JsonUtility.FromJson<Users>(jsonStr);

                string userStr = "";

                foreach (User user in users.Items)
                {
                    userStr += $"ID : {user.id}, Name : {user.username}\n";
                }
                userList.text = userStr;
            }
        }
    }
}
// ���� ���� ����°� ����(���ǻ� ���⼭ ����)
[System.Serializable]
public class User
{
    public int id;
    public string username;
}
public class Users
{
    public User[] Items;
}
