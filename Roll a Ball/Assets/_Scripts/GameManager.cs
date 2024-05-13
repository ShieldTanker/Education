using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TextMeshPro ���� TMPro �� ����ؾ���
using TMPro;

public class GameManager : MonoBehaviour
{
    public int count;
    public int score;

    // TextMeshPro ������Ʈ�� TMP_Text �� �Ǿ�����
    public TMP_Text countText;
    public TMP_Text lessText;
    public TMP_Text pointText;
    public TMP_Text winText;

    private int totalCount;
    private int lessCount;
    private GameObject[] pickUps;
    private PlayerController player;

    private void Start()
    {
        //������ ����
        Application.targetFrameRate = 30;

        //Ƚ�� �� ���� �ʱ�ȭ
        count = 0;
        score = 0;

        pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        totalCount = pickUps.Length;

        SetCountText();
        //���ڰ� ���� �Ⱥ���
        winText.text = "";
        
    }

    public void SetCountText()
    {
        countText.text = "Count : " + count;
        
        lessCount = totalCount - count;
        lessText.text = "Less : " + lessCount;

        pointText.text = "Point : " + score;

        if (count >= pickUps.Length)
        {
            player.finish = true;
            winText.text = "! You Win !";
        }
    }

}
