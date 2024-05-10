using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// �÷��̾� �� ���� ���� Ȯ�ο�
public enum PlayerState
{
    Idle,
    Walk,
    WalkLeft,
    WalkRight,
    Run,
    Attack,
    Dead,
}

public class PlayerController : MonoBehaviour
{
    public PlayerState playerState;
    PlayerState beforState;
    public PlayManager pm;

    public Vector3 lookDirection;
    Vector3 moveDirection;
    public float speed;
    public float walkSpeed;
    public float runSpeed;

    // �÷��̾�� LegacyŸ��
    private Animation anim;
    public AnimationClip idleAni;
    public AnimationClip walkAni;
    public AnimationClip walkLeft;
    public AnimationClip walkRight;
    public AnimationClip runAni;

    // �̻��� �߻�
    public GameObject bullet;
    public GameObject shotFx;
    public Transform shotPoint;
    public AudioClip shotSound;
    private AudioSource audioSrc;

    // �÷��̾� ü�� ����
    public Slider lifeBar;
    public float maxHp;
    public float hp;

    private void Start()
    {
        // ����          ������
        playerState = PlayerState.Idle;

        anim = GetComponent<Animation>();
        audioSrc = GetComponent<AudioSource>();

        anim.CrossFade(idleAni.name, 0.2f);
    }

    private void Update()
    {
        if (playerState != PlayerState.Dead)
        {
            KeyboardInput();
            LookUpdate();
        }
        AnimationUpdate();
    }

    void KeyboardInput()
    {
        float xx = Input.GetAxis("Horizontal");
        float zz = Input.GetAxis("Vertical");

        if (playerState != PlayerState.Attack)
        {
            if ((xx != 0 || zz != 0) && (playerState != PlayerState.WalkLeft && playerState != PlayerState.WalkRight))
            {
                // �ɹ� ���� lookDirection = ���� ����
                lookDirection = (xx * Vector3.right) + (zz * Vector3.forward);
                speed = walkSpeed;
                playerState = PlayerState.Walk;
                moveDirection = Vector3.forward;

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    speed = runSpeed;
                    playerState = PlayerState.Run;
                }
            }

            else if (Input.GetKey(KeyCode.Q))
            {
                speed = walkSpeed;
                playerState = PlayerState.WalkLeft;
                moveDirection = Vector3.left;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                speed = walkSpeed;
                playerState = PlayerState.WalkRight;
                moveDirection = Vector3.right;
            }

            else if (playerState != PlayerState.Idle)
            {
                playerState = PlayerState.Idle;
                speed = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !pm.playEnd)
            {
                //StartCoroutine("Shot"); , StartCoroutine(Shot()) �� ����
                StartCoroutine(nameof(Shot));
            }
        }
    }
    void LookUpdate()
    {
        // �ٶ�����ϴ� ���������� ȸ���� ���
        Quaternion r = Quaternion.LookRotation(lookDirection);
        
        // �ٶ�����ϴ� �������� õõ�� �ٶ󺸰� (���� ȸ����, ��ǥ ȸ���� , ȸ���� ����)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, r, 600f * Time.deltaTime);
        
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void AnimationUpdate()
    {
        if (beforState == playerState)
            return;

        switch (playerState)
        {
            // �ִϸ��̼��� idleAni �ȿ� ����ִ� �̸��� ���� �ִϸ��̼� ���� 0.2�� ��ġ�� ��ȯ
            case PlayerState.Idle:
                anim.CrossFade(idleAni.name,0.2f);
                break;

            case PlayerState.Walk:
                anim.CrossFade(walkAni.name,0.2f);
                anim[walkAni.name].speed = 1;
                break;

             case PlayerState.WalkLeft:
                anim.CrossFade(walkLeft.name, 0.2f);
                break;

            case PlayerState.WalkRight:
                anim.CrossFade(walkRight.name, 0.2f);
                break;

            case PlayerState.Run:
                anim.CrossFade(runAni.name,0.2f);
                anim[runAni.name].speed = 2;
                break;

            case PlayerState.Attack:
                anim.CrossFade(idleAni.name,0.2f);
                break;

            case PlayerState.Dead:
                anim.CrossFade(idleAni.name, 0.2f);
                break;
        }

        beforState = playerState;
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

        playerState = PlayerState.Attack;
        speed = 0f;

        yield return new WaitForSeconds(0.15f);
        shotFx.SetActive(false);

        yield return new WaitForSeconds(0.15f);
        playerState = PlayerState.Idle;
    }

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            lifeBar.value = hp / maxHp;
        }

        if (hp <= 0)
        {
            speed = 0;
            playerState = PlayerState.Dead;
            
            PlayManager pm = GameObject.Find("PlayManager").GetComponent<PlayManager>();
            pm.GameOver();
        }
    }
}
