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

    //����� Ǯ�� �ڵ�
    public float feverFloat;
    public float feverForEnd;
    public bool isFever;

    Coroutine feverCoroutine;
    
    // ����� Ǯ�� �ڵ�
    public float lvUpFloat;
    private float lvUpTime;
    private int lv;
    public TMP_Text levelTMP;

    private Rigidbody2D rb;

    //�÷��̾� ���� ��Ҵ��� ���� �� �ִϸ��̼� ����
    public Transform groundCheckTransform;
    //���̾� ������ ����
    public LayerMask groundCheckLayerMask;
    private bool grounded;
    private Animator animator;


    // �÷��̾� ��� ����
    bool dead = false;
    public bool died;

    // ����� Ǯ���ڵ�
    public int lifeCnt;
    private float invincibleTimeCnt;
    public SpriteRenderer sp;


    // ���� ���� ����
    private uint coins = 0;
    public TMP_Text textCoins;

    //��������
    private int level = 1;
    public TMP_Text levelsText;

    
    public GameObject buttonRestart;
    public GameObject goToMenuBtn;

    // �Ҹ� ����
    public AudioClip coinCollectSound;
    
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;

    public AudioSource bgmVolumes;
    public AudioSource bgMusicAudio;

    // ��� �̹��� ����
    public ParallaxScroll parallaxScroll;


    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        textCoins.text = coins.ToString();

        // ����� Ǯ�� �ڵ�
        lv = 1; 
        levelTMP.text = $"Lv.{lv}";

        levelsText.text = "Levels : " + level;
        //���ķ� ����ǰ� �ϴ� �޼ҵ�
        
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

            //�������¼ӵ��� ���� ������ �ϴ°� ���� �ϱ� ���� x ���� ���������� ����
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


    // ��Ʈ�� ����Ʈ
    private void AdjustJetpack(bool jetpackActive)
    {
        /* Ÿ���� �� var �� ��
           Particle ������Ʈ�� Emission �� �����ϱ�����               */
        var emission = jetpack.emission;
        // �ٴڿ� ������ ��Ȱ��
        emission.enabled = !grounded;
        // jetpackActive �� ���̸� ��ƼŬ ������ 300 �ƴϸ� 75
        emission.rateOverTime = jetpackActive ? 300f : 75f;
    }


    // �ٴڿ� ��Ҵ��� Ȯ��
    private void UpdateGroundedStatus()
    {
        /*Physics2D.OverlapCircle : ������ ���� ����� ��ü�� ��Ҵ��� Ȯ��
          (���� ���� ��ġ, ���� ũ��, Ȯ���� ���̾�(�ش� ���̾ Ȯ��)
          ������� �� �ƴϸ� ����             */
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        animator.SetBool("grounded", grounded);
    }

    // ���ο� �������
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


    //���� �Ծ�����
    private void ColectCoins(Collider2D coinCollider)
    {
        ++coins;
        textCoins.text = coins.ToString();

        Destroy(coinCollider.gameObject);

        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }


    // �������� �������
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



    /* ����� Ǯ���ڵ�
       �׾����� ����� ��ư Ȱ��ȭ
    */
    private void DisplayButtons()
    {
        // gameObject.activeSelf �� Ȱ��ȭ �Ǿ����� ����
        bool active = buttonRestart.activeSelf;

        // !active ��� �׾����� �����۵������� ��� if���� ���⿡ �־���
        if (grounded && dead && !active)
        {
            buttonRestart.gameObject.SetActive(true);
            goToMenuBtn.gameObject.SetActive(true);
        }
    }


    /*
       ���� �� Ǯ��
    // �׾����� �޴� ��ư Ȱ��ȭ
    public void DisplayMenuButton()
    {
        bool active = goToMenuBtn.gameObject.activeSelf;

        if (dead && grounded && !active)
        {
            goToMenuBtn.gameObject.SetActive(true);
        }
    }
    */

    // ����� ��ư��������
    public void OnClickedRestartButton()
    {
        //LoadScene(SceneManager.GetActiveScene().name) = ���� �����ִ� �� �� �̸����� ���� �ҷ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // �޴� ��ư ��������
    public void OnClickedMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }


    // ��Ʈ��, �߼Ҹ� 
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

    /*//���ķ� ����Ǵ� �ڵ� �� ������ IEnumerator Ÿ���̾����
    IEnumerator LevelCount()
    {
        while (!dead)
        {
            // yield return �� ������ �ȵ�
            // �ڵ������� 2�� ���ߴ� �ڵ�
            yield return new WaitForSeconds(levelUpTime);
            
            fowardMovementSpeed += plusSpeed;

            levelsText.text = "Level : " + ++level;

            Debug.Log("���� ����");
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
            Debug.Log("�ǹ� ����");
            if (dead)
                break;

            isFever = true;
            fowardMovementSpeed = 10f;
            
            GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
            foreach (var obj in lasers)
                obj.SetActive(false);

            yield return new WaitForSeconds(feverForEnd);
            Debug.Log("�ǹ� ����");
            isFever = false;
            fowardMovementSpeed = 2.5f + lv * 0.5f;
        }
    }
}
