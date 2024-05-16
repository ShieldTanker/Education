using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aspect : MonoBehaviour
{
    // 태그처럼 카테고리 만들기
    public enum AspectName
    {
        Player,
        Enemy
    }

    public AspectName aspectName;
}
