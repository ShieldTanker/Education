using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class PlayerFire : NetworkBehaviour
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

    // �ִϸ����� ����
    Animator anim;

    // ���� ��� ����
    enum WeaponMode
    {
        Normal,
        Sniper,
    }
    WeaponMode wMode;

    // ī�޶� Ȯ�� Ȯ�ο� ����
    bool zoomMode = false;

    // ���� ��� �ؽ�Ʈ
    public Text wModeText;

    // ���� ������ ��������Ʈ ����
    public GameObject weapon01;
    public GameObject weapon02;

    // ũ�ν���� ��������Ʈ ����
    public GameObject crosshair01;
    public GameObject crosshair02;

    // ���콺 ������ ��ư Ŭ�� ������ ��������Ʈ ����
    public GameObject weapon01_R;
    public GameObject weapon02_R;

    // �� �߻� ȿ�� ������Ʈ �迭
    public GameObject[] eff_Flash;

    // ���콺 ������ ��ư Ŭ�� �� ��� ��������Ʈ ����
    public GameObject crosshair02_zoom;

    public override void Spawned()
    {
        wModeText = GameManager.gm.wModeText;
        bulletEffect = GameManager.gm.bulletEffect;
        weapon01 = GameManager.gm.weapon01;
        weapon02 = GameManager.gm.weapon02;
        crosshair01 = GameManager.gm.crosshair01;
        crosshair02 = GameManager.gm.crosshair02;
        weapon01_R = GameManager.gm.weapon01_R;
        weapon02_R = GameManager.gm.weapon02_R;
        crosshair02_zoom = GameManager.gm.crosshair02_zoom;

        // �ǰ� ����Ʈ ������Ʈ���� ��ƼŬ �ý��� ������Ʈ ��������
        ps = bulletEffect.GetComponent<ParticleSystem>();

        // �ִϸ����� ������Ʈ ��������
        anim = GetComponentInChildren<Animator>();

        // ���� �ʱ� ��带 ��� ���� ����
        wMode = WeaponMode.Normal;

    }

    // �����ϰ� ó���ϴ� �����ӿ� �۵�
    public override void FixedUpdateNetwork()
    {
        // ���� ���°� '���� ��' ������ ���� ������ �� �ְ� ��
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;

        // ���콺 ������ ��ư�� �Է� ����
        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:
                    // ����ź ������Ʈ�� ������ �� ����ź�� ���� ��ġ�� �߻� ��ġ�� ����
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    // ����ź ������Ʈ�� rigidbody ������Ʈ�� ������
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    // ī�޶��� ���� �������� ����ź�� �������� ���� ����
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
                case WeaponMode.Sniper:
                    // ����, �� ��� ���°� �ƴ϶�� ī�޶� Ȯ���ϰ� �� ��� ���·� ����
                    if (!zoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        zoomMode = true;

                        // �� �ڵ��� �� ũ�ν��� ����
                        crosshair02_zoom.SetActive(true);
                        crosshair02.SetActive(false);
                    }
                    // �׷��� ������ ī�޶� ���� ���·� �ǵ����� �� ��� ���¸� ����
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;

                        // ũ�ν��� �������� ���� ��������
                        crosshair02_zoom.SetActive(false);
                        crosshair02.SetActive(true);
                    }
                    break;
            }
        }

        // ���콺 ���� ��ư�� �Է�
        if (Input.GetMouseButtonDown(0))
        {
            // ���� �̵� ���� Ʈ�� �Ķ������ ���� 0�̶��, ���� �ִϸ��̼� �ǽ�
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            // ���̸� ������ �� �߻�� ��ġ�� ���� ������ ����
            Ray ray = new Ray(
                Camera.main.transform.position,
                Camera.main.transform.forward);

            // ���̰� �ε��� ����� ������ ������ ������ ����
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                // ���� ���̿� �ε��� ����� ���̾ 'Enemy'��� ������ �Լ��� ����
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                // �׷��� �ʴٸ�, ���̿� �ε��� ������ �ǰ� ����Ʈ�� �÷���
                else
                {
                    // �ǰ� ����Ʈ�� ��ġ�� ���̰� �ε��� �������� �̵�
                    bulletEffect.transform.position = hitInfo.point;

                    // �ǰ� ����Ʈ�� forward ������ ���̰� �ε��� ������ ���� ���Ϳ� ��ġ ��Ŵ
                    bulletEffect.transform.forward = hitInfo.normal;

                    // �ǰ� ����Ʈ�� �÷���
                    ps.Play();
                }
            }
            // �� ����Ʈ�� �ǽ�
            StartCoroutine(ShootEffectOn(0.05f));
        }

        // ���� Ű������ ���� 1�� �Է��� ������, ���� ��带 �Ϲ� ���� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // �������� ��忡�� �Ϲ� ��� Ű�� �������� Weapon01_R_zoom�� ��Ȱ��ȭ, �ܸ��� ����
            crosshair02_zoom.SetActive(false);
            zoomMode = false;

            wMode = WeaponMode.Normal;

            // ī�޶��� ȭ���� �ٽ� ������� ������
            Camera.main.fieldOfView = 60f;

            // �Ϲ� ��� �ؽ�Ʈ ���
            wModeText.text = "Normal Mode";

            // 1�� ��������Ʈ�� Ȱ��ȭ, 2�� ��������Ʈ�� ��Ȱ��ȭ
            weapon01.SetActive(true);
            weapon02.SetActive(false);
            crosshair01.SetActive(true);
            crosshair02.SetActive(false);
            weapon01_R.SetActive(true);
            weapon02_R.SetActive(false);
        }
        // ���� Ű������ ���� 2�� �Է��� ������, ���� ��带 �������� ���� ����
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            // �������� ��� �ؽ�Ʈ ���
            wModeText.text = "Sniper Mode";

            // 1�� ��������Ʈ�� ��Ȱ��ȭ, 2�� ��������Ʈ�� Ȱ��ȭ
            weapon01.SetActive(false);
            weapon02.SetActive(true);
            crosshair01.SetActive(false);
            crosshair02.SetActive(true);
            weapon01_R.SetActive(false);
            weapon02_R.SetActive(true);
        }
    }

    // �ѱ� ����Ʈ �ڷ�ƾ �Լ�
    IEnumerator ShootEffectOn(float duration)
    {
        // �����ϰ� ���ڸ� ����
        int num = Random.Range(0, eff_Flash.Length);
        // ����Ʈ ������Ʈ �迭���� ���� ���ڿ� �ش��ϴ� ����Ʈ ������Ʈ�� Ȱ��ȭ
        eff_Flash[num].SetActive(true);
        // ������ �ð���ŭ ��ٸ�
        yield return new WaitForSeconds(duration);
        // ����Ʈ ������Ʈ�� �ٽ� ��Ȱ��ȭ
        eff_Flash[num].SetActive(false);
    }
}
