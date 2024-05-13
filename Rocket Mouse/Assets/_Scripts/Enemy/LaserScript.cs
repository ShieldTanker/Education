using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private bool isLaserOn = true;

    SpriteRenderer spriteRenderer;
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;
    private Collider2D collider2d;
    public float rotationSpeed;

    public float interval;
    private float timeUntilNextToggle;

    private void Start()
    {
        timeUntilNextToggle = interval;

        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        timeUntilNextToggle -= Time.fixedDeltaTime;
        
        if (timeUntilNextToggle <= 0)
        {
            isLaserOn = !isLaserOn;
        
            //Collider2D 컴포넌트 활성화 여부
            collider2d.enabled = isLaserOn;

            if (isLaserOn)
                spriteRenderer.sprite = laserOnSprite;
            else
                spriteRenderer.sprite = laserOffSprite;

            timeUntilNextToggle = interval;
        }

        /* Rotate 는 중심점 기준 으로 회전
           RotateAround 는 설정한 지점을 기준으로 회전시킬때 사용
           (설정 위치, 회전할 축, 회전할 각도) */
        transform.RotateAround(transform.position,
            Vector3.forward,
            rotationSpeed * Time.fixedDeltaTime);
    }
}
