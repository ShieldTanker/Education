using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFx : MonoBehaviour
{
    public Light gunLight;

    private void Update()
    {
        gunLight.range = Random.Range(4f, 10f);

        // 빛이 아닌 오브젝트의 크기 조절
        transform.localScale = Vector3.one * Random.Range(2f, 4f);

        // transform.localEulerAngles = Quatercion.Euler(270f, 0 , Random.Range) 와 같은 코드
        transform.localEulerAngles = new Vector3(270f, 0, Random.Range(0, 90f));

    }
}
