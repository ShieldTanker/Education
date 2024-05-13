using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void Start()
    {
        int age = 30;
        if (age >= 30 && age <= 39)
        {
            Debug.Log("30대 입니다.");
        }
        else 
        {
            Debug.Log("30대가 아닙니다");
        }
    }
}
