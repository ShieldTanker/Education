using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEevnt : MonoBehaviour
{
    // ���ʹ� FSM ��ũ��Ʈ�� ����ϱ� ���� ����
    public EnemyFSM eFsm;

    // �÷��̾�� �������� ������ ���� �̺�Ʈ �Լ�
    public void PlayerHit()
    {
        eFsm.AttackAction();
    }
}
