using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaTime : MonoBehaviour
{
    public Text playTime;
    float timeCount = 0;

    private void Update()
    {
        timeCount += Time.deltaTime;
        playTime.text = "Play Time : " + timeCount;
    }
}
