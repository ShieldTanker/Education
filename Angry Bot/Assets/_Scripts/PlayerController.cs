using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �÷��̾� �� ���� ���� Ȯ�ο�
public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Attack,
    Dead,
}

public class PlayerController : MonoBehaviour
{
    public PlayerState playerState;

    public Vector3 lookDirection;
    public float speed;
    public float walkSpeed;
    public float runSpeed;

    // �÷��̾�� LegacyŸ��
    private Animation anim;
    public AnimationClip idleAni;
    public AnimationClip walkAni;
    public AnimationClip runAni;

    // �̻��� �߻�
    public GameObject bullet;
    public GameObject shotFx;
    public Transform shotPoint;
    public AudioClip shotSound;
    private AudioSource audioSrc;

    private void Start()
    {
        // ����          ������
        playerState = PlayerState.Idle;

        anim = GetComponent<Animation>();
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        KeyboardInput();
        LookUpdate();

        AnimationUpdate();
    }

    void KeyboardInput()
    {
        float xx = Input.GetAxis("Horizontal");
        float zz = Input.GetAxis("Vertical");

        if (xx != 0 || zz != 0)
        {
            // �ɹ� ���� lookDirection = ���� ����
            lookDirection = (xx * Vector3.right) + (zz * Vector3.forward);
            speed = walkSpeed;
            playerState = PlayerState.Walk;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = runSpeed;
                playerState = PlayerState.Run;
            }
        }

        else if (playerState != PlayerState.Idle)
        {
            playerState = PlayerState.Idle;
            speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerState != PlayerState.Dead)
        {
            //StartCoroutine("Shot"); , StartCoroutine(Shot()) �� ����
            StartCoroutine(nameof(Shot));
        }
    }
    void LookUpdate()
    {
        // �ٶ�����ϴ� ���������� ȸ���� ���
        Quaternion r = Quaternion.LookRotation(lookDirection);
        
        // �ٶ�����ϴ� �������� õõ�� �ٶ󺸰� (���� ȸ����, ��ǥ ȸ���� , ȸ���� ����)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, r, 600f * Time.deltaTime);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void AnimationUpdate()
    {
        switch (playerState)
        {
            // �ִϸ��̼��� idleAni �ȿ� ����ִ� �̸��� ���� �ִϸ��̼� ���� 0.2�� ��ġ�� ��ȯ
            case PlayerState.Idle:      anim.CrossFade(idleAni.name,0.2f);
                break;
            case PlayerState.Walk:      anim.CrossFade(walkAni.name,0.2f);
                break;
            case PlayerState.Run:       anim.CrossFade(runAni.name,0.2f);
                break;
            case PlayerState.Attack:    anim.CrossFade(idleAni.name,0.2f);
                break;
            case PlayerState.Dead:      anim.CrossFade(idleAni.name, 0.2f);
                break;
        }
    }

    IEnumerator Shot()
    {
        GameObject bulletObj = 
            Instantiate(
                bullet, 
                shotPoint.position,
                // ��ٶ� �Ѿ��̱⿡ �ٶ󺸴¹��� ���� �ʿ�
                Quaternion.LookRotation(shotPoint.forward));

        // �ش� ������Ʈ�� bullet ������Ʈ �� �浹 ����
        Physics.IgnoreCollision(
            // �Ѿ� ������Ʈ �� BoxCollider ������ Collider �� �θ� ���� �̱⿡ ��� ���� 
            bulletObj.GetComponent<Collider>(), 
            GetComponent<Collider>());

        audioSrc.clip = shotSound;
        audioSrc.Play();

        shotFx.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        shotFx.SetActive(false);
    }
}
