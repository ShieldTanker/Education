using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상속 필요없으니 MonoBehavior 제거
public class User
{
    public string name;
    public float score;

    public User(string name, float score)
    {
        this.name = name;
        this.score = score;
    }
}
