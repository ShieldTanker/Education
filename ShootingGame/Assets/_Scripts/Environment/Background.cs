using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    //硅版 付磐府倔
    public Material bgMaterial;

    //胶农费 加档
    public float scrollSpeed;

    private void Update()
    {
        //胶农费 规氢
        Vector2 difrection = Vector2.up;

        //胶农费
        bgMaterial.mainTextureOffset += difrection * scrollSpeed * Time.deltaTime;
    }
}