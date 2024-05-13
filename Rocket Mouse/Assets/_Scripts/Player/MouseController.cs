using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MouseController : MonoBehaviour
{
    public float jetpackForce;
    public ParticleSystem jetpack;

    public float fowardMovementSpeed;

    /*
    public float plusSpeed;
    public float levelUpTime;
    */

    //강사님 풀이 코드
    public float feverFloat;
    public float feverForEnd;
    public bool isFever;

    Coroutine feverCoroutine;
    
    // 강사님 풀이 코드
    public float lvUpFloat;
    private float lvUpTime;
    private int lv;
    public TMP_Text levelTMP;

    private Rigidbody2D rb;

    //플레이어 땅에 닿았는지 여부 및 애니메이션 설정
    public Transform groundCheckTransform;
    //레이어 정보만 저장
    public LayerMask groundCheckLayerMask;
    private bool grounded;
    private Animator animator;


    // 플레이어 사망 여부
    bool dead = false;
    public bool died;

    // 강사님 풀이코드
    public int lifeCnt;
    private float invincibleTimeCnt;
    public SpriteRenderer sp;


    // 코인 점수 관련
    private uint coins = 0;
    public TMP_Text textCoins;

    //레벨관련
    private int level = 1;
    public TMP_Text levelsText;

    
    public GameObject buttonRestart;
    public GameObject goToMenuBtn;

    // 소리 관련
    public AudioClip coinCollectSound;
    
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;

    public AudioSource bgmVolumes;
    public AudioSource bgMusicAudio;

    // 배경 이미지 관련
    public ParallaxScroll parallaxScroll;


    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        textCoins.text = coins.ToString();

        // 강사님 풀이 코드
        lv = 1; 
        levelTMP.text = $"Lv.{lv}";

        levelsText.text = "Levels : " + level;
        //병렬로 실행되게 하는 메소드
        
        // StartCoroutine(LevelCount());
        // StartCoroutine(FeverTime());
        
        LoadVolume();
        //bgmVolumes.volume = PlayerPrefs.GetFloat("bgmSlider", 1f);

        feverCoroutine = StartCoroutine(FeverCtrl());
    }

    private void FixedUpdate()
    {
        died = dead;
        bool jetpackActive = Input.GetButton("Fire1");


        if (!dead)
        {
            if (jetpackActive)
            {
                rb.AddForce(jetpackForce * Vector2.up);
            }

            Vector2 newVelocity = rb.velocity;

            //떨어지는속도는 점점 빠르게 하는걸 유지 하기 위해 x 값만 고정값으로 적용
            newVelocity.x = fowardMovementSpeed;
            rb.velocity = newVelocity;
        }

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
        
        DisplayButtons();

        AdjustFootstepsAndJetpackSound(jetpackActive);

        parallaxScroll.offset = transform.position.x;
    }

    private void Update()
    {
        if (!dead)
        {
            lvUpTime += Time.deltaTime;

            if (lvUpTime >= lvUpFloat)
            {
                lv++;
                levelTMP.text = $"Lv.{lv}";
                // Debug.Log(lv);
                if (!isFever)
                    fowardMovementSpeed = 2.5f + lv * 0.5f;
                lvUpTime = 0;
            }

            if (invincibleTimeCnt > 0)
                invincibleTimeCnt -= Time.deltaTime;
        } 
    }


    // 제트팩 이펙트
    private void AdjustJetpack(bool jetpackActive)
    {
        /* 타입이 길어서 var 로 함
           Particle 컴포넌트의 Emission 에 접근하기위함               */
        var emission = jetpack.emission;
        // 바닥에 닿으면 비활성
        emission.enabled = !grounded;
        // jetpackActive 가 참이면 파티클 생성량 300 아니면 75
        emission.rateOverTime = jetpackActive ? 300f : 75f;
    }


    // 바닥에 닿았는지 확인
    private void UpdateGroundedStatus()
    {
        /*Physics2D.OverlapCircle : 가상의 원을 만들고 물체가 닿았는지 확인
          (원을 만들 위치, 원의 크기, 확인할 레이어(해당 레이어만 확인)
          닿았으면 참 아니면 거짓             */
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        animator.SetBool("grounded", grounded);
    }

    // 코인에 닿았을때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coins")
        {
            ColectCoins(collision);
        }
        else
        {
            HitByLaser(collision);
        }
    }


    //코인 먹었을때
    private void ColectCoins(Collider2D coinCollider)
    {
        ++coins;
        textCoins.text = coins.ToString();

        Destroy(coinCollider.gameObject);

        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }


    // 레이저에 닿았을때
    private void HitByLaser(Collider2D laserCollider)
    {
        if (invincibleTimeCnt > 0)
            return;
        if (!dead)
        {
            AudioSource laser = laserCollider.GetComponent<AudioSource>();
            laser.Play();
        }

        --lifeCnt;
        if (lifeCnt > 0)
        {
            invincibleTimeCnt = 3.0f;
            StartCoroutine(InvincibleTime());
            return;
        }

        dead = true;
        animator.SetBool("dead", true);

        StopCoroutine(feverCoroutine);
    }



    /* 강사님 풀이코드
       죽었을때 재시작 버튼 활성화
    */
    private void DisplayButtons()
    {
        // gameObject.activeSelf 가 활성화 되었는지 여부
        bool active = buttonRestart.activeSelf;

        // !active 없어도 죽었을떄 정상작동하지만 계속 if문이 돌기에 넣어줌
        if (grounded && dead && !active)
        {
            buttonRestart.gameObject.SetActive(true);
            goToMenuBtn.gameObject.SetActive(true);
        }
    }


    /*
       내가 한 풀이
    // 죽었을때 메뉴 버튼 활성화
    public void DisplayMenuButton()
    {
        bool active = goToMenuBtn.gameObject.activeSelf;

        if (dead && grounded && !active)
        {
            goToMenuBtn.gameObject.SetActive(true);
        }
    }
    */

    // 재시작 버튼눌렀을때
    public void OnClickedRestartButton()
    {
        //LoadScene(SceneManager.GetActiveScene().name) = 현재 열려있는 씬 의 이름으로 씬을 불러옴
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 메뉴 버튼 눌렀을때
    public void OnClickedMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }


    // 제트팩, 발소리 
    private void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    {
        footstepsAudio.enabled = !dead && grounded;
        jetpackAudio.enabled = !dead && !grounded;
        jetpackAudio.volume = jetpackActive ? 1f : 0.5f;
    }

    private void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat("BGMVolume", 1);
        bgMusicAudio.volume = volume;
    }

    /*//병렬로 실행되는 코드 에 쓰려면 IEnumerator 타입이어야함
    IEnumerator LevelCount()
    {
        while (!dead)
        {
            // yield return 이 없으면 안됨
            // 코드진행이 2초 멈추는 코드
            yield return new WaitForSeconds(levelUpTime);
            
            fowardMovementSpeed += plusSpeed;

            levelsText.text = "Level : " + ++level;

            Debug.Log("레벨 증가");
        }
    }*/

    IEnumerator InvincibleTime()
    {
        for (int i = 0; i < 3; i++)
        {
            sp.color = new Color(0.5f, 0, 0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            sp.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }


    IEnumerator FeverCtrl()
    {
        while (true)
        {
            yield return new WaitForSeconds(feverFloat);
            Debug.Log("피버 시작");
            if (dead)
                break;

            isFever = true;
            fowardMovementSpeed = 10f;
            
            GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
            foreach (var obj in lasers)
                obj.SetActive(false);

            yield return new WaitForSeconds(feverForEnd);
            Debug.Log("피버 종료");
            isFever = false;
            fowardMovementSpeed = 2.5f + lv * 0.5f;
        }
    }
}
