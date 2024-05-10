using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 플레이어 의 현재 상태 확인용
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

    // 플레이어는 Legacy타입
    private Animation anim;
    public AnimationClip idleAni;
    public AnimationClip walkAni;
    public AnimationClip walkLeft;
    public AnimationClip walkRight;
    public AnimationClip runAni;

    // 미사일 발사
    public GameObject bullet;
    public GameObject shotFx;
    public Transform shotPoint;
    public AudioClip shotSound;
    private AudioSource audioSrc;

    // 플레이어 체력 관련
    public Slider lifeBar;
    public float maxHp;
    public float hp;

    private void Start()
    {
        // 변수          열거형
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
                // 맴버 변수 lookDirection = 보는 방향
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
                //StartCoroutine("Shot"); , StartCoroutine(Shot()) 과 같음
                StartCoroutine(nameof(Shot));
            }
        }
    }
    void LookUpdate()
    {
        // 바라봐야하는 방향으로의 회전값 계산
        Quaternion r = Quaternion.LookRotation(lookDirection);
        
        // 바라봐야하는 방향으로 천천히 바라보게 (현재 회전값, 목표 회전값 , 회전할 각도)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, r, 600f * Time.deltaTime);
        
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void AnimationUpdate()
    {
        if (beforState == playerState)
            return;

        switch (playerState)
        {
            // 애니메이션이 idleAni 안에 들어있는 이름을 가진 애니메이션 으로 0.2초 겹치게 전환
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
                // 기다란 총알이기에 바라보는방향 조정 필요
                Quaternion.LookRotation(shotPoint.forward));

        // 해당 오브젝트와 bullet 오브젝트 의 충돌 무시
        Physics.IgnoreCollision(
            // 총알 오브젝트 는 BoxCollider 이지만 Collider 가 부모 관계 이기에 상관 없음 
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
