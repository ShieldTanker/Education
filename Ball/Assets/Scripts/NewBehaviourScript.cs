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
            Debug.Log("30�� �Դϴ�.");
        }
        else 
        {
            Debug.Log("30�밡 �ƴմϴ�");
        }
    }
}
