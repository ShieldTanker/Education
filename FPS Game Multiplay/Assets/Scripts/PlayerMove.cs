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

    // �߷� ����
    float gravity = -20f;

    // ���� �ӵ� ����
    float yVelocity = 0;

    // ������ ����
    public float jumpPower = 10f;

    // ���� ���� ����
    public bool isJumping = false;

    // �÷��̾� ü�� ����
    public int hp = 20;

    // �ִ� ü�� ����
    int maxHp = 20;

    // hp �����̴� ����
    public Slider hpSlider;

    // Hit ȿ�� ������Ʈ
    public GameObject hitEffect;

    // �ִϸ����� ����
    Animator anim;

    private void Start()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ִϸ����� �޾ƿ���
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // ���� ���°� '���� ��' ������ ���� ������ �� �ְ� ��
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;

        // ������� �Է��� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �̵� ������ ����
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // �̵� ���� Ʈ���� ȣ���ϰ� ������ ũ�� ���� �Ѱ���
        anim.SetFloat("MoveMotion", dir.magnitude);

        // ���� ī�޶� �������� ������ ��ȯ
        dir = Camera.main.transform.TransformDirection(dir);

        // ����, ���� ���̾���, �ٽ� �ٴڿ� �����ߴٸ�
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // ���� �� ���·� �ʱ�ȭ
            isJumping = false;

            // ĳ���� ���� �ӵ��� 0���� ����
            yVelocity = 0;
        }

        // ���� Ű���� Spacebar Ű�� �Է��ߴٸ�
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // ĳ���� ���� �ӵ��� �������� ����
            yVelocity = jumpPower;
            isJumping = true;
        }

        // ĳ���� ���� �ӵ��� �߷� ���� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // �̵� �ӵ��� ���� �̵�
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // ���� �÷��̾� hp(%)�� hp �����̴��� value�� �ݿ�
        hpSlider.value = (float)hp / maxHp;
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        // ���ʹ��� ���ݷ¸�ŭ �÷��̾��� ü���� ����
        hp -= damage;

        // ����, �÷��̾��� ü���� 0���� ũ�� �ǰ� ȿ���� ���
        if (hp > 0)
        {
            // �ǰ� ����Ʈ �ڷ�ƾ�� ����
            StartCoroutine(PlayHitEffect());
        }
    }

    // �ǰ� ȿ�� �ڷ�ƾ �Լ�
    IEnumerator PlayHitEffect()
    {
        // �ǰ� UI�� Ȱ��ȭ
        hitEffect.SetActive(true);
        // 0.3�ʰ� ���
        yield return new WaitForSeconds(0.3f);
        // �ǰ� UI�� ��Ȱ��ȭ
        hitEffect.SetActive(false);
    }
}
