using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // �߻� ��ġ
    public GameObject firePosition;

    // ��ô ���� ������Ʈ
    public GameObject bombFactory;

    // ��ô �Ŀ�
    public float throwPower = 15f;

    // �ǰ� ����Ʈ ������Ʈ
    public GameObject bulletEffect;

    // �ǰ� ����Ʈ ��ƼŬ �ý���
    ParticleSystem ps;

    // �߻� ���� ���ݷ�
    public int weaponPower = 5;

    private void Start()
    {
        // �ǰ� ����Ʈ ������Ʈ���� ��ƼŬ �ý��� ������Ʈ ��������
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        // ���� ���°� '������' ���� �� ���� ������ �� �ְ� ��
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;

        // ���콺 ������ ��ư�� �Է� ����
        if (Input.GetMouseButtonDown(1))
        {
            // ����ź ������Ʈ�� ������ �� ����ź�� ���� ��ġ�� �߻� ��ġ�� ����
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            // ����ź�� ������Ʈ�� Rigidbody ������Ʈ�� ������
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            // ī�޶��� ���� �������� ����ź�� �������� ���� ���� (Impulse : �������� ��)
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }

        // ���콺 ���� ��ư�� �Է�
        if (Input.GetMouseButtonDown(0))
        {
            // ���̸� ������ �� �߻�� ��ġ�� ���� ������ ����
            Ray ray = new Ray(
                Camera.main.transform.position,
                Camera.main.transform.forward);

            // ���̰� �΋L�� ����� ������ ������ ������ ����
            RaycastHit hitInfo;
            if (Physics.Raycast(ray,out hitInfo))
            {
                // ���� ���̿� �ε��� ����� ���̾ "Enemy" ��� ������ �Լ��� ����
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFsm = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFsm.HitEnemy(weaponPower);
                }
                // �׷��� �ʴٸ� ���̿� �ε��� ������ �ǰ� ����Ʈ �÷���
                else
                {
                    // �ǰ� ����Ʈ�� ��ġ�� ���̰� �ε��� �������� �̵�
                    bulletEffect.transform.position = hitInfo.point;

                    /* �ǰ� ����Ʈ ��  forward ������ ���̰�
                     �ε��� ������ ����(�΋H�� ���� ����) ���Ϳ� ��ġ ��Ŵ */
                    bulletEffect.transform.forward = hitInfo.normal;

                    // �ǰ� ����Ʈ �÷���
                    ps.Play();
                }
            }
        }
    }
}
