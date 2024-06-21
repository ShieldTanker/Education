using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class PlayerMove : NetworkBehaviour
{
    // �̵� �ӵ� ����
    public float moveSpeed = 7f;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

/*
    // �߷� ����
    float gravity = -20f;

    // ���� �ӵ� ����
    float yVelocity = 0;
*/
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

    public Transform camPosition;

    // ��Ʈ��ũ�� ĳ���� ��Ʈ�ѷ� ����
    NetworkCharacterControllerPrototype netCC;

    // ���� ��ư�Է� ������ ���� ����
    [Networked] private NetworkButtons _buttonsPrevious { get; set; }

    public override void Spawned()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        netCC = GetComponent<NetworkCharacterControllerPrototype>();  // cc �����ϰ� Net CC Proto �� �ٲ�

        // �ִϸ����� �޾ƿ���
        anim = GetComponentInChildren<Animator>();

        if (Object.HasInputAuthority)
        {// ��Ʈ��ũ ������Ʈ�� ��������� �ִ���
            GameManager.gm.player = this;
            hpSlider = GameManager.gm.hpSlider;

            // ���� ī�޶��� CamFollow ������Ʈ�� ������
            CamFollow cf = Camera.main.GetComponent<CamFollow>();
            cf.target = camPosition;
        }

        hitEffect = GameManager.gm.hitEffect;
    }

    // �����ϰ� ó���ϴ� �����ӿ� �۵�
    public override void FixedUpdateNetwork()
    {
        // ���� ���°� '���� ��' ������ ���� ������ �� �ְ� ��
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;

        if (GetInput(out NetworkInputData data))  // OnInput ���� �۽�(�Է�)�� ���� �޾ƿ��� data �� ��ȯ
        {
            // FixedUpdateNetwork() �� FixedDeltaTime �� �ƴ� Runner.DeltaTime �� ���
            netCC.Move(data.dir * moveSpeed * Runner.DeltaTime);

            if (data.Buttons.WasPressed(_buttonsPrevious, PlayerButtons.Jump))
            {// data ���� ��ư�� Jump ��° ������ư �� �ٸ���
                netCC.Jump(); // ���߿� ������ �����ȵǴ°Ͱ� ���� �Ǿ�����
            }

            _buttonsPrevious = data.Buttons;
        }
        // �̵� ���� Ʈ���� ȣ���ϰ� ������ ũ�� ���� �Ѱ���
        anim.SetFloat("MoveMotion", netCC.Velocity.magnitude);

        if (hpSlider != null)
        {
            // ���� �÷��̾� hp(%)�� hp �����̴��� value�� �ݿ�
            hpSlider.value = (float)hp / maxHp;
        }

/*        // ������� �Է��� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �̵� ������ ����
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

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
*/
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        // ���ʹ��� ���ݷ¸�ŭ �÷��̾��� ü���� ����
        hp -= damage;

        // ����, �÷��̾��� ü���� 0���� ũ��, ��Ʈ��ũ �Է±����� ������ �ǰ� ȿ���� ���
        if (Object.HasInputAuthority && hp > 0)
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
