using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadShot : MonoBehaviour
{
    public EnemyFSM eFsm;

    public void EnemyHeadShot()
    {
        Debug.Log("HeadShot");
        eFsm.HitEnemy(100);
    }
}
