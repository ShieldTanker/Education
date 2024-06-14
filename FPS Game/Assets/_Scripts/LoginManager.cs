using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
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

    // ���̵� �� �н����� ���� �Լ�
    public void SaveUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� ����
        if (!CheckInput(id.text,password.text))
            return;

        // ���� �ý��ۿ� ����Ǿ� �ִ� ���̵� �������� �ʴ´ٸ�
        if (!PlayerPrefs.HasKey(id.text))
        {
            // ������� ���̵�� Ű(key)�� �н����带 ��(value)���� ����
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "���̵� ������ �Ϸ� �Ǿ����ϴ�";
        }
        // �׷��� ������
        else
        {
            notify.text = "�̹� �����ϴ� ���̵� �Դϴ�.";
        }
    }

    // �α��� �Լ�
    public void CheckUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� ����
        if (!CheckInput(id.text, password.text))
            return;

        // ����ڰ� �Է��� ���̵� Ű(key)�� ����� �ý��ۿ� ����� ���� �ҷ���
        string pass = PlayerPrefs.GetString(id.text);

        // ����, ����ڰ� �Է��� �н������ �ý��ۿ��� �ҷ��� ���� ���ؼ� �����ϴٸ�
        if (password.text == pass)
        {
            // ���� ��(1�� ��)�� �ε�
            SceneManager.LoadScene(1);
        }
        // �׷��� �ʰ� �� �������� ���� �ٸ���, ���� ���� ����ġ �޽����� ����
        else
        {
            notify.text = "�Է��Ͻ� ���̵� �Ǵ� ��й�ȣ �� ��ġ���� �ʽ��ϴ�.";
        }
    }

    bool CheckInput(string id, string pwd)
    {
        // ����, �Է¶��� �ϳ��� ��� ������ ���� ���� �Է��� �䱸
        if (id == "" || pwd == "")
        {
            notify.text = "���̵� �Ǵ� ��й�ȣ�� �Է����ּ���.";
            return false;
        }
        // �Է��� ������� ������
        else
            return true;
    }
}