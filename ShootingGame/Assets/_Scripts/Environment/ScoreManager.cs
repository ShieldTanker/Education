using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    //�̱��� ��ü (Awake ������ Instance ������ �ڱ� �ڽ��� �Ҵ�)
    public static ScoreManager Instance = null;

    //Awake �� Start ���� ���� �����
    private void Awake()
    {
        // �̱��� ��ü�� ���� ������ ������ �ڱ� �ڽ��� �Ҵ�
        if (Instance == null)
            //�ڱ� �ڽ� ��ü�� �ǹ�
            Instance = this;
    }

    //�Ӽ�
    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            //ScoreManager Ŭ������ �Ӽ��� ���� �Ҵ� �Ѵ�
            currentScore = value;

            //���� ȭ�鿡 ���� ǥ��
            currentScoreUI.text = "���� ���� : " + currentScore;

            //���� ���� ������ �ְ� ������ �ʰ��Ͽ��ٸ�
            if (currentScore > bestScore)
            {
                //�ְ����� ����
                bestScore = currentScore;

                //�ְ����� ȭ�� ���
                bestScoreUI.text = "�ְ� ���� : " + bestScore;

                //�ְ������� ����
                //                 (    Key     ,    Value   )
                PlayerPrefs.SetInt("Best Score", bestScore);
                //��ũ�� ����(�������� ��Ƽ� �ѹ��� �ϴ°� ����)
                //������ ���� ������ ���� �ǰ� �Ϸ��� �̹�� ���
                PlayerPrefs.Save();
            }
        }
    }

    //���� ���� UI
    public Text currentScoreUI;

    //�ְ� ����UI
    public Text bestScoreUI;

    //�ְ� ����
    public int bestScore;



    //���� ����(ĸ��ȭ)
    private int currentScore;

    private void Start()
    {
        //�ְ������� �ҷ��ͼ� bestScore �� �־��ֱ�
        //�ҷ��ö��� GetInt,GetFloat,GetString ���� Key �ҷ�����
        // GetInt("Best Score", 0) �� �ǹ̴� Best Score �� ���� ������ �⺻������ 0 ���
        bestScore = PlayerPrefs.GetInt("Best Score", 0);

        //�ְ����� ȭ�� ǥ��
        bestScoreUI.text = "�ְ����� : " + bestScore;
    }


///////////////////////////////////////////////////////////////////////////////////////////////////////

/*
     public class ScoreManager : MonoBehaviour
{

    // �̱��� ��ü
    public static ScoreManager Instance = null;

    // �̱��� ��ü�� ���� ������ ������ �ڱ� �ڽ��� �Ҵ�
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    // �Ӽ�
    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            // ScoreManager Ŭ������ �Ӽ��� ���� �Ҵ� �Ѵ�.
            currentScore = value;

            // ȭ�鿡 ���� ���� ǥ���ϱ�
            currentScoreUI.text = "�������� : " + currentScore;

            // ���� ���� ������ �ְ� ������ �ʰ��Ͽ��ٸ�
            if (currentScore > bestScore)
            {
                // �ְ� ������ ���� ��Ų��.
                bestScore = currentScore;

                // �ְ� ������ UI�� ǥ��
                bestScoreUI.text = "�ְ����� : " + bestScore;

                // �ְ������� ����
                PlayerPrefs.SetInt("Best Score", bestScore);
                PlayerPrefs.Save();
            }
        }
    }

    // ���� ���� UI
    public Text currentScoreUI;

    // ���� ����
    private int currentScore;

    // �ְ� ���� UI
    public Text bestScoreUI;

    // �ְ� ����
    public int bestScore;

    private void Start()
    {
        // �ְ������� �ҷ��ͼ� bestScore�� �־��ֱ�
        bestScore = PlayerPrefs.GetInt("Best Score", 0);
        // �ְ����� ȭ�鿡 ǥ���ϱ�
        bestScoreUI.text = "�ְ����� : " + bestScore;
    }
}
*/

//////////////////////////////////////////////////////////////////////////////////////////////////////////

/*

        ���� �Ӽ� �ϱ����� �̷���� �̾���

        currentScore�� ���� �ְ� ȭ�鿡 ǥ���ϱ�

       public void SetScore(int value)
    {
        //ScoreManager Ŭ������ �Ӽ��� ���� �Ҵ� �Ѵ�
        currentScore = value;

        //���� ȭ�鿡 ���� ǥ��
        currentScoreUI.text = "���� ���� : " + currentScore;

        //���� ���� ������ �ְ� ������ �ʰ��Ͽ��ٸ�
        if (currentScore > bestScore)
        {
            //�ְ����� ����
            bestScore = currentScore;

            //�ְ����� ȭ�� ���
            bestScoreUI.text = "�ְ� ���� : " + bestScore;

            //�ְ������� ����
            //                 (    Key     ,    Value   )
            PlayerPrefs.SetInt("Best Score", bestScore);
            //��ũ�� ����(�������� ��Ƽ� �ѹ��� �ϴ°� ����)
            //������ ���� ������ ���� �ǰ� �Ϸ��� �̹�� ���
            PlayerPrefs.Save();
        }

    }
*/

    //current �� ��������
/*
       public int GetScore()
    {
        return currentScore;
    }
*/
}
