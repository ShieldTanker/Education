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

    private void Start()
    {
        // 변수          열거형
        playerState = PlayerState.Idle;

        anim = GetComponent<Animation>();
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
}
