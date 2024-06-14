using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public AudioClip gunSound;
    public AudioSource audio;

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

    // ���� ������ ��������Ʈ ����
    public GameObject weapon01;
    public GameObject weapon02;

    // ũ�ν���� ��������Ʈ ����
    public GameObject crossHair01;
    public GameObject crossHair02;

    // ���콺 ������ ��ư Ŭ�� ������ ��������Ʈ ����
    public GameObject weapon01_R;
    public GameObject weapon02_R;

    // �ѹ߻� ȿ�� ����
    public GameObject[] effects;

    // ���콺 ������ ��ư Ŭ�� �� ��� ��������Ʈ ����
    public GameObject crosshair02_zoom;

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
        if (GameManager.GM.gState != GameManager.GameState.Run)
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

                        // �� ��� �϶� ũ�ν��� ����
                        crosshair02_zoom.SetActive(true);
                        crossHair02.SetActive(false);
                    }
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;

                        // ũ�ν��� �������� ���� �������� 
                        crosshair02_zoom.SetActive(false);
                        crossHair02.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }

        // ���콺 ���� ��ư�� �Է�
        if (Input.GetMouseButtonDown(0))
        {
            audio.clip = gunSound;
            audio.Play();

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
            if (Physics.Raycast(ray, out hitInfo))
            {
                // ���� ���̿� �ε��� ����� ���̾ "Enemy" ��� ������ �Լ��� ����
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFsm = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFsm.HitEnemy(weaponPower);
                }
                else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("EnemyHead"))
                {
                    HeadShot eHead = hitInfo.transform.GetComponent<HeadShot>();
                    eHead.EnemyHeadShot();
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
            // �������� ��忡�� �Ϲ� ��� Ű�� �������� Weapone01_R_zoom �� ��Ȱ��ȭ
            crosshair02_zoom.SetActive(false);
            zoomMode = false;

            wMode = WeaponMode.Normal;

            // �Ϲ� ��� �ؽ�Ʈ ���
            wModeText.text = "Normal Mode";

            // ī�޶��� ȭ���� �ٽ� ������� ������
            Camera.main.fieldOfView = 60;

            // 1�� ��������Ʈ�� Ȱ��ȭ, 2�� ��������Ʈ�� ��Ȱ��ȭ
            weapon01.SetActive(true);
            weapon01_R.SetActive(true);
            crossHair01.SetActive(true);

            weapon02.SetActive(false);
            weapon02_R.SetActive(false);
            crossHair02.SetActive(false);
        }
        // ���� Ű������ ���� 2���Է��� ������, ���� ��带 �������� ���� ����
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            // �������� ��� �ؽ�Ʈ ���
            wModeText.text = "Sniper Mode";

            // 2�� ��������Ʈ�� Ȱ��ȭ, 1�� ��������Ʈ�� ��Ȱ��ȭ
            weapon02.SetActive(true);
            weapon02_R.SetActive(true);
            crossHair02.SetActive(true);

            weapon01.SetActive(false);
            weapon01_R.SetActive(false);
            crossHair01.SetActive(false);
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
