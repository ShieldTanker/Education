using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // 게임 플레이 하는동안 계속 기억해야할 정보
    public string UserId { get; set; }

    private void Start()
    {
        // 스타트 는 오브젝트가 생성될때 실행됨
        // 이미 생성된 오브젝트는 다른씬에서 다시와도 스타트 실행 안함
        int count = FindObjectsOfType<PlayerData>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}