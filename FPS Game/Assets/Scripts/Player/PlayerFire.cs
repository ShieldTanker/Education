using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // �ִϸ����� ����
    Animator anim;

    //���� ��� ����
    enum WeaponMode
    {
        Normal,
        Sniper,
    }
    // ī�޶� Ȯ�� Ȯ�ο� ����
    WeaponMode wMode;

    bool zoomMode = false;

    // ���� ��� �ؽ�Ʈ
    public Text wModeText;


    // �ѹ߻� ȿ�� ����
    public GameObject[] effects;

    private void Start()
    {
        // �ǰ� ����Ʈ ������Ʈ���� ��ƼŬ �ý��� ������Ʈ ��������
        ps = bulletEffect.GetComponent<ParticleSystem>();

        // �ڽĿ�����Ʈ�� �ִϸ����� �ҷ�����
        anim = GetComponentInChildren<Animator>();

        // ���� �ʱ� ��带 �븻 ���� ����
        wMode = WeaponMode.Normal;
    }

    private void Update()
    {
        // ���� ���°� '������' ���� �� ���� ������ �� �ְ� ��
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

                    // ����ź�� ������Ʈ�� Rigidbody ������Ʈ�� ������
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    // ī�޶��� ���� �������� ����ź�� �������� ���� ���� (Impulse : �������� ��)
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
                
                case WeaponMode.Sniper:
                    // ����, �� ��� ���°� �ƴ϶�� ī�޶� Ȯ�� �ϰ� �� ��� ���·� ����
                    if (!zoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        zoomMode = true;
                    }
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;
                    }
                    break;
                default:
                    break;
            }
        }

        // ���콺 ���� ��ư�� �Է�
        if (Input.GetMouseButtonDown(0))
        {
            // ���� �̵� ���� Ʈ�� �Ķ������ ���� 0 �̶��, ���� �ִϸ��̼� �ǽ�
            if (anim.GetFloat("moveMotion") == 0)
            {
                anim.SetTrigger("attack");
            }

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
            // �� ����Ʈ�� �ǽ�
            StartCoroutine(ShootEffectOn(0.05f));
        }

        // ���� Ű������ ���� 1�� �Է��� ������, ���� ��带 �Ϲ� ��Ʈ�� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;

            // �Ϲ� ��� �ؽ�Ʈ ���
            wModeText.text = "Normal Mode";

            // ī�޶��� ȭ���� �ٽ� ������� ������
            Camera.main.fieldOfView = 60;
            zoomMode = false;
        }
        // ���� Ű������ ���� 2���Ԥ����� ������, ���� ��带 �������� ���� ����
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            // �������� ��� �ؽ�Ʈ ���
            wModeText.text = "Sniper Mode";
        }
    }

    // �ѱ� ����Ʈ �ڷ�ƾ �Լ�
    IEnumerator ShootEffectOn(float duration)
    {
        // �����ϰ� ���ڸ� ����
        int num = Random.Range(0, effects.Length);
        // ����Ʈ ������Ʈ �迭���� ���� ���ڿ� �ش��ϴ� ����Ʈ ������Ʈ�� Ȱ��ȭ
        effects[num].SetActive(true);
        // ������ �ð� ��ŭ ��ٸ�
        yield return new WaitForSeconds(duration);
        // ����Ʈ ������Ʈ�� ��Ȱ��ȭ
        effects[num].SetActive(false);
    }
}
