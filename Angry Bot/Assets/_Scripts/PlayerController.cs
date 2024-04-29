using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어 의 현재 상태 확인용
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

    // 플레이어는 Legacy타입
    private Animation anim;
    public AnimationClip idleAni;
    public AnimationClip walkAni;
    public AnimationClip runAni;

    // 미사일 발사
    public GameObject bullet;
    public GameObject shotFx;
    public Transform shotPoint;
    public AudioClip shotSound;
    private AudioSource audioSrc;

    private void Start()
    {
        // 변수          열거형
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
            // 맴버 변수 lookDirection = 보는 방향
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
            //StartCoroutine("Shot"); , StartCoroutine(Shot()) 과 같음
            StartCoroutine(nameof(Shot));
        }
    }
    void LookUpdate()
    {
        // 바라봐야하는 방향으로의 회전값 계산
        Quaternion r = Quaternion.LookRotation(lookDirection);
        
        // 바라봐야하는 방향으로 천천히 바라보게 (현재 회전값, 목표 회전값 , 회전할 각도)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, r, 600f * Time.deltaTime);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void AnimationUpdate()
    {
        switch (playerState)
        {
            // 애니메이션이 idleAni 안에 들어있는 이름을 가진 애니메이션 으로 0.2초 겹치게 전환
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

        yield return new WaitForSeconds(0.15f);

        shotFx.SetActive(false);
    }
}
