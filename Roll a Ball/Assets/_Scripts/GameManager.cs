using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TextMeshPro 사용시 TMPro 로 사용해야함
using TMPro;

public class GameManager : MonoBehaviour
{
    public int count;
    public int score;

    // TextMeshPro 컴포넌트는 TMP_Text 로 되어있음
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
        //프레임 제한
        Application.targetFrameRate = 30;

        //횟수 및 점수 초기화
        count = 0;
        score = 0;

        pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        totalCount = pickUps.Length;

        SetCountText();
        //글자가 없어 안보임
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
