using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    // ���ʹ� ��ũ��Ʈ ������Ʈ�� ����ϱ� ���� ����
    public EnemyFSM eFsm;

    // �÷��̾�� �������� ������ ���� �̺�Ʈ �Լ�
    public void PlayerHit()
    {
        eFsm.AttackAction();
    }
}
