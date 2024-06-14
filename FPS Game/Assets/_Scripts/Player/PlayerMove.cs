using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // �̵� �ӵ� ����
    public float moveSpeed = 7f;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    Coroutine lowHP;

    // �߷� ����
    float gravity = -20f;

    // ���� �ӵ� ����
    [SerializeField]
    float yVelocity = 0f;

    // ������ ����
    public float jumpPower = 10f;

    // ���� ���� ����
    public bool isJumping = false;

    // �÷��̾� ü��
    public int hp = 20;
    // �÷��̾� �ִ� ü�º���
    int maxHP = 20;

    // hp �����̴� ����
    public Slider hpSlider;

    // hit ȿ�� ������Ʈ
    public GameObject hitEffect;

    // �ִϸ����� ����
    Animator anim;

    private void Start()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ڽ� ������Ʈ�� �ִϸ����� �޾ƿ���
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // ���� ���°� '������' ���� �� ���� ������ �� �ְ� ��
        if (GameManager.GM.gState != GameManager.GameState.Run)
            return;

        // ������� �Է��� ����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // �̵� ������ ����
        Vector3 dir = new Vector3(h, 0, v);
        // nomalized : ������ �������� ���� 1��
        dir = dir.normalized;

        // �̵� ���� Ʈ���� ȣ���ϰ� ������ ũ�� ���� �Ѱ���(���� Ʈ���� �Ķ���ʹ� float��)
        // magnitude : ������ ����
        anim.SetFloat("moveMotion", dir.magnitude);

        // ���� ī�޶� �������� ������ �߰��� ��ȯ(Translate ���� �̹���� ����)
        // ������ �ϴ��� �ٶ󺸸� ���� ������
        dir = Camera.main.transform.TransformDirection(dir);

        // ���� �� �ٴڿ� ���� �������� Ȯ��
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // ���� �����ϰ� ����
            isJumping = false;

            // ĳ������ �����ӵ��� 0���� ����
            yVelocity = 0f;
        }

        // ���� Spacebar Ű�� �Է�������
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // ĳ���� ���� �ӵ��� ������ ����
            yVelocity = jumpPower;
            isJumping = true;
        }

        /* �̵� �ӵ��� ���� �̵� (ĳ���� ��Ʈ�ѷ��� �̵���)
        transform.position += dir * moveSpeed * Time.deltaTime; */

        // ĳ���� ���� �ӵ��� �߷� ���� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // �̵��ӵ��� ���� �̵�
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {// ���ʹ��� ���ݷ¸�ŭ ü���� ����
        hp -= damage;

        // ���� �÷��̾� hp(%)�� hp �����̴��� value�� �ݿ�
        hpSlider.value = (float)hp / maxHP;

        if (hp > 0)
        {
            // �ǰ� ����Ʈ �ڷ�ƾ�� ����
            StartCoroutine(PlayerHitEffect());

            if (hpSlider.value <= 0.15f)
                lowHP = StartCoroutine(LowHP());
        }
        else
        {
            Debug.Log("LowHP");
            StopCoroutine(lowHP);
        }
    }

    IEnumerator LowHP()
    {
        while (hpSlider.value < 0.15f)
        {
            // �ǰ� UI �� Ȱ��ȭ
            hitEffect.SetActive(true);

            yield return new WaitForSeconds(1);

            // �ǰ� UI ��Ȱ��ȭ
            hitEffect.SetActive(false);

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator PlayerHitEffect()
    {
        // �ǰ� UI �� Ȱ��ȭ
        hitEffect.SetActive(true);

        // 0.3 �ʰ� ���
        yield return new WaitForSeconds(0.3f);

        // �ǰ� UI ��Ȱ��ȭ
        hitEffect.SetActive(false);
    }
}